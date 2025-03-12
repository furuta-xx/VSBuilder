using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VSBuilder.Models;
using VSBuilder.Helpers;
using VSBuilder.Services;

namespace VSBuilder
{
    public class CopyFileSettingWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanges

        /// <summary>
        /// 通知イベント
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// プロパティの変更通知を起動する
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Member

        protected IFileDialogService __fileDialogService = new FileDialogService();

        protected bool __isEditMode = false;

        protected CopyFileSetting __copyFileSetting = new CopyFileSetting();

        #endregion

        #region Property

        public bool IsEditMode { get => __isEditMode; set { __isEditMode = value; NotifyPropertyChanged(nameof(Title)); NotifyPropertyChanged(nameof(ExecButtonText)); } }

        public string Title { get => IsEditMode ? "コピーファイル設定の編集" : "コピーファイル設定の追加"; }

        public string CopyFileName { get => __copyFileSetting.Name; set { __copyFileSetting.Name = value; NotifyPropertyChanged(); } }

        public string SourcePath { get => __copyFileSetting.SourcePath; set { __copyFileSetting.SourcePath = value; NotifyPropertyChanged(); } }

        public string DestinationPath { get => __copyFileSetting.DestinationPath; set { __copyFileSetting.DestinationPath = value; NotifyPropertyChanged(); } }

        public string ExecButtonText { get => IsEditMode ? "編集" : "追加"; }

        #endregion

        #region Command

        public ICommand CommandAddCopyFile { get; set; }

        public ICommand CommandCancel { get; set; }

        public ICommand CommandRefSourcePath { get; set; }

        public ICommand CommandRefDestinationPath { get; set; }

        #endregion

        public CopyFileSettingWindowViewModel()
        {
            CommandAddCopyFile = new RelayCommand(ExecuteAddCopyFile);
            CommandCancel = new RelayCommand(ExecuteCancel);
            CommandRefSourcePath = new RelayCommand(ExecuteRefSourcePath);
            CommandRefDestinationPath = new RelayCommand(ExecuteRefDestinationPath);
        }

        public void SetCopyFileData(CopyFileSetting setting)
        {
            CopyFileName = setting.Name;
            SourcePath = setting.SourcePath;
            DestinationPath = setting.DestinationPath;
            IsEditMode = true;
        }

        public void ExecuteAddCopyFile()
        {
            (App.Current as App)?.ViewModelLocator.VM.AddCopyFile(__copyFileSetting, IsEditMode);
            (App.Current as App)?.CloseCopyFileSettingWindow();
        }

        public void ExecuteCancel()
        {
            (App.Current as App)?.CloseCopyFileSettingWindow();
        }

        public void ExecuteRefSourcePath()
        {
            string title = "コピー元フォルダを選択してください。";
            string? defaultDirectory = SourcePath != string.Empty ? System.IO.Path.GetDirectoryName(SourcePath) : string.Empty;
            string? path = __fileDialogService.OpenFolderDialog(title, defaultDirectory);
            if (path != null) SourcePath = path;
        }

        public void ExecuteRefDestinationPath()
        {
            string title = "コピー先フォルダを選択してください。";
            string? defaultDirectory = DestinationPath != string.Empty ? System.IO.Path.GetDirectoryName(DestinationPath) : string.Empty;
            string? path = __fileDialogService.OpenFolderDialog(title, defaultDirectory);
            if (path != null) DestinationPath = path;
        }
    }
}
