using Core;

namespace SampleApplication.Models
{
    public class HealthCareUser : ModelBase
    {
        private string _description;
        private bool _isLoggedIn;

        private string _name;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}