using System;
using System.IO;
using GolfScore.Contracts;
using GolfScore.iOS.Services;

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