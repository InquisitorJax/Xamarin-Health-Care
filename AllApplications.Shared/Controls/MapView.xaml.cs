using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Core.Controls
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentView
    {
        public static readonly BindableProperty LocationsProperty = BindableProperty.Create("Locations", typeof(ObservableCollection<GeoLocation>), typeof(MapView), new ObservableCollection<GeoLocation>(), BindingMode.TwoWay, null, OnLocationsChanged);
        public static readonly BindableProperty MapCenterProperty = BindableProperty.Create("MapCenter", typeof(GeoLocation), typeof(MapView), null, BindingMode.TwoWay, null, OnMapCenterChanged);

        private readonly IDictionary<GeoLocation, Pin> _locationMappings;

        public MapView()
        {
            InitializeComponent();
            _locationMappings = new Dictionary<GeoLocation, Pin>();
        }

        public ObservableCollection<GeoLocation> Locations
        {
            get { return (ObservableCollection<GeoLocation>)GetValue(LocationsProperty); }
            set
            {
                SetValue(LocationsProperty, value);
                if (Locations != null)
                {
                    Locations.CollectionChanged -= Locations_CollectionChanged;
                }
                SetValue(LocationsProperty, value);
                if (Locations != null)
                {
                    Locations.CollectionChanged += Locations_CollectionChanged;
                }

                UpdateLocationPins();
            }
        }

        public GeoLocation MapCenter
        {
            get { return (GeoLocation)GetValue(MapCenterProperty); }
            set
            {
                SetValue(MapCenterProperty, value);
                MyMap.CenterMap(MapCenter);
            }
        }

        private static void OnLocationsChanged(BindableObject instance, object oldValue, object newValue)
        {
            MapView map = (MapView)instance;
            map.Locations = (ObservableCollection<GeoLocation>)newValue;
        }

        private static void OnMapCenterChanged(BindableObject instance, object oldValue, object newValue)
        {
            MapView map = (MapView)instance;
            map.MapCenter = (GeoLocation)newValue;
        }

        private void AddLocationPin(GeoLocation location)
        {
            if (_locationMappings.ContainsKey(location))
                return;

            var position = new Position(location.Latitude, location.Longitude);

            string labelText = location.Title ?? "location";
            string addressText = location.Description ?? null;

            var pin = new Pin
            {
                Position = position,
                Label = labelText,
                Address = addressText,
                Type = PinType.Place
            };

            _locationMappings.Add(location, pin);
            MyMap.Pins.Add(pin);
        }

        private void ClearMappings()
        {
            MyMap.Pins.Clear();
            _locationMappings.Clear();
        }

        private void Locations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MyMap.Pins.Clear();
            UpdateLocationPins();
        }

        private void MyMap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRegion")
            {
                MyMap.CenterMap(MapCenter);
            }
        }

        private void UpdateLocationPins()
        {
            _locationMappings.Clear();
            MyMap.Pins.Clear();
            if (Locations != null)
            {
                foreach (var location in Locations)
                {
                    AddLocationPin(location);
                }
            }
            MyMap.PositionMapToPins();
        }
    }
}