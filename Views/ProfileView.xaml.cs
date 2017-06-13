using Core.Controls;
using SampleApplication.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileView : CommandView
    {
        //public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(ProfileView), null);
        //public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ProfileView), null);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(HealthCareUser), typeof(ProfileView), null);

        public ProfileView()
        {
            InitializeComponent();
        }

        //public ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}

        //public object CommandParameter
        //{
        //    get { return GetValue(CommandParameterProperty); }
        //    set { SetValue(CommandParameterProperty, value); }
        //}

        public HealthCareUser Value
        {
            get { return (HealthCareUser)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}