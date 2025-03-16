using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder.ViewModels
{
    public class ViewModelLocator
    {
        #region Member

        protected ViewModel _vm = new ViewModel();

        protected SolutionSettingWindowViewModel __solutionSettingVM = new SolutionSettingWindowViewModel();

        protected CopyFileSettingWindowViewModel __copyFileSettingVM = new CopyFileSettingWindowViewModel();

        protected HistoryWindowViewModel __historyVM = new HistoryWindowViewModel();

        #endregion

        #region Property

        public ViewModel VM { get => _vm; }

        public SolutionSettingWindowViewModel SolutionSettingVM { get => __solutionSettingVM; }

        public CopyFileSettingWindowViewModel CopyFileSettingVM { get => __copyFileSettingVM; }

        public HistoryWindowViewModel HistoryVM { get => __historyVM; }

        #endregion
    }
}
