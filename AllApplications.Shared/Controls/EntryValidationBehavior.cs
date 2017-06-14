using Autofac;
using Xamarin.Forms;

namespace Core.Controls
{
    /*
        *	<Entry.Behaviors>
               <behaviour:EntryValidationBehaviour ValidationPropertyName="GivenName" ValidationTypeName="Identity"/>
           </Entry.Behaviors>
        */

    public class EntryValidationBehaviour : Behavior<Entry>
    {
        //see: https://blog.xamarin.com/behaviors-in-xamarin-forms/

        #region FirstDeclareThese

        public static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EntryValidationBehaviour), true);
        public static readonly BindablePropertyKey ValidationMessagePropertyKey = BindableProperty.CreateReadOnly("ValidationMessage", typeof(string), typeof(EntryValidationBehaviour), null);

        #endregion FirstDeclareThese

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;
        public static readonly BindableProperty ValidationMessageProperty = ValidationMessagePropertyKey.BindableProperty;
        public static readonly BindableProperty ValidationPropertyNameProperty = BindableProperty.Create("ValidationPropertyName", typeof(string), typeof(EntryValidationBehaviour), null);
        public static readonly BindableProperty ValidationTypeNameProperty = BindableProperty.Create("ValidationTypeName", typeof(string), typeof(EntryValidationBehaviour), null, BindingMode.OneWay, null, OnValidationTypeNameChanged);
        private Entry _entry;
        private IModelValidator _validator;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public string ValidationMessage
        {
            get { return (string)base.GetValue(ValidationMessageProperty); }
            private set { base.SetValue(ValidationMessagePropertyKey, value); }
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

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            _entry = bindable;
            if (_entry != null)
            {
                _entry.TextChanged += Entry_TextChanged;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            if (_entry != null)
            {
                _entry.TextChanged -= Entry_TextChanged;
            }
        }

        private static void OnValidationTypeNameChanged(BindableObject instance, object oldValue, object newValue)
        {
            var behavior = (EntryValidationBehaviour)instance;
            string typeName = (string)newValue;
            if (!string.IsNullOrEmpty(typeName) && behavior != null)
            {
                object item;
                bool result = CC.IoC.TryResolveNamed(typeName, typeof(IModelValidator), out item);
                behavior._validator = result ? (IModelValidator)item : default(IModelValidator);
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_validator != null)
            {
                var entry = (Entry)sender;
                var validationResult = _validator.ValidateProperty(entry.BindingContext, ValidationPropertyName);
                IsValid = validationResult == null;
                ValidationMessage = validationResult == null ? null : validationResult.Message;
            }
        }
    }
}