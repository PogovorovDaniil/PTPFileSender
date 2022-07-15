namespace PTPFileSender.Helpers
{
    internal static class FileHelper
    {
        public static string NameFromPath(string path)
        {
            path = path.Replace("\\", "/");
            string[] nodes = path.Split("/");
            return nodes[nodes.Length - 1];
        }

        public static (string, string) SplitFileName(string name)
        {
            string[] strings = name.Split('.');
            string extention = "";
            if(strings.Length > 0) extention = strings[strings.Length - 1];
            name = name.Substring(0, name.Length - extention.Length - 1);
            return (name, extention);
        }
        public static string AddNameToPath(string path, string name)
        {
            path = path.Replace("\\", "/");
            if (path[path.Length - 1] == '/') return path + name;
            return path + '/' + name;
        }
    }
}
