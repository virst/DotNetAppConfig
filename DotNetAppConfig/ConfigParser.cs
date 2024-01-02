using System.Reflection;
using System.Text.Json;

namespace DotNetAppConfig
{
    /// <summary>
    /// Provides methods for parsing application parameters from command line, environment variables, or files.
    /// </summary>
    public static class ConfigParser
    {
        private static readonly System.Globalization.CultureInfo CultureInfo = new("en-US");

        /// <summary>
        /// Parsing parameter values from the command line like "param=val"
        /// </summary>
        /// <typeparam name="T">Parameter storage type</typeparam>
        /// <param name="obj">Object to fill values</param>
        /// <param name="args">Array of command line options</param>
        public static void ParseArgs<T>(T obj, string[] args) => ParseArgs(obj, args, StringComparer.Ordinal);

        /// <summary>
        /// Parsing parameter values from the command line like "param=val"
        /// </summary>
        /// <typeparam name="T">Parameter storage type</typeparam>
        /// <param name="obj">Object to fill values</param>
        /// <param name="args">Array of command line options</param>
        /// <param name="nameComparer">Methods to support the comparison of naming for equality.</param>
        public static void ParseArgs<T>(T obj, string[] args, IEqualityComparer<string>? nameComparer)
        {
            var props = typeof(T).GetProperties();
            var vals = new Dictionary<string, List<string?>>(nameComparer);
            foreach (var a in args)
            {
                var ss = a.Split('=').Select(s => s.Trim()).ToArray();
                if (!vals.ContainsKey(ss[0])) vals[ss[0]] = new();
                if (ss.Length > 1) vals[ss[0]].Add(ss[1]);
            }

            foreach (var p in props)
            {
                if (!vals.ContainsKey(p.Name)) continue;
                if (p.PropertyType == typeof(bool) && vals[p.Name].Count == 0)
                {
                    p.SetValue(obj, true);
                    continue;
                }
                if (p.PropertyType.IsArray)
                {
                    var elementType = p.PropertyType.GetElementType();
                    if (elementType == null) continue;
                    var list = vals[p.Name].Select(v => Convert.ChangeType(v, elementType, CultureInfo)).ToList();
                    var array = Array.CreateInstance(elementType, list.Count);
                    for (int i = 0; i < list.Count; i++)
                        array.SetValue(list[i], i);
                    p.SetValue(obj, array);
                    continue;
                }
                var val = Convert.ChangeType(vals[p.Name].LastOrDefault(), p.PropertyType, CultureInfo);
                p.SetValue(obj, val);
            }
        }

        /// <summary>
        /// Parsing values from environment variables
        /// </summary>
        /// <typeparam name="T">Parameter storage type</typeparam>
        /// <param name="obj">Object to fill values</param>
        /// <param name="autoUpperSnakeCase"></param>
        public static void ParseEnvironment<T>(T obj, bool autoUpperSnakeCase = true)
        {
            var props = typeof(T).GetProperties();
            foreach (var p in props)
            {
                var nm = p.Name;
                if (autoUpperSnakeCase) nm = Utils.ToUpperSnakeCase(nm);
                var eval = Environment.GetEnvironmentVariable(nm);
                if (eval == null) continue;
                var val = Convert.ChangeType(eval, p.PropertyType, CultureInfo);
                p.SetValue(obj, val);
            }
        }

        /// <summary>
        /// Parsing parameter values from a file with lines like “param=val”
        /// </summary>
        /// <typeparam name="T">Parameter storage type</typeparam>
        /// <param name="obj">Object to fill values</param>
        /// <param name="fn">Path to file</param>
        public static void ParseSimpleFile<T>(T obj, string fn)
        {
            var array = File.ReadAllLines(fn);
            ParseArgs<T>(obj, array);
        }

        /// <summary>
        /// Parsing parameter values from a json file
        /// </summary>
        /// <typeparam name="T">Parameter storage type</typeparam>
        /// <param name="obj">Object to fill values</param>
        /// <param name="fn">Path to file</param>
        public static void ParseJsonFile<T>(T obj, string fn)
        {
            using var reader = File.OpenRead(fn);
            using JsonDocument js = JsonDocument.Parse(reader);

            var props = typeof(T).GetProperties();
            foreach (var p in props)
            {
                if (js.RootElement.TryGetProperty(p.Name, out var jp))
                {
                    var val = Convert.ChangeType(jp.GetRawText(), p.PropertyType, CultureInfo);
                    p.SetValue(obj, val);
                }
            }
        }

        /// <summary>
        /// Parsing parameter values from the command line, with support for named parameters like -p/--param and param=val
        /// </summary>
        /// <typeparam name="T">Parameter storage type</typeparam>
        /// <param name="obj">Object to fill values</param>
        /// <param name="args">Array of command line options</param>
        public static void ParseCommandLine<T>(T obj, string[] args)
        {
            var propNames = new Dictionary<string, PropertyInfo>(StringComparer.CurrentCultureIgnoreCase);
            var props = typeof(T).GetProperties();
            var arrayVals = new Dictionary<PropertyInfo, List<string>>();

            foreach (var p in props)
            {
                propNames[p.Name] = p;
                var a = p.GetCustomAttribute<ParamAttribute>();
                if (a == null) continue;
                if (a.ArgsName != null) propNames["--" + a.ArgsName] = p;
                if (a.ArgsShortName != null) propNames["-" + a.ArgsShortName] = p;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (a.Contains('='))
                {
                    var ss = a.Split('=');
                    if (propNames.ContainsKey(ss[0]))
                    {
                        var p = propNames[ss[0]];
                        var val = Convert.ChangeType(ss[1], p.PropertyType, CultureInfo);
                        p.SetValue(obj, val);
                    }
                    continue;
                }
                if (propNames.ContainsKey(a))
                {
                    var p = propNames[a];
                    if (p.PropertyType == typeof(bool))
                    {
                        p.SetValue(obj, true);
                        continue;
                    }
                    if (p.PropertyType.IsArray)
                    {
                        if (!arrayVals.ContainsKey(p))
                            arrayVals[p] = new();
                        arrayVals[p].Add(args[++i]);
                        continue;
                    }
                    var val = Convert.ChangeType(args[++i], p.PropertyType, CultureInfo);
                    p.SetValue(obj, val);
                }
            }

            foreach (var av in arrayVals)
            {
                var p = av.Key;
                var elementType = p.PropertyType.GetElementType();
                if (elementType == null) continue;
                var list = av.Value.Select(v => Convert.ChangeType(v, elementType, CultureInfo)).ToList();
                var array = Array.CreateInstance(elementType, list.Count);
                for (int i = 0; i < list.Count; i++)
                    array.SetValue(list[i], i);
                p.SetValue(obj, array);
            }

        }
    }
}
