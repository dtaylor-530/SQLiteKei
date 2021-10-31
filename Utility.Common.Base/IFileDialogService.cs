using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Common.Contracts
{

    public enum DialogType
    {
        Open, Save
    }

    public record DialogConfiguration(string Filter, DialogType DialogType);
    public record DialogResult(bool? Success, string FilePath);

    public interface IFileDialogService
    {
        DialogResult Show(DialogConfiguration dialogConfiguration);
    }
}
