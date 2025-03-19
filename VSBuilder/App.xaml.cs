using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using VSBuilder.Models;
using VSBuilder.ViewModels;
using VSBuilder.Views;

namespace VSBuilder
{
    /// <summary>
    /// アプリケーションクラス
    /// </summary>
    public partial class App : Application
    {
        public ViewModelLocator ViewModelLocator { get; set; } = new ViewModelLocator();

        protected SolutionSettingWindow? SolutionSettingWindow = null;
        protected CopyFileSettingWindow? CopyFileSettingWindow = null;
        protected HistoryWindow? HistoryWindow = null;

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            ViewModelLocator.VM.SaveSettings();
        }

        public void OpenSolutionSettingWindow(SolutionSetting? setting = null)
        {
            if (SolutionSettingWindow == null)
            {
                SolutionSettingWindow = new SolutionSettingWindow();
                SolutionSettingWindow.Closed += (s, e) => SolutionSettingWindow = null;

                if (setting != null)
                {
                    ViewModelLocator.SolutionSettingVM.SetSolutionData(setting, true);
                }
                else
                {
                    ViewModelLocator.SolutionSettingVM.SetSolutionData(new SolutionSetting());
                }
                SolutionSettingWindow.ShowDialog();
            }
        }

        public void CloseSolutionSettingWindow()
        {
            if (SolutionSettingWindow != null)
            {
                SolutionSettingWindow.Close();
                SolutionSettingWindow = null;
            }
        }

        public void OpenCopyFileSettingWindow(CopyFileSetting? setting = null)
        {
            if (CopyFileSettingWindow == null)
            {
                CopyFileSettingWindow = new CopyFileSettingWindow();
                CopyFileSettingWindow.Closed += (s, e) => CopyFileSettingWindow = null;

                if (setting != null)
                {
                    ViewModelLocator.CopyFileSettingVM.SetCopyFileData(setting, true);
                }
                else
                {
                    ViewModelLocator.CopyFileSettingVM.SetCopyFileData(new CopyFileSetting());
                }
                CopyFileSettingWindow.ShowDialog();
            }
        }

        public void CloseCopyFileSettingWindow()
        {
            if (CopyFileSettingWindow != null)
            {
                CopyFileSettingWindow.Close();
                CopyFileSettingWindow = null;
            }
        }

        public void OpenHistoryWindow()
        {
            if (HistoryWindow == null)
            {
                HistoryWindow = new HistoryWindow();
                HistoryWindow.Closed += (s, e) => HistoryWindow = null;

                HistoryWindow.Left = MainWindow.Left + MainWindow.Width;
                HistoryWindow.Top = MainWindow.Top;
                HistoryWindow.Show();
            }
        }

        public void CloseHistoryWindow()
        {
            if (HistoryWindow != null)
            {
                HistoryWindow.Close();
                HistoryWindow = null;
            }
        }
    }
}
