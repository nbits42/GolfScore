using System;
using System.Linq;
using GolfScore.Contracts;
using GolfScore.iOS.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AppStarter))]
namespace GolfScore.iOS.Services
{
    public class AppStarter : IAppStarter
    {
        public bool CanStart(IExtraInfo info)
        {
            switch (info.Type)
            {
                case ExtraInfoType.Website:
                    return false;
                case ExtraInfoType.Facebook:
                case ExtraInfoType.Instagram:
                case ExtraInfoType.Youtube:
                    return true;

                default:
                    return false;
            }
        }

        public void StartApp(IExtraInfo info)
        {
            try
            {
                var infoUri = new Uri(info.Url);
                var url = info.Url;
                var user = GetUserFromUri(infoUri);
                switch (info.Type)
                {
                    case ExtraInfoType.Facebook:
                        url = $"https://www.facebook.com/n/?{user}";
                        break;
                    case ExtraInfoType.Instagram:
                        url = $"http://instagram.com/_u/{user}";
                        break;
                    case ExtraInfoType.Youtube:
                        url = $"http://vnd.youtube://user/{user}";
                        break;
                }
                Device.OpenUri(new Uri(url));
            }
            catch (Exception)
            {
                Device.OpenUri(new Uri(info.Url));
            }
        }



        private string GetUserFromUri(Uri uri)
        {
            return uri.Segments.Last().Replace("/", "");
        }
    }
}