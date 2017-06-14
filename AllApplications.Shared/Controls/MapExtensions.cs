using System;
using System.Linq;
using Xamarin.Forms.Maps;

namespace Core.Controls
{
    public static class MapExtensions
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        public static void CenterMap(this Map map, GeoLocation mapCenter, double defaultZoom = 2)
        {
            if (mapCenter == null)
                return;

            var position = new Position(mapCenter.Latitude, mapCenter.Longitude);

            Distance radius = map.VisibleRegion == null ? Distance.FromMiles(defaultZoom) : map.VisibleRegion.Radius;

            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, radius));
        }

        public static void PositionMapToPins(this Map map)
        {
            //adapted from: http://adventuresinxamarinforms.com/2015/07/07/adding-a-bindable-map-with-the-map-behavior/

            var positions = map.Pins.Select(x => x.Position).ToList();

            if (positions == null || !positions.Any()) return;

            var centerPosition = new Position(positions.Average(x => x.Latitude), positions.Average(x => x.Longitude));

            var minLongitude = positions.Min(x => x.Longitude);
            var minLatitude = positions.Min(x => x.Latitude);

            var maxLongitude = positions.Max(x => x.Longitude);
            var maxLatitude = positions.Max(x => x.Latitude);

            double distance = 2;

            if (positions.Count > 1)
            {
                distance = CalculateDistance(minLatitude, minLongitude, maxLatitude, maxLongitude, 'M') / 2;
            }

            map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));

            //Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            //		{
            //			map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));
            //			return false;
            //		});
        }

        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}