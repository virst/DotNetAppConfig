# DotNetConfig

A library for parsing and loading application settings from the command line, environment variables or files.

### Example 1
Parsing parameter values from the command line like "param=val"

```csharp
class Params
    {
        public int Val { get; set; }
        public string? Name { get; set; }

        public bool Rule1 { get; set; } 
        public bool Rule2 { get; set; } 
        public bool Rule3 { get; set; } 
        public int[]? Nums { get; set; }
    }

      args = new string[] { "Val=12", "Name=zorro", "Nums=1", "Nums=3", "Nums=2", "rule1", "rule3=true" };
      Params p = new();
      ConfigParser.ParseArgs(p, args);
```

### Example 2
Parsing values from environment variables
```csharp
using DotNetConfig;

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
```

### Example 3
Parsing parameter values from a file

json
```
{
  "Val": 104,
  "Name": "Blacksmith"
}
```

txt
```
Val = 10
FloatVal = 1.56
Name = Gin
Rule2
```

```csharp
using DotNetConfig;

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

```


### Example 4
Parsing parameter values from the command line, with support for named parameters like -p/--param and param=val
```csharp
  args = new string[] { "--val", "12", "Name=zorro", "-n", "1", "-n", "2", "rule1", "rule3=true" };
  Params p = new();
  ConfigParser.ParseCommandLine(p, args);
  Console.WriteLine(p);
```
