using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo2
{
    internal class Params
    {
        public int Val { get; set; }
        public float FloatVal { get; set; }
        public string? Name { get; set; }
        public bool Rule1 { get; set; }
        public bool Rule2 { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Val={Val}");
            sb.AppendLine($"FloatVal={FloatVal}");
            sb.AppendLine($"Name={Name}");
            sb.AppendLine($"Rule1={Rule1}");
            sb.AppendLine($"Rule2={Rule2}");
            return sb.ToString();
        }
    }
}
