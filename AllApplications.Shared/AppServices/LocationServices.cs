using Core.Controls;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.AppServices
{
    public interface ILocationService
    {

        bool HasGpsPermissions { get; set; }

        Task<FetchCurrentLocationResult> FetchCurrentLocationAsync();

        List<GeoLocation> GenerateRandomLocations(GeoLocation fromLocation, int radiusInMeters, int numberOfLocations = 1);
    }

    public class FetchCurrentLocationResult : CommandResult
    {
        public GeoLocation CurrentLocation { get; set; }
    }

    public class LocationService : ILocationService
    {
        private readonly IGeolocator _locationService;

        public bool HasGpsPermissions { get; set; }

        public LocationService(IGeolocator locationService)
        {
            _locationService = locationService;
        }

        public async Task<FetchCurrentLocationResult> FetchCurrentLocationAsync()
        {
            var retResult = new FetchCurrentLocationResult();

            if (!_locationService.IsGeolocationAvailable)
            {
                //TODO BM: Request location to be enabled?
                retResult.Notification.Add(new NotificationItem("Geolocation is not available on this device"));
            }

            if (!_locationService.IsGeolocationEnabled)
            {
                //TODO BM: Request location to be enabled?
                retResult.Notification.Add(new NotificationItem("Geolocation has not been enabled"));
            }

            if (retResult.IsValid())
            {
                try
                {
                    GeoLocation location = new GeoLocation();                    

                    //TODO: make part of request
                    _locationService.DesiredAccuracy = 25;
                    int timeout = 10000;

                    //if (_locationService.IsGeolocationAvailable)
                    if (HasGpsPermissions)
                    {
                        Position position = await _locationService.GetPositionAsync(timeout);

                        if (position != null)
                        {
                            location = new GeoLocation
                            {
                                Latitude = position.Latitude,
                                Longitude = position.Longitude,
                                TimeStamp = position.Timestamp,
                                Description = "Current Location"
                            };
                        }
                    }
                    else
                    {
                        //TODO: device doesn't have Gps - or location detection denied - allow user to select location from map and store in settings
                        //dummy code:
                        location = new GeoLocation
                        {
                            Latitude = 23.3,
                            Longitude = 23.3,
                            TimeStamp = DateTimeOffset.Now,
                            Description = "Current Location"
                        };
                    }

                    retResult.CurrentLocation = location;
                }
                catch
                {
                    retResult.Notification.Add("Unable to get location :(");
                }
            }

            return retResult;
        }

        public List<GeoLocation> GenerateRandomLocations(GeoLocation fromLocation, int radiusInMeters, int numberOfLocations = 1)
        {
            //from https://stackoverflow.com/questions/33976732/generate-random-latlng-given-device-location-and-radius

            if (fromLocation == null || numberOfLocations <= 0)
            {
                return new List<GeoLocation>();
            }

            List<GeoLocation> randomPoints = new List<GeoLocation>();
            GeoLocation myLocation = fromLocation;

            for (int i = 0; i < numberOfLocations; i++)
            {
                double x0 = fromLocation.Latitude;
                double y0 = fromLocation.Longitude;

                Random random = new Random();

                // Convert radius from meters to degrees
                double radiusInDegrees = radiusInMeters / 111300f;

                double u = random.NextDouble();
                double v = random.NextDouble();
                double w = radiusInDegrees * Math.Sqrt(u);
                double t = 2 * Math.PI * v;
                double x = w * Math.Cos(t);
                double y = w * Math.Sin(t);

                // Adjust the x-coordinate for the shrinking of the east-west distances
                double new_x = x / Math.Cos(y0);

                double foundLatitude = new_x + x0;
                double foundLongitude = y + y0;

                GeoLocation randomLocation = new GeoLocation { Latitude = foundLatitude, Longitude = foundLongitude };
                randomPoints.Add(randomLocation);
            }

            return randomPoints;
        }
    }
}