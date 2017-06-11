using Core;
using SQLite;
using System.IO;
using Windows.Storage;

namespace SampleApplication.Windows
{
    public class WindowsDatabaseConnectionFactory : LogicCommand<object, DatabaseConnectioFactoryResult>, IDatabaseConnectionFactory
    {
        #region implemented abstract members of LogicCommand

        public override DatabaseConnectioFactoryResult Execute(object request)
        {
            DatabaseConnectioFactoryResult result = new DatabaseConnectioFactoryResult();

            const string dbFileName = "SampleDatabse.db3";
            string documentsPath = ApplicationData.Current.LocalFolder.Path;
            string path = Path.Combine(documentsPath, dbFileName);

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

        #endregion implemented abstract members of LogicCommand
    }
}