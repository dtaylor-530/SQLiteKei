using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;

namespace Database.Service.Model
{
    public interface ITableGeneralModel
    {
        TableGeneralInformation Get(ITableKey tableKey);
    }
}