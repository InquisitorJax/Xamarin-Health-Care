using Syncfusion.ListView.XForms;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.Controls
{
    public class SfListViewEx : SfListView
    {
        public static BindableProperty ItemTappedCommandProperty = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(SfListViewEx), null);
        public static BindableProperty LeftSwipeCommandProperty = BindableProperty.Create(nameof(LeftSwipeCommand), typeof(ICommand), typeof(SfListViewEx), null);
        public static BindableProperty RightSwipeCommandProperty = BindableProperty.Create(nameof(RightSwipeCommand), typeof(ICommand), typeof(SfListViewEx), null);

        public SfListViewEx()
        {
            this.ItemTapped += this.OnItemTappped;
            this.SwipeEnded += SfListViewEx_SwipeEnded;
        }

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)this.GetValue(ItemTappedCommandProperty); }
            set { this.SetValue(ItemTappedCommandProperty, value); }
        }

        public ICommand LeftSwipeCommand
        {
            get { return (ICommand)this.GetValue(LeftSwipeCommandProperty); }
            set { this.SetValue(LeftSwipeCommandProperty, value); }
        }

        public ICommand RightSwipeCommand
        {
            get { return (ICommand)this.GetValue(RightSwipeCommandProperty); }
            set { this.SetValue(RightSwipeCommandProperty, value); }
        }

        private void OnItemTappped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (e.ItemData != null && this.ItemTappedCommand != null && this.ItemTappedCommand.CanExecute(e.ItemData))
            {
                this.ItemTappedCommand.Execute(e.ItemData);
            }
        }

        private void SfListViewEx_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (e.ItemData != null && e.SwipeOffset > 180)
            {
                if (e.SwipeDirection == SwipeDirection.Left && RightSwipeCommand != null && RightSwipeCommand.CanExecute(e.ItemData))
                {
                    RightSwipeCommand.Execute(e.ItemData);
                    ResetSwipe();
                }
                else if (e.SwipeDirection == SwipeDirection.Right && LeftSwipeCommand != null && LeftSwipeCommand.CanExecute(e.ItemData))
                {
                    LeftSwipeCommand.Execute(e.ItemData);
                    ResetSwipe();
                }
            }
        }
    }
}