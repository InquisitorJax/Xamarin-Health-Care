using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public class ViewModelBase : BindableBase, IViewModel
    {
        private string _busyMessage;
        private bool _isBusy;

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        protected INavigationService Navigation
        {
            get { return CC.Navigation; }
        }

        protected IUserNotifier UserNotifier
        {
            get { return CC.UserNotifier; }
        }

        protected async Task Close()
        {
            await Navigation.GoBack();
        }

        protected void NotBusy()
        {
            IsBusy = false;
            BusyMessage = null;
        }

        protected void ShowBusy(string message)
        {
            IsBusy = true;
            BusyMessage = message;
        }

        #region IViewModel implementation

        public virtual void Closing()
        {
        }

        public virtual Task InitializeAsync(Dictionary<string, string> args)
        {
            return Task.FromResult(default(int));
        }

        public virtual Task LoadStateAsync()
        {
            return Task.FromResult(default(int));
        }

        public virtual Task SaveStateAsync()
        {
            return Task.FromResult(default(int));
        }

        #endregion IViewModel implementation
    }
}