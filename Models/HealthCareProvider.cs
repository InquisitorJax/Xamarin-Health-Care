using Core;

namespace SampleApplication.Models
{
    public class HealthCareProvider : ModelBase
    {
        private string _description;

        private string _imageName;
        private string _location;
        private string _name;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string ImageName
        {
            get { return _imageName; }
            set { SetProperty(ref _imageName, value); }
        }

        public string Location
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