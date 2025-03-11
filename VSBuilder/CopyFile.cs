using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder
{
    /// <summary>
    /// コピーファイル設定クラス
    /// </summary>
    public class CopyFile
    {
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string DestinationPath { get; set; } = string.Empty;
        public bool IsOutput { get; set; } = false;
    }
}
