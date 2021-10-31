using Utility;
using Utility.Database;

namespace SQLite.Service.Service;

public class TableNameService
{
    public TableName Map(Key key)
    {
        switch (key)
        {
            case TableKey { TableName: { } name }:
                return new TableName(name);
            default:
                throw new Exception("Key types dont match");
        }
    }

}
