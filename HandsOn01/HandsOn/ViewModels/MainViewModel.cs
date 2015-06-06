using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;

namespace HandsOn.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Models.KinectModel Model;

        public MainViewModel()
        {
            this.Model = new Models.KinectModel();
            this.Model.PropertyChanged += Model_PropertyChanged;
        }

        public String Message
        {
            get { return this.Model.Message; }
            set
            {
                this.Model.Message = value;
            }
        }

        public ImageSource ColorImageElement
        {
            get { return this.Model.ColorImageElement; }
            set
            {
                this.Model.ColorImageElement = value;
            }
        }

        public void KinectStart()
        {
            this.Model.KinectStart();
        }

        public void KinectStop()
        {
            this.Model.KinectStop();
        }


        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
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
