namespace DotNetAppConfig
{
    internal static class Utils
    {
        public static string ToUpperSnakeCase(string s)
        {
            var rez = "";
            foreach (var c in s)
            {
                if (char.IsUpper(c) && rez != "")
                    rez += '_';
                rez += char.ToUpper(c);
            }
            return rez;
        }
    }
}
