using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System.Collections.Generic;

namespace SQLiteKei.ViewModels.QueryEditorWindow
{
    public class QueryEditorViewModel : NotifyingModel
    {

        private string sqlStatement;
        private string statusInfo;

        public QueryEditorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            sqlStatement = string.Empty;
        }

        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public List<DatabaseSelectItem> Databases { get; set; }


    }
}
