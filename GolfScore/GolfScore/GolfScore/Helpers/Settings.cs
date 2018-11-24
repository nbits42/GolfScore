using GlobalContracts.Enumerations;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TeeScore.Helpers
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

        /* ------------------- LastGameType  -------------------- */

        private const string LastGameTypeKey = "LastGameType_key";
        private static readonly int LastGameTypeDefault = (int)GameType.Golf;

        public static GameType LastGameType
        {
            get => (GameType)AppSettings.GetValueOrDefault(LastGameTypeKey, LastGameTypeDefault);
            set => AppSettings.AddOrUpdateValue(LastGameTypeKey, (int)value);
        }

        /* ------------------- LastTeeCount  -------------------- */

        private const string LastTeeCountKey = "LastTeeCount_key";
        private static readonly int LastTeeCountDefault = 1;

        public static int LastTeeCount
        {
            get => AppSettings.GetValueOrDefault(LastTeeCountKey, LastTeeCountDefault);
            set => AppSettings.AddOrUpdateValue(LastTeeCountKey, value);
        }

        /* ------------------- LastPlayersCount  -------------------- */

        private const string LastPlayersCountKey = "LastPlayersCount_key";
        private static readonly int LastPlayersCountDefault = 1;

        public static int LastPlayersCount
        {
            get => AppSettings.GetValueOrDefault(LastPlayersCountKey, LastPlayersCountDefault);
            set => AppSettings.AddOrUpdateValue(LastPlayersCountKey, value);
        }
    }
}
