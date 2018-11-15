using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GolfScore.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        /* ------------------- SetupCompleted  -------------------- */

        private const string SetupCompletedKey = "SetupCompleted_key";
        private static readonly bool SetupCompletedDefault = false;

        public static bool SetupCompleted
        {
            get => AppSettings.GetValueOrDefault(SetupCompletedKey, SetupCompletedDefault);
            set => AppSettings.AddOrUpdateValue(SetupCompletedKey, value);
        }

        /* ------------------- MyPlayerId  -------------------- */

        private const string MyPlayerIdKey = "MyPlayerId_key";
        private static readonly string MyPlayerIdDefault = null;

        public static string MyPlayerId
        {
            get => AppSettings.GetValueOrDefault(MyPlayerIdKey, MyPlayerIdDefault);
            set => AppSettings.AddOrUpdateValue(MyPlayerIdKey, value);
        }
        /* ------------------- CurrentGameId  -------------------- */

        private const string CurrentGameIdKey = "CurrentGameId_key";
        private static readonly string CurrentGameIdDefault = null;

        public static string CurrentGameId
        {
            get => AppSettings.GetValueOrDefault(CurrentGameIdKey, CurrentGameIdDefault);
            set => AppSettings.AddOrUpdateValue(CurrentGameIdKey, value);
        }

    }
}
