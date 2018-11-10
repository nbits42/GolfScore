using System.IO;
using GolfScore.Contracts;
using GolfScore.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]

namespace GolfScore.Droid.Services
{
    public class DatabaseConnection: IDatabaseConnection
    {
        public string DbConnection(string dbVersion)
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}