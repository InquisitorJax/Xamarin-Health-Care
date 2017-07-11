using Xamarin.Forms;

namespace Core.Controls
{
    public class EditorEx : Editor
    {
        //see: http://forums.xamarin.com/discussion/21951/allow-the-editor-control-to-grow-as-content-lines-are-added#latest
        public EditorEx()
        {
            MaxHeight = double.MaxValue;
            //NOTE: will only work if HeightRequest is not set
            this.TextChanged += (sender, e) => { this.InvalidateMeasure(); };
        }

        public double MaxHeight { get; set; }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (heightConstraint > MaxHeight)
                heightConstraint = MaxHeight;

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        //renderer for iOS - see also https://forums.xamarin.com/discussion/70355/automatically-resize-editor-until-maxheight-is-reached
        //public class EditorExRenderer : EditorRenderer
        //{
        //    protected override void OnElementChanged(ElementChangedEventArgs e)
        //    {
        //        base.OnElementChanged(e);

        //        if (Control != null)
        //        {
        //            Control.ScrollEnabled = false;
        //        }
        //    }
        //}
    }
}