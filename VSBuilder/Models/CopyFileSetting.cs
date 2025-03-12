using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder.Models
{
    /// <summary>
    /// コピーファイル設定クラス
    /// </summary>
    public class CopyFileSetting
    {
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string DestinationPath { get; set; } = string.Empty;
        public bool IsOutput { get; set; } = false;

        public void CopyFrom(CopyFileSetting other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Name = other.Name;
            SourcePath = other.SourcePath;
            DestinationPath = other.DestinationPath;
            IsOutput = other.IsOutput;
        }
    }
}
