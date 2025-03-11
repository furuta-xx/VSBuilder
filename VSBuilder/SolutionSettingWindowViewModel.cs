using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VSBuilder
{
    public class SolutionSettingWindowViewModel : INotifyPropertyChanged
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
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Member

        protected bool __isEditMode = false;

        protected SolutionSetting __solutionSetting = new SolutionSetting();

        #endregion

        #region Property

        public static SolutionSettingWindowViewModel? Current { get; set; } = null;

        public bool IsEditMode { get => __isEditMode; set { __isEditMode = value; NotifyPropertyChanged(nameof(Title)); NotifyPropertyChanged(nameof(ExecButtonText)); } }

        public string Title { get => IsEditMode ? "ソリューションの編集" : "ソリューションの追加"; }

        public string SolutionName { get => __solutionSetting.Name; set { __solutionSetting.Name = value; NotifyPropertyChanged(); } }

        public string SolutionFilePath { get => __solutionSetting.SolutionFilePath; set { __solutionSetting.SolutionFilePath = value; NotifyPropertyChanged(); } }

        public string BuildConfig { get => __solutionSetting.BuildConfig; set { __solutionSetting.BuildConfig = value; NotifyPropertyChanged(); } }

        public string Platform { get => __solutionSetting.Platform; set { __solutionSetting.Platform = value; NotifyPropertyChanged(); } }

        public string ModulePath { get => __solutionSetting.ModulePath; set { __solutionSetting.ModulePath = value; NotifyPropertyChanged(); } }

        public string OutputPath {  get => __solutionSetting.OutputPath; set { __solutionSetting.OutputPath = value; NotifyPropertyChanged(); } }

        public string ExecButtonText { get => IsEditMode ? "編集" : "追加"; }

        #endregion

        #region Command

        public ICommand CommandAddSolution { get; set; }
        
        public ICommand CommandCancel { get; set; }

        public ICommand CommandRefSolutionFile { get; set; }

        public ICommand CommandRefModulePath { get; set; }

        public ICommand CommandRefOutputPath { get; set; }

        #endregion

        public SolutionSettingWindowViewModel()
        {
            Current = this;

            CommandAddSolution = new RelayCommand(ExecuteAddSolution);
            CommandCancel = new RelayCommand(ExecuteCancel);
            CommandRefSolutionFile = new RelayCommand(ExecuteRefSolutionFile);
            CommandRefModulePath = new RelayCommand(ExecuteRefModulePath);
            CommandRefOutputPath = new RelayCommand(ExecuteRefOutputPath);
        }

        public void SetSolutionData(SolutionSetting setting)
        {
            SolutionName = setting.Name;
            SolutionFilePath = setting.SolutionFilePath;
            BuildConfig = setting.BuildConfig;
            Platform = setting.Platform;
            ModulePath = setting.ModulePath;
            OutputPath = setting.OutputPath;
            IsEditMode = true;
        }

        protected void ExecuteAddSolution()
        {
            ViewModel.Current?.AddSolution(__solutionSetting, IsEditMode);
            (App.Current as App)?.CloseSolutionSettingWindow();
        }

        protected void ExecuteCancel()
        {
            (App.Current as App)?.CloseSolutionSettingWindow();
        }

        protected void ExecuteRefSolutionFile()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".sln";
            if (SolutionFilePath != string.Empty) dialog.DefaultDirectory = System.IO.Path.GetDirectoryName(SolutionFilePath);
            if (__solutionSetting.SolutionFilePath != string.Empty)
            {
                dialog.DefaultDirectory = System.IO.Path.GetDirectoryName(__solutionSetting.SolutionFilePath);
            }
            dialog.Filter = "Solution File (.sln)|*.sln";

            if (dialog.ShowDialog() == true)
            {
                SolutionFilePath = dialog.FileName;
            }
        }

        protected void ExecuteRefModulePath()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Title = "モジュールフォルダを選択してください。";
            if (ModulePath != string.Empty) dialog.DefaultDirectory = System.IO.Path.GetDirectoryName(ModulePath);
            if (__solutionSetting.SolutionFilePath != string.Empty)
            {
                dialog.DefaultDirectory = System.IO.Path.GetDirectoryName(ModulePath);
            }
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ModulePath = dialog.FileName;
            }
        }

        protected void ExecuteRefOutputPath()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Title = "出力フォルダを選択してください。";
            if (OutputPath != string.Empty) dialog.DefaultDirectory = System.IO.Path.GetDirectoryName(OutputPath);
            if (__solutionSetting.SolutionFilePath != string.Empty)
            {
                dialog.DefaultDirectory = System.IO.Path.GetDirectoryName(__solutionSetting.OutputPath);
            }
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OutputPath = dialog.FileName;
            }
        }
    }
}
