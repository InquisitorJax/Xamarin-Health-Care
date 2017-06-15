using System;

namespace Core.AppServices
{
    public interface IMapService
    {
        Notification LaunchNativeMap(Place place);
    }

    public class MapService : IMapService
    {
        public Notification LaunchNativeMap(Place place)
        {
            var retResult = Notification.Success();
            // Windows Phone doesn't like ampersands in the names and the normal URI escaping doesn't help
            var name = place.Name.Replace("&", "and"); // var name = Uri.EscapeUriString(place.Name);
            var loc = string.Format("{0},{1}", place.Location.Latitude, place.Location.Longitude);

            var addr = string.IsNullOrEmpty(place.Address) ? null : Uri.EscapeUriString(place.Address);

            string request = null;
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Platforms.iOS:
                    // iOS doesn't like %s or spaces in their URLs, so manually replace spaces with +s
                    request = string.Format("http://maps.apple.com/maps?q={0}&sll={1}", name.Replace(' ', '+'), loc);
                    break;

                case Platforms.Android:
                    // pass the address to Android if we have it
                    request = string.Format("geo:0,0?q={0}({1})", string.IsNullOrWhiteSpace(addr) ? loc : addr, name);
                    break;

                case Platforms.Windows:
                    request = string.Format("bingmaps:?cp={0}&q={1}", loc, name);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(request))
            {
                Xamarin.Forms.Device.OpenUri(new Uri(request));
            }
            else
            {
                retResult.Add("Could not launch maps for platform " + Xamarin.Forms.Device.RuntimePlatform);
            }

            return retResult;
        }
    }

    public class Place : ModelBase
    {
        private string _address;

        private Controls.GeoLocation _location;

        private string _name;

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        public Controls.GeoLocation Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}