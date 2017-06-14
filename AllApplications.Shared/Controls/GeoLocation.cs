using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core.Controls
{
    public class GeoLocation : ModelBase
    {
        private string _description;
        private double _latitude;
        private double _longitude;
        private DateTimeOffset? _timeStamp;

        private string _title;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }

        public DateTimeOffset? TimeStamp
        {
            get { return _timeStamp; }
            set { SetProperty(ref _timeStamp, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public static bool AreEqual(GeoLocation location1, GeoLocation location2)
        {
            if (ReferenceEquals(location1, null) ^ ReferenceEquals(location2, null))
                return false;

            return ReferenceEquals(location1, location2) || (location1.Latitude == location2.Latitude && location1.Longitude == location2.Longitude);
        }

        public static GeoLocation FromWellKnownText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else
            {
                try
                {
                    GeoLocation location = new GeoLocation();

                    int firstParenth = text.IndexOf("(") + 1;
                    int secondParent = text.IndexOf(")");
                    string locationString = text.Substring(firstParenth, secondParent - firstParenth).Trim();
                    string[] locations = locationString.Split(' ');
                    double latitude = double.Parse(locations[1]);
                    double longitude = double.Parse(locations[0]);

                    location.Latitude = latitude;
                    location.Longitude = longitude;

                    return location;
                }
                catch (Exception ex)
                {
                    //typeof(GeoLocation).GetLogger().Error("unable to create geolocation from " + text, ex);
                    return null;
                }
            }
        }

        public static bool IsInCloseProximity(GeoLocation location1, GeoLocation location2, int rounding = 4)
        {
            if (ReferenceEquals(location1, null) ^ ReferenceEquals(location2, null))
                return false;

            return ReferenceEquals(location1, location2) || (Math.Round(location1.Latitude, rounding) == Math.Round(location2.Latitude, rounding)
                && Math.Round(location1.Longitude, rounding) == Math.Round(location2.Longitude, rounding));
        }

        public bool IsInPolygon(IList<GeoLocation> polygonLocations)
        {
            //based in PIP: https://en.wikipedia.org/wiki/Point_in_polygon
            int i, j;
            bool c = false;
            for (i = 0, j = polygonLocations.Count - 1; i < polygonLocations.Count; j = i++)
            {
                if ((((polygonLocations[i].Latitude <= Latitude) && (Latitude < polygonLocations[j].Latitude))
                        || ((polygonLocations[j].Latitude <= Latitude) && (Latitude < polygonLocations[i].Latitude)))
                        && (Longitude < (polygonLocations[j].Longitude - polygonLocations[i].Longitude) * (Latitude - polygonLocations[i].Latitude)
                            / (polygonLocations[j].Latitude - polygonLocations[i].Latitude) + polygonLocations[i].Longitude))

                    c = !c;
            }

            return c;
        }

        public override string ToString()
        {
            return SerializeToString();
        }

        public string ToWellKnownText()
        {
            return string.Format(CultureInfo.InvariantCulture, "POINT ({0})", SerializeToString());
        }

        private string SerializeToString()
        {
            const string template = "{0} {1} {2} {3}";
            string longitude = Longitude.ToString(CultureInfo.InvariantCulture);
            string latitude = Latitude.ToString(CultureInfo.InvariantCulture);
            string altitude = string.Empty;
            string measure = string.Empty;
            //if (Altitude.HasValue)
            //{
            //	altitude = " " + Altitude.Value.ToString(CultureInfo.InvariantCulture);
            //	if (Measure.HasValue)
            //	{
            //		measure = " " + Measure.Value.ToString(CultureInfo.InvariantCulture);
            //	}
            //}

            return string.Format(CultureInfo.InvariantCulture, template, longitude, latitude, altitude, measure);
        }
    }
}