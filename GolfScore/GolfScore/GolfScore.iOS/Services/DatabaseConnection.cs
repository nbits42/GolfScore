using System;
using System.IO;
using GolfScore.iOS.Services;
using TeeScore.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace GolfScore.iOS.Services
{
    public class DatabaseConnection: IDatabaseConnection
    {
        public string DbConnection(string dbVersion)
        {
            var personalFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(personalFolder, "..", "Library");
        }
    }
}