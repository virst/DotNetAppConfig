using DotNetAppConfig;

namespace Demo3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Params p = new();
            ConfigParser.ParseSimpleFile(p, "demo.txt");
            Console.WriteLine(p);

            p = new();
            ConfigParser.ParseJsonFile(p, "demo.json");
            Console.WriteLine(p);
        }
    }
}
