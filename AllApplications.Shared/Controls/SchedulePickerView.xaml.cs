using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePicker : ContentView
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(SchedulePicker), null);

        private DateTime _selectedDateTime;
        private Button _selectionButton;

        public SchedulePicker()
        {
            InitializeComponent();
            schedule.CellTapped += Schedule_CellTapped;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private void Schedule_CellTapped(object sender, Syncfusion.SfSchedule.XForms.CellTappedEventArgs args)
        {
            if (_selectionButton != null)
            {
                _selectionButton.Clicked -= SelectionButton_Clicked;
            }

            _selectionButton = schedule.SelectionView as Button;
            _selectedDateTime = args.Datetime;

            if (_selectionButton != null)
            {
                _selectionButton.Clicked += SelectionButton_Clicked;
            }
        }

        private void SelectionButton_Clicked(object sender, EventArgs e)
        {
            //TODO: How to get start AND end time from selection?
            var result = new DateTimeResult { SelectedDate = _selectedDateTime };
            Command.Execute(result);
        }
    }
}