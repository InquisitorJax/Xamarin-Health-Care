using Core.AppServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryEx : ContentView
    {
        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(EntryEx), null, BindingMode.TwoWay, propertyChanged: OnEntryTextChanged);

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryEx), false, BindingMode.Default, null, (bindable, oldValue, newValue) =>
            {
                var ctrl = (EntryEx)bindable;
                ctrl.IsPassword = (bool)newValue;
            });

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(EntryEx), null);

        public static readonly BindableProperty ValidationPropertyNameProperty = BindableProperty.Create(nameof(ValidationPropertyName), typeof(string), typeof(EntryEx), null);

        public static readonly BindableProperty ValidationTypeNameProperty = BindableProperty.Create(nameof(ValidationTypeName), typeof(string), typeof(EntryEx), null);

        public static BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryEx), Keyboard.Default,
                BindingMode.Default, null, (bindable, oldValue, newValue) =>
                {
                    var ctrl = (EntryEx)bindable;
                    ctrl._entry.Keyboard = (Keyboard)newValue;
                });

        public EntryEx()
        {
            InitializeComponent();
            _entry.Focused += Entry_Focused;
            _entry.Unfocused += Entry_Unfocused;
        }

        public string EntryText
        {
            get { return (string)GetValue(EntryTextProperty); }
            set { SetValue(EntryTextProperty, value); }
        }

        public bool IsPassword
        {
            get { return (bool)base.GetValue(Entry.IsPasswordProperty); }
            set
            {
                base.SetValue(Entry.IsPasswordProperty, value);
                _entry.IsPassword = value;
            }
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public string ValidationPropertyName
        {
            get { return (string)GetValue(ValidationPropertyNameProperty); }
            set { SetValue(ValidationPropertyNameProperty, value); }
        }

        public string ValidationTypeName
        {
            get { return (string)GetValue(ValidationTypeNameProperty); }
            set { SetValue(ValidationTypeNameProperty, value); }
        }

        //TODO: Make use of Validation Callbacks? https://developer.xamarin.com/guides/xamarin-forms/xaml/bindable-properties/#validation
        private static async void OnEntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var entry = (EntryEx)bindable;
            if (!string.IsNullOrEmpty((string)newValue))
            {
                int y = Xamarin.Forms.Device.RuntimePlatform == Platforms.Android ? -15 : -20;
                await entry._label.TranslateTo(-2, y);
            }
            else
            {
                await entry._label.TranslateTo(-2, 5);
            }
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            _label.TextColor = Color.Accent;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            _label.TextColor = Color.Default;
        }
    }
}