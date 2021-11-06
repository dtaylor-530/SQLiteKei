using System.Data.SQLite;
using Utility.Database;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// The base class for DbHandlers which implements IDisposable and provides a DbConnection for the specified sqlite database.
    /// </summary>
    public abstract class DisposableDbHandler : IDisposable
    {
        protected DisposableDbHandler(DatabasePath connectionPath)
        {
            Connection = InitializeConnection(connectionPath);
            ConnectionPath = connectionPath;
        }

        protected SQLiteConnection Connection { get; }
        protected DatabasePath ConnectionPath { get; }

        private SQLiteConnection InitializeConnection(DatabasePath connectionPath)
        {
            var connection = new SQLiteConnection
            {
                ConnectionString = string.Format("Data Source={0}", connectionPath.Path)
            };

            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
