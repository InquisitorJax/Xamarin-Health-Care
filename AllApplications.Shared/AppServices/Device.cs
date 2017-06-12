using System;

namespace Core.AppServices
{
    public interface IDevice
    {
        void OpenUri(Uri uri);
    }

    public class Device : IDevice
    {
        public void OpenUri(Uri uri)
        {
            Xamarin.Forms.Device.OpenUri(uri);
        }
    }
}