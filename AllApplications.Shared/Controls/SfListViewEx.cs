using Syncfusion.ListView.XForms;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.Controls
{
    public class SfListViewEx : SfListView
    {
        public static BindableProperty ItemTappedCommandProperty = BindableProperty.Create("ItemTappedCommand", typeof(ICommand), typeof(SfListViewEx), null);

        public SfListViewEx()
        {
            this.ItemTapped += this.OnItemTappped;
        }

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)this.GetValue(ItemTappedCommandProperty); }
            set { this.SetValue(ItemTappedCommandProperty, value); }
        }

        private void OnItemTappped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (e.ItemData != null && this.ItemTappedCommand != null && this.ItemTappedCommand.CanExecute(e.ItemData))
            {
                this.ItemTappedCommand.Execute(e.ItemData);
            }
        }
    }
}