using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.SQLite.Helpers
{
    public class ConnectionFactory
    {

        public static SQLiteConnection CreateDatabase(string filePath)
        {
            return new SQLiteConnection(filePath);
        }

    }
}
