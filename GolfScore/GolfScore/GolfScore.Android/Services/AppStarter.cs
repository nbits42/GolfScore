using System;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using GolfScore.Contracts;
using GolfScore.Droid.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AppStarter))]
namespace GolfScore.Droid.Services
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
                Intent intent = null;
                switch (info.Type)
                {
                    case ExtraInfoType.Facebook:
                        url = $"https://www.facebook.com/n/?{user}";
                        intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                        intent.SetPackage("com.facebook.katana");
                        break;
                    case ExtraInfoType.Instagram:
                        url = $"http://instagram.com/_u/{user}";
                        intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                        intent.SetPackage("com.instagram.android");
                        break;
                }

                if (intent != null && IsIntentAvaiable(MainActivity.Instance, intent))
                {
                    StartActivity(intent);
                }
                else
                {
                    StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse(info.Url)));
                }
            }
            catch (Exception)
            {
                Device.OpenUri(new Uri(info.Url));
            }
        }

        private void StartActivity(Intent intent)
        {
            MainActivity.Instance.StartActivity(intent);
        }

        private bool IsIntentAvaiable(Context context, Intent intent)
        {
            var packageManager = context.PackageManager;
            var list = packageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return list.Any();
        }

        private string GetUserFromUri(Uri uri)
        {
            return uri.Segments.Last().Replace("/","");
        }
    }
}