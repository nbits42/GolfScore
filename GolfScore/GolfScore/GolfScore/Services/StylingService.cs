using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TeeScore.Services
{
    public class StylingService
    {
        public static Color GetColor(string key)
        {
            var result = Color.Transparent;
            var resource = Application.Current.Resources[key];
            if (resource is Color color)
            {
                return color;
            }
            var colorString = resource as string;
            if (string.IsNullOrEmpty(colorString))
            {
                return result;
            }

            return colorString.StartsWith("#") 
                ? Color.FromHex(colorString.Substring(1)) 
                : result;
        }

        public static Style GetStyle(string key)
        {
            return Application.Current.Resources[key] as Style;
        }
    }
}
