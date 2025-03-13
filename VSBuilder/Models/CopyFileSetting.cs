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
        public int ID { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string DestinationPath { get; set; } = string.Empty;
        public bool IsOutput { get; set; } = false;

        public CopyFileSetting()
        {
        }

        public CopyFileSetting(CopyFileSetting other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(CopyFileSetting other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            ID = other.ID;
            Name = other.Name;
            SourcePath = other.SourcePath;
            DestinationPath = other.DestinationPath;
            IsOutput = other.IsOutput;
        }
    }
}
