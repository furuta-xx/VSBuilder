using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder.Models
{
    /// <summary>
    /// ソリューション設定クラス
    /// </summary>
    public class SolutionSetting
    {
        public int ID { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string SolutionFilePath { get; set; } = string.Empty;
        public string BuildConfig { get; set; } = "Release";
        public string Platform { get; set; } = "Any CPU";
        public string ModulePath { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
        public bool IsOutput { get; set; } = false;

        public SolutionSetting()
        {
        }

        public SolutionSetting(SolutionSetting other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(SolutionSetting other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            ID = other.ID;
            Name = other.Name;
            SolutionFilePath = other.SolutionFilePath;
            BuildConfig = other.BuildConfig;
            Platform = other.Platform;
            ModulePath = other.ModulePath;
            OutputPath = other.OutputPath;
            IsOutput = other.IsOutput;
        }
    }
}
