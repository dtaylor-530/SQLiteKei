using System.Data.SQLite;
using Utility.Entity;

namespace Utility.SQLite.Helpers;

public class ConnectionFactory
{

    public static ConnectionResult CreateDatabase(string filePath)
    {
        var connection = new SQLiteConnection(filePath);
        return new ConnectionResult();
    }

}
