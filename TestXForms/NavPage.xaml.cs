using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestXForms
{
	public partial class NavPage : ContentPage
	{
		public NavPage ()
		{
			InitializeComponent ();
			CurrentPageTracker.CurrentPage = this;
		}
	}
}

