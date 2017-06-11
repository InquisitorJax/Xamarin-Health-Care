using System.Windows.Input;
using Xamarin.Forms;

namespace Core
{
    /// <summary>
    /// ListView class that supports executing command when an item is tapped
    /// </summary>
    public class ListView : Xamarin.Forms.ListView
    {
        //public static BindableProperty ItemClickCommandProperty = BindableProperty.Create<ListView, ICommand>(x => x.ItemClickCommand, null);
        public static BindableProperty ItemTapCommandProperty = BindableProperty.Create("ItemTapCommand", typeof(ICommand), typeof(ListView), null);

        public ListView()
        {
            this.ItemTapped += this.OnItemTapped;
        }

        public ICommand ItemTapCommand
        {
            get { return (ICommand)this.GetValue(ItemTapCommandProperty); }
            set { this.SetValue(ItemTapCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.ItemTapCommand != null && this.ItemTapCommand.CanExecute(e.Item))
            {
                this.ItemTapCommand.Execute(e.Item);
                this.SelectedItem = null;
            }
        }
    }
}