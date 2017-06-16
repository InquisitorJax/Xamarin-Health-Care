using Prism.Mvvm;
using SQLite;

namespace Core
{
    public abstract class ModelBase : BindableBase
    {
        private string _id;

        private bool _isSelected;

        [PrimaryKey, Unique]
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [Ignore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }
}