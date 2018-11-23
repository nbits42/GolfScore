using System;
using Android;
using Android.Content.PM;
using Android.Support.V4.Content;
using TeeScore.Contracts;
using TeeScore.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(MapService))]

namespace TeeScore.Droid.Services
{
    public class MapService : IMapService
    {
        public bool IsLocationPermissionGranted
        {
            get
            {
                try
                {
                    var granted = (ContextCompat.CheckSelfPermission(MainActivity.Instance,
                                       Manifest.Permission.AccessCoarseLocation) ==
                                   Permission.Granted) ||
                                  (ContextCompat.CheckSelfPermission(MainActivity.Instance,
                                       Manifest.Permission.AccessFineLocation) ==
                                   Permission.Granted);
                    return granted;

                }
                catch (Exception )
                {
                    //Crashes.TrackError(e);
                    return false;
                }
            }
        }
    }
}