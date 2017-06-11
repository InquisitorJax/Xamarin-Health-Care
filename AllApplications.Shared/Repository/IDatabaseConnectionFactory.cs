using SQLite;

namespace Core
{
    public interface IDatabaseConnectionFactory : ILogicCommand<object, DatabaseConnectioFactoryResult>
    {
        //https://developer.xamarin.com/guides/xamarin-forms/working-with/databases/
    }

    public class DatabaseConnectioFactoryResult : CommandResult
    {
        public SQLiteAsyncConnection Connection { get; set; }
    }
}