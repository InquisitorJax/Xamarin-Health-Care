using Core;

namespace SampleApplication.Models
{
    public class HealthCareUser : ModelBase
    {
        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }
    }
}