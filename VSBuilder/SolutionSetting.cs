using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder
{
    /// <summary>
    /// ソリューション設定クラス
    /// </summary>
    public class SolutionSetting
    {
        public string Name { get; set; } = string.Empty;
        public string SolutionFilePath { get; set; } = string.Empty;
        public string BuildConfig { get; set; } = "Release";
        public string Platform { get; set; } = "Any CPU";
        public string ModulePath { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
        public bool IsOutput { get; set; } = false;

        public static void Copy(SolutionSetting setting1, SolutionSetting setting2)
        {
            setting1.Name = setting2.Name;
            setting1.SolutionFilePath = setting2.SolutionFilePath;
            setting1.BuildConfig = setting2.BuildConfig;
            setting1.Platform = setting2.Platform;
            setting1.ModulePath = setting2.ModulePath;
            setting1.OutputPath = setting2.OutputPath;
            setting1.IsOutput = setting2.IsOutput;
        }
    }
}
