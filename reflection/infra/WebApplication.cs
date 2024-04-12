using System.Net;
using System.Reflection;

namespace reflection.infra
{
    public class WebApplication
    {
        private readonly string[] _prefixes;
        public WebApplication(string [] prefixes)
        {
            if (prefixes is null)
                throw new ArgumentNullException(nameof(prefixes));

            _prefixes = prefixes;
        }
        public void Open()
        {
            while (true)
                ProcessRequest();
        }

        private void ProcessRequest()
        {
            var httpListener = new HttpListener();

            foreach (var prefix in _prefixes)
            {
                httpListener.Prefixes.Add(prefix);
            }

            httpListener.Start();

            var context = httpListener.GetContext();
            var request = context.Request;
            var response = context.Response;

            var path = request.Url.AbsolutePath;

            var assembly = Assembly.GetExecutingAssembly();
            string nameResource = Utils.ConvertPathToAssemblyName(path);
            
            var resourceStream = assembly.GetManifestResourceStream(nameResource);

            if (resourceStream is null)
            {
                response.StatusCode = 404;
                response.OutputStream.Close();
            }
            else
            {
                var bytesResource = new byte[resourceStream.Length];

                resourceStream.Read(bytesResource, 0, (int)resourceStream.Length);

                response.ContentType = Utils.GetContentFile(path);
                response.StatusCode = 200;
                response.ContentLength64 = resourceStream.Length;
                response.OutputStream.Write(bytesResource, 0, bytesResource.Length);
                response.OutputStream.Close();
            }


            httpListener.Stop();
        }
    }
}