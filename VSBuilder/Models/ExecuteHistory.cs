using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSBuilder.Models
{
    public class ExecuteHistory
    {
        public enum ExecuteType
        {
            Build = 0,
            CopyFile,
        }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public ExecuteType Type { get; set; } = ExecuteType.Build;

        public string Target { get; set; } = string.Empty;

        public bool State { get; set; } = false;

        public string StateText { get => State ? "成功" : "失敗"; }
    }
}
