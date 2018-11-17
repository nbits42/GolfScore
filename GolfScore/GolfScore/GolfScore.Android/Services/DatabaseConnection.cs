using TeeScore.Contracts;
using TeeScore.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]

namespace TeeScore.Droid.Services
{
    public class DatabaseConnection: IDatabaseConnection
    {
        public string DbConnection(string dbVersion)
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}