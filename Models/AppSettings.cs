using Core;
using Core.Controls;

namespace SampleApplication.Models
{
    public interface IAppCache
    {
        GeoLocation CurrentLocation { get; set; }
    }

    public class AppCache : ModelBase, IAppCache
    {
        private GeoLocation _currentLocation;

        public GeoLocation CurrentLocation
        {
            get { return _currentLocation; }
            set { SetProperty(ref _currentLocation, value); }
        }
    }
}