using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder.Services
{
    public interface IFileDialogService
    {
        public string? OpenFileDialog(string title, string? defaultExt, string? defaultDirectory, string? filter);
        public string? OpenFolderDialog(string title, string? defaultDirectory);
    }

    public class FileDialogService : IFileDialogService
    {
        public string? OpenFileDialog(string title, string? defaultExt, string? defaultDirectory, string? filter)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = title;
            if (defaultExt != null) dialog.DefaultExt = defaultExt;
            if (defaultDirectory != null && defaultDirectory != string.Empty) dialog.DefaultDirectory = defaultDirectory;
            if (filter != null) dialog.Filter = filter;

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }

        public string? OpenFolderDialog(string title, string? defaultDirectory)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Title = title;
            if (defaultDirectory != null && defaultDirectory != string.Empty) dialog.DefaultDirectory = defaultDirectory;
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
