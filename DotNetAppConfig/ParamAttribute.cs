using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAppConfig
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParamAttribute : Attribute
    {
        public readonly string? ArgsName;
        public readonly char? ArgsShortName = null;
        public readonly string? EnvironmentName;

        public ParamAttribute(string? argsName = null, string? environmentName = null)
        {
            ArgsName = argsName;
            EnvironmentName = environmentName;
        }
        public ParamAttribute(char argsShortName, string? argsName = null, string? environmentName = null)
            : this(argsName, environmentName)
        {
            ArgsShortName = argsShortName;
        }
    }
}
