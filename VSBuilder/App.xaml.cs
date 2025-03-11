using System.Configuration;
using System.Data;
using System.Windows;

namespace VSBuilder
{
    /// <summary>
    /// アプリケーションクラス
    /// </summary>
    public partial class App : Application
    {
        protected SolutionSettingWindow? SolutionSettingWindow = null;

        public void OpenSolutionSettingWindow(SolutionSetting? setting = null)
        {
            if (SolutionSettingWindow == null)
            {
                SolutionSettingWindow = new SolutionSettingWindow();
                if (setting != null)
                {
                    SolutionSettingWindowViewModel.Current?.SetSolutionData(setting);
                }
                SolutionSettingWindow.ShowDialog();
            }
        }

        public void CloseSolutionSettingWindow()
        {
            if (SolutionSettingWindow != null )
            {
                SolutionSettingWindow.Close();
                SolutionSettingWindow = null;
            }
        }
    }
}
