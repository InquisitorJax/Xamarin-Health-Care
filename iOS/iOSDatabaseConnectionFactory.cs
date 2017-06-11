using Core;
using SQLite;
using System;
using System.IO;

namespace SampleApplication.iOS
{
    public class iOSDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        //https://developer.xamarin.com/guides/xamarin-forms/working-with/databases/
        public DatabaseConnectioFactoryResult Execute(object request)
        {
            DatabaseConnectioFactoryResult result = new DatabaseConnectioFactoryResult();

            const string dbFileName = "SampleDatabse.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, dbFileName);

            try
            {
                result.Connection = new SQLiteAsyncConnection(path);
            }
            catch (SQLiteException)
            {
                //TODO: Log
                result.Notification.Add("Error Creating Database Connection");
            }

            return result;
        }
    }
}