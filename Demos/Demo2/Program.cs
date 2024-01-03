using DotNetAppConfig;

namespace Demo2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Params p = new();
            ConfigParser.ParseEnvironment(p);
            Console.WriteLine(p);
        }
    }
}
