using Microsoft.Win32;
using SQLite.Common.Contracts;
using System;

namespace Database.WPF.Infrastructure
{
    public class FileDialogService : IFileDialogService
    {
        public DialogResult Show(DialogConfiguration dialogConfiguration)
        {
            var dialog = Map(dialogConfiguration);
            var re = dialog.ShowDialog();
            return new(re, dialog.FileName);
        }

        static FileDialog Map(DialogConfiguration dialogConfiguration)
        {
            return dialogConfiguration.DialogType switch
            {
                DialogType.Open => new OpenFileDialog
                {
                    Filter = dialogConfiguration.Filter
                },
                DialogType.Save => new SaveFileDialog
                {
                    Filter = dialogConfiguration.Filter
                },
                _ => throw new Exception("333iiii"),
            };
        }
    }
}
