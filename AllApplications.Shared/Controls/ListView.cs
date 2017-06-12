using System.Windows.Input;
using Xamarin.Forms;

namespace Core.Controls
{
    /// <summary>
    /// ListView class that supports executing command when an item is tapped
    /// </summary>
    public class ListView : Xamarin.Forms.ListView
    {
        public static BindableProperty EnableSelectionProperty = BindableProperty.Create("EnableSelection", typeof(bool), typeof(ListView), false);
        public static BindableProperty ItemClickCommandProperty = BindableProperty.Create("ItemClickCommand", typeof(ICommand), typeof(ListView), null);

        public ListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            this.ItemTapped += this.OnItemTapped;
        }

        public ListView(ListViewCachingStrategy cacheStrategy) : base(cacheStrategy)
        {
            this.ItemTapped += this.OnItemTapped;
        }

        public bool EnableSelection
        {
            get { return (bool)this.GetValue(EnableSelectionProperty); }
            set { this.SetValue(EnableSelectionProperty, value); }
        }

        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { this.SetValue(ItemClickCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.ItemClickCommand != null && this.ItemClickCommand.CanExecute(e.Item))
            {
                this.ItemClickCommand.Execute(e.Item);
            }

            if (!EnableSelection)
            {
                this.SelectedItem = null;
            }
        }
    }
}