using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Common.Contracts
{
    public interface IThemeService
    {
        void ApplyCurrentUserTheme();

        void ApplyTheme(string themeName);

        string Theme { get; }

        string[] Themes { get; }
    }
}
