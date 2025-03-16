using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VSBuilder.Models;
using VSBuilder.Helpers;

namespace VSBuilder
{
    public class ViewModel : INotifyPropertyChanged
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

        #region Class

        protected class SettingJson
        {
            public string MsBuildPath { get; set; } = string.Empty;

            public List<SolutionSetting>? SolutionSettings { get; set; } = null;

            public List<CopyFileSetting>? CopyFileSettings { get; set; } = null;
        }

        #endregion

        #region Member

        protected ObservableCollection<SolutionSetting> __solutionSettings = new ObservableCollection<SolutionSetting>();

        protected ObservableCollection<CopyFileSetting> __copyFileSettings = new ObservableCollection<CopyFileSetting>();

        protected ObservableCollection<ExecuteHistory> __executeHistory = new ObservableCollection<ExecuteHistory>();

        protected string __messageText = string.Empty;

        protected bool __isWaitBuilding = true;

        #endregion

        #region Property

        public string MsBuildPath { get; set; } = string.Empty;

        public ObservableCollection<SolutionSetting> SolutionSettings
        {
            get => __solutionSettings;
            set
            {
                __solutionSettings = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(SolutionSettingsVisibility));
                NotifyPropertyChanged(nameof(OutputVisibility));
                NotifyPropertyChanged(nameof(IsMenuEnabledSolution));
            }
        }

        public ObservableCollection<CopyFileSetting> CopyFileSettings
        {
            get => __copyFileSettings;
            set
            {
                __copyFileSettings = value;
                NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CopyFileSettingsVisibility));
                NotifyPropertyChanged(nameof(OutputVisibility));
                NotifyPropertyChanged(nameof(IsMenuEnabledCopyFile));
            }
        }

        public ObservableCollection<ExecuteHistory> ExecuteHistory { get => __executeHistory; set { __executeHistory = value; NotifyPropertyChanged(); } }

        public string MessageText { get => __messageText; set { __messageText = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MessageTextVisibility)); } }

        public Visibility SolutionSettingsVisibility { get => SolutionSettings.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }

        public Visibility CopyFileSettingsVisibility { get => CopyFileSettings.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }

        public Visibility MessageTextVisibility { get => MessageText != string.Empty ? Visibility.Visible : Visibility.Collapsed; }

        public Visibility OutputVisibility { get => SolutionSettings.Count == 0 && CopyFileSettings.Count == 0 ? Visibility.Collapsed : Visibility.Visible; }

        public bool IsWaitBuilding { get => __isWaitBuilding; set { __isWaitBuilding = value; NotifyPropertyChanged(); } }

        public bool IsMenuEnabledSolution { get => SolutionSettings.Count > 0 && IsWaitBuilding; }

        public bool IsMenuEnabledCopyFile { get => CopyFileSettings.Count > 0 && IsWaitBuilding; }

        public bool IsMenuEnabledOutput { get => (IsMenuEnabledSolution || IsMenuEnabledCopyFile) && IsWaitBuilding; }

        public int SolutionNextID { get; set; } = 0;

        public int CopyFileNextID { get; set; } = 0;

        #endregion

        #region Command

        public ICommand CommandAddSolution { get; set; }

        public ICommand CommandEditSolution { get; set; }

        public ICommand CommandDeleteSolution { get; set; }

        public ICommand CommandAddCopyFile { get; set; }

        public ICommand CommandEditCopyFile { get; set; }

        public ICommand CommandDeleteCopyFile { get; set; }

        public ICommand CommandBulkBuild { get; set; }

        public ICommand CommandShowHistory { get; set; }

        public ICommand CommandExit { get; set; }

        #endregion

        public ViewModel()
        {
            CommandAddSolution = new RelayCommand(ExecuteAddSolution);
            CommandEditSolution = new RelayCommand(ExecuteEditSolution);
            CommandDeleteSolution = new RelayCommand(ExecuteDeleteSolution);
            CommandAddCopyFile = new RelayCommand(ExecuteAddCopyFile);
            CommandEditCopyFile = new RelayCommand(ExecuteEditCopyFile);
            CommandDeleteCopyFile = new RelayCommand(ExecuteDeleteCopyFile);
            CommandBulkBuild = new RelayCommand(ExecuteBulkBuild);
            CommandShowHistory = new RelayCommand(ExecuteShowHistory);
            CommandExit = new RelayCommand(ExecuteExit);

            LoadSettings();
            CheckAndSetMsBuildPath();
        }

        protected void ExecuteAddSolution()
        {
            (App.Current as App)?.OpenSolutionSettingWindow();
        }

        protected void ExecuteEditSolution()
        {
            List<SolutionSetting> solutions = SolutionSettings.ToList().FindAll(s => s.IsOutput);
            if (solutions.Count == 0)
            {
                MessageBox.Show("ソリューションを1つ選択してください。");
            }
            else if (solutions.Count > 1)
            {
                MessageBox.Show("ソリューションを1つだけ選択してください。");
            }
            else
            {
                (App.Current as App)?.OpenSolutionSettingWindow(solutions.FirstOrDefault());
            }
        }

        protected void ExecuteDeleteSolution()
        {
            SolutionSettings = new ObservableCollection<SolutionSetting>(SolutionSettings.Where(s => !s.IsOutput));
            SaveSettings();

            NotifyPropertyChanged(nameof(SolutionSettingsVisibility));
            NotifyPropertyChanged(nameof(OutputVisibility));
        }

        protected void ExecuteAddCopyFile()
        {
            (App.Current as App)?.OpenCopyFileSettingWindow();
        }

        protected void ExecuteEditCopyFile()
        {
            List<CopyFileSetting> copyFileSettings = CopyFileSettings.ToList().FindAll(s => s.IsOutput);
            if (copyFileSettings.Count == 0)
            {
                MessageBox.Show("コピーファイル設定を1つ選択してください。");
            }
            else if (copyFileSettings.Count > 1)
            {
                MessageBox.Show("コピーファイル設定を1つだけ選択してください。");
            }
            else
            {
                (App.Current as App)?.OpenCopyFileSettingWindow(copyFileSettings.FirstOrDefault());
            }
        }

        protected void ExecuteDeleteCopyFile()
        {
            CopyFileSettings = new ObservableCollection<CopyFileSetting>(CopyFileSettings.Where(s => !s.IsOutput));
            SaveSettings();

            NotifyPropertyChanged(nameof(CopyFileSettingsVisibility));
            NotifyPropertyChanged(nameof(OutputVisibility));
        }

        protected void ExecuteBulkBuild()
        {
            IsWaitBuilding = false;
            int buildSuccess = 0;
            int buildFailed = 0;
            int copySuccess = 0;
            int copyFailed = 0;
            foreach (SolutionSetting setting in SolutionSettings)
            {
                DispatcherFrame frame = new DispatcherFrame();
                Task.Run(() =>
                {
                    if (setting.IsOutput)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageText = $"{setting.Name} のビルド中です。";
                        });

                        ProcessStartInfo psi = new ProcessStartInfo()
                        {
                           FileName = $"{MsBuildPath}",
                           Arguments = $"\"{setting.SolutionFilePath}\" /t:clean;rebuild /p:Configuration=\"{setting.BuildConfig}\";Platform=\"{setting.Platform}\"",
                           CreateNoWindow = true,
                           UseShellExecute = false,
                           WorkingDirectory = System.IO.Path.GetDirectoryName(MsBuildPath)
                        };
                        System.Diagnostics.Debug.WriteLine(psi.Arguments);
                        using (Process? p = Process.Start(psi))
                        {
                            p?.WaitForExit();
                            if (p?.ExitCode == 0)
                            {
                                if (!System.IO.Directory.Exists(setting.OutputPath))
                                {
                                    System.IO.Directory.CreateDirectory(setting.OutputPath);
                                }
                                CopyAll(new DirectoryInfo(setting.ModulePath), new DirectoryInfo(setting.OutputPath));
                            }
                            buildSuccess += p?.ExitCode == 0 ? 1 : 0;
                            buildFailed += p?.ExitCode == 0 ? 0 : 1;
                            AddHistory(Models.ExecuteHistory.ExecuteType.Build, setting.Name, p?.ExitCode == 0);
                        }
                    }
                    frame.Continue = false;
                });
                Dispatcher.PushFrame(frame);
            }
            foreach (CopyFileSetting setting in CopyFileSettings)
            {
                DispatcherFrame frame = new DispatcherFrame();
                Task.Run(() =>
                {
                    if (setting.IsOutput)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageText = $"{setting.Name} のコピー中です。";
                        });
                        try
                        {
                            CopyAll(new DirectoryInfo(setting.SourcePath), new DirectoryInfo(setting.DestinationPath));
                            copySuccess++;
                            AddHistory(Models.ExecuteHistory.ExecuteType.CopyFile, setting.Name, true);
                        }
                        catch
                        {
                            copyFailed++;
                            AddHistory(Models.ExecuteHistory.ExecuteType.CopyFile, setting.Name, false);
                        }
                    }
                    frame.Continue = false;
                });
                Dispatcher.PushFrame(frame);
            }
            MessageText = "全てのビルドが完了しました。" +
                (SolutionSettings.Count > 0 ? $"  ビルド成功: {buildSuccess}、ビルド失敗: {buildFailed}" : string.Empty) +
                (CopyFileSettings.Count > 0 ? $"  コピー成功: {copySuccess}、コピー失敗: {copyFailed}" : string.Empty);
            IsWaitBuilding = true;
        }

        public void ExecuteShowHistory()
        {
            (App.Current as App)?.OpenHistoryWindow();
        }

        public void ExecuteExit()
        {
            SaveSettings();
            Environment.Exit(0);
        }

        public void AddSolution(SolutionSetting ss, bool isEdit = false)
        {
            if (isEdit)
            {
                foreach (SolutionSetting s in SolutionSettings)
                {
                    if (s.ID == ss.ID)
                    {
                        s.CopyFrom(ss);
                        s.IsOutput = true;
                    }
                }
            }
            else
            {
                ss.ID = SolutionNextID;
                SolutionNextID++;
                SolutionSettings.Add(new SolutionSetting(ss));
            }

            SaveSettings();

            NotifyPropertyChanged(nameof(SolutionSettings));
            NotifyPropertyChanged(nameof(SolutionSettingsVisibility));
            NotifyPropertyChanged(nameof(OutputVisibility));
        }

        public void AddCopyFile(CopyFileSetting cfs, bool isEdit = false)
        {
            if (isEdit)
            {
                foreach (CopyFileSetting s in CopyFileSettings)
                {
                    if (s.ID == cfs.ID)
                    {
                        s.CopyFrom(cfs);
                        s.IsOutput = true;
                    }
                }
            }
            else
            {
                cfs.ID = CopyFileNextID;
                CopyFileNextID++;
                CopyFileSettings.Add(new CopyFileSetting(cfs));
            }

            SaveSettings();

            NotifyPropertyChanged(nameof(CopyFileSettings));
            NotifyPropertyChanged(nameof(CopyFileSettingsVisibility));
            NotifyPropertyChanged(nameof(OutputVisibility));
        }

        public void SaveSettings()
        {
            SettingJson json = new SettingJson() { MsBuildPath = MsBuildPath, SolutionSettings = SolutionSettings.ToList(), CopyFileSettings = CopyFileSettings.ToList() };
            using (StreamWriter sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}\\setting.json", false))
            {
                sw.Write(JsonSerializer.Serialize(json));
            }
        }

        public void LoadSettings()
        {
            SettingJson? json = new SettingJson();
            try
            {
                using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\setting.json"))
                {
                    string text = sr.ReadToEnd();
                    json = JsonSerializer.Deserialize<SettingJson>(text);
                }
            }
            catch { }
            if (json != null)
            {
                MsBuildPath = json.MsBuildPath;
                json.SolutionSettings?.ForEach(s => SolutionSettings.Add(s));
                json.CopyFileSettings?.ForEach(s => CopyFileSettings.Add(s));
            }

            if (SolutionSettings.Count > 0)
            {
                SolutionNextID = SolutionSettings.ToList().ConvertAll(s => s.ID).Max() + 1;
            }
            if (CopyFileSettings.Count  > 0)
            {
                CopyFileNextID = CopyFileSettings.ToList().ConvertAll(s => s.ID).Max() + 1;
            }
        }

        public void CheckAndSetMsBuildPath()
        {
            if (MsBuildPath != string.Empty) return;

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".exe";
            dialog.Filter = "Exe File (.exe)|*.exe";
            dialog.FileName = "MSBuild.exe";
            dialog.Title = "MSBuild.exe を指定してください。";

            if (dialog.ShowDialog() == true)
            {
                MsBuildPath = dialog.FileName;
                SaveSettings();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public void AddHistory(ExecuteHistory.ExecuteType type, string target, bool state)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ExecuteHistory history = new ExecuteHistory()
                {
                    Type = type,
                    Target = target,
                    State = state
                };
                ExecuteHistory.Insert(0, history);
            });
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
