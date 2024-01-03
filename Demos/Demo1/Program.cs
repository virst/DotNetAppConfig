using DotNetAppConfig;

namespace Demo1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "Val=12", "Name=zorro", "Nums=1", "Nums=3", "Nums=2", "rule1", "rule3=true" };
            Params p = new();
            ConfigParser.ParseArgs(p, args);
            Console.WriteLine(p);

            p = new();
            ConfigParser.ParseArgs(p, args, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine(p);
        }
    }
}
