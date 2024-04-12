namespace reflection.infra
{
    public static class Utils
    {
        public static string ConvertPathToAssemblyName(string path)
        {
            string prefixAssembly = "reflection";
            string formatedPath = path.Replace('/', '.');
            string fullPath = $"{prefixAssembly}{formatedPath}";

            return fullPath;
        }

        public static string GetContentFile(string path)
        {
            if (path.EndsWith(".css"))
                return "text/css; charset=utf-8";

            if (path.EndsWith(".js"))
                return "application/js; charset=utf-8";

            if (path.EndsWith(".html"))
                return "text/html; charset=utf-8";

            throw new NotImplementedException("Tipo de conteúdo não previsto!");
        }
    }
}
