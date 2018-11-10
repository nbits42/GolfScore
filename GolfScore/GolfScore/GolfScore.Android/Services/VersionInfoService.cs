using Android.Content.PM;
using GolfScore.Contracts;
using GolfScore.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionInfoService))]

namespace GolfScore.Droid.Services
{
    public class VersionInfoService : IVersionInfo
    {
        public VersionInfoService()
        {
            var context = global::Android.App.Application.Context;

            var manager = context.PackageManager;
            Info = manager.GetPackageInfo(context.PackageName, 0);

        }

        public PackageInfo Info { get; set; }

        public string GetVersion()
        {

            return Info?.VersionName??string.Empty;
        }

        public string GetBuild()
        {
            return Info?.VersionCode.ToString()??string.Empty;
        }
    }
}