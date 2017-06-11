using Core;

namespace SampleApplication
{
    public class SampleItem : ModelBase
    {
        private string _description;
        private string _name;

        private string _password;

        private byte[] _picture;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public byte[] Picture
        {
            get { return _picture; }
            set { SetProperty(ref _picture, value); }
        }
    }
}