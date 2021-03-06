﻿using Foundation;
using GolfScore.iOS.Services;
using TeeScore.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(VersionInfoService))]
namespace GolfScore.iOS.Services
{
    public class VersionInfoService : IVersionInfo
    {
        public string GetVersion()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString")?.ToString()??string.Empty;
        }

        public string GetBuild()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion")?.ToString()??string.Empty;
        }
    }
}