using Core;
using SQLite;
using System.IO;

namespace SampleApplication.Droid
{
    public class AndroidDatabaseConnectionFactory : LogicCommand<object, DatabaseConnectioFactoryResult>, IDatabaseConnectionFactory
    {
        #region implemented abstract members of LogicCommand

        public override DatabaseConnectioFactoryResult Execute(object request)
        {
            DatabaseConnectioFactoryResult result = new DatabaseConnectioFactoryResult();

            const string dbFileName = "SampleDatabse.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
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