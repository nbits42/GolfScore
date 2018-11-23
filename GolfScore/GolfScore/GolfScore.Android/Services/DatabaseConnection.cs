using Android.Graphics;
using TeeScore.Contracts;
using TeeScore.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]

namespace TeeScore.Droid.Services
{
    public class DatabaseConnection: IDatabaseConnection
    {
        public string DbConnection(string dbVersion)
        {
            var path= System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return $@"{path}\teescore_v{dbVersion}.sqlite3";
        }
    }
}