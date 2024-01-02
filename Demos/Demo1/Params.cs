using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1
{
    internal class Params
    {
        public int Val { get; set; }
        public string? Name { get; set; }

        public bool Rule1 { get; set; } 
        public bool Rule2 { get; set; } 
        public bool Rule3 { get; set; } 
        public int[]? Nums { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new ();
            sb.AppendLine($"Val={Val}");
            sb.AppendLine($"Name={Name}");
            sb.AppendLine($"Rule1={Rule1}");
            sb.AppendLine($"Rule2={Rule2}");
            sb.AppendLine($"Rule3={Rule3}");
            if (Nums != null)
                sb.AppendLine($"Nums=[{string.Join(',', Nums)}]");
            return sb.ToString();
        }
    }
}
