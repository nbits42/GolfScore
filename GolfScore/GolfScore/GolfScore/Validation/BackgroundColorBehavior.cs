using Xamarin.Forms;

namespace TeeScore.Validation
{
    public static class BackgroundColorBehavior
    {
        private static void OnApplyBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
            {
                return;
            }

            bool hasLine = (bool)newValue;
            view.BackgroundColor = hasLine ? Color.Red : Color.Transparent;
        }


        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundProperty", typeof(Color), typeof(BackgroundColorBehavior), null);
    }
}
