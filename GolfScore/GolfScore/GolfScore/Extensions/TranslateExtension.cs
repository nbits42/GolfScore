using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using TeeScore.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Extensions
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private readonly CultureInfo _ci;
        private const string ResourceId = "TeeScore.Translations.Labels";

        public TranslateExtension()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                _ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, _ci);

            if (string.IsNullOrEmpty(translation))
            {
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
            }
            return translation;
        }
    }
}