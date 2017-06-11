using System;
using XLabs.Forms.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using Prism.Services;

namespace TestXForms
{
	public class MainPageViewModel : ViewModel
	{
		public MainPageViewModel ()
		{
			TestCommand = new DelegateCommand(Test);
		}

		public ICommand TestCommand { get; private set;}

		private void Test()
		{
			CurrentPageTracker.CurrentPage.Navigation.PushAsync (new NavPage ());
			//PageDialogService dialog = new PageDialogService ();
			//dialog.DisplayAlert ("Command", "Fired!", "OK" );
		}
	}
}

