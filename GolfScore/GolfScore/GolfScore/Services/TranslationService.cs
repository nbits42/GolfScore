using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using GolfScore.Contracts;
using GolfScore.Extensions;
using Xamarin.Forms;

namespace GolfScore.Services
{
    public class TranslationService
    {
        private const string ResourceId = "XRallyResults.Strings.Strings";
        private static CultureInfo _ci = null;

        public static string Translate(string text)
        {
            return TranslateInternal(text);
        }

        public string TranslateText(string text, params object[] parameters)
        {
            return Translate(text, parameters);
        }

        public string TranslateText(string text)
        {
            return Translate(text);
        }

        public static string Translate(string text, params object[] parameters)
        {
            var result = TranslateInternal(text);
            try
            {
                return string.Format(result, parameters);

            }
            catch (Exception)
            {
                return result;
            }
        }

        private static string TranslateInternal(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            var resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            var translation = resmgr.GetString(text, Ci);

            if (string.IsNullOrEmpty(translation))
            {
                translation = text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
            }
            return translation;
        }

        private static CultureInfo Ci
        {
            get {
                if (_ci != null) return _ci;

                _ci = new CultureInfo("nl");
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                {
                    _ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                }
                return _ci;
            }
        }
    }

    public interface ITranslationService
    {
        string Translate(string text);
        string Translate(string text, params object[] parameters);
    }
}
