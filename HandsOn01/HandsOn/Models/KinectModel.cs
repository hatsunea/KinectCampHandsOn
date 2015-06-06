using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;

namespace HandsOn.Models
{
    public class KinectModel : INotifyPropertyChanged
    {
        private KinectSensor Kinect = null;
        private MultiSourceFrameReader Reader = null;

        private WriteableBitmap ColorImageBitmap = null;

        private String _Message = null;
        public String Message
        {
            get { return this._Message; }
            set
            {
                this._Message = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _ColorImageElement = null;
        public ImageSource ColorImageElement
        {
            get { return this._ColorImageElement; }
            set
            {
                this._ColorImageElement = value;
                OnPropertyChanged();
            }
        }

        public void KinectStart()
        {
            try
            {
                this.Kinect = KinectSensor.GetDefault();
                var colorFrameDescription = this.Kinect.ColorFrameSource.FrameDescription;
                this.Kinect.IsAvailableChanged += Kinect_IsAvailableChanged;
                this.Kinect.Open();
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
        }

        public void KinectStop()
        {
            try
            {
                this.Kinect.Close();
                this.ColorImageBitmap = null;
                this.ColorImageElement = null;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
        }

        private void Kinect_IsAvailableChanged(KinectSensor sender, IsAvailableChangedEventArgs e)
        {
            try
            {
                if (e.IsAvailable)
                {
                    if (this.Reader == null)
                    {
                        var colorFrameDescription = this.Kinect.ColorFrameSource.FrameDescription;

                        this.Message = "Kinect Connected";
                        this.Reader = this.Kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Body);
                        this.Reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                        //RGB
                        this.ColorImageBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
        }

        private void Reader_MultiSourceFrameArrived(MultiSourceFrameReader sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                var frameReference = e.FrameReference;
                var multiSourceFrame = frameReference.AcquireFrame();

                if (multiSourceFrame != null)
                {
                    using (var frame = multiSourceFrame.ColorFrameReference.AcquireFrame())
                    {
                        if (frame != null)
                        {
                            var frameDescription = frame.FrameDescription;
                            if ((frameDescription.Width == this.ColorImageBitmap.PixelWidth) && (frameDescription.Height == this.ColorImageBitmap.PixelHeight))
                            {
                                if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
                                {
                                    frame.CopyRawFrameDataToBuffer(this.ColorImageBitmap.PixelBuffer);
                                }
                                else
                                {
                                    frame.CopyConvertedFrameDataToBuffer(this.ColorImageBitmap.PixelBuffer, ColorImageFormat.Bgra);
                                }
                                this.ColorImageBitmap.Invalidate();
                                this.ColorImageElement = this.ColorImageBitmap;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

