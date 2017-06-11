using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestXForms
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
			BindingContext = new MainPageViewModel ();
			_btnNav.Clicked += Nav_Clicked;

			CurrentPageTracker.CurrentPage = this;
		}

		void Nav_Clicked (object sender, EventArgs e)
		{
			this.Navigation.PushAsync (new NavPage ());
		}


	}
}

