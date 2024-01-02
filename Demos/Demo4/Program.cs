using DotNetConfig;

namespace Demo4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "--val", "12", "Name=zorro", "-n", "1", "-n", "2", "rule1", "rule3=true" };
            Params p = new();
            ConfigParser.ParseCommandLine(p, args);
            Console.WriteLine(p);
        }
    }
}
