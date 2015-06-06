using System;
using Windows.UI.Xaml.Data;

namespace HandsOn.Converter
{
    public sealed class WaveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is HandsOn.Models.KinectModel.ResultState)
            {
                if ((HandsOn.Models.KinectModel.ResultState)value == HandsOn.Models.KinectModel.ResultState.Lock)
                {
                    return new Uri("ms-appx:///Assets/17goo.wav");
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}