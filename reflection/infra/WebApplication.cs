using reflection.Controller;
using System.Net;
using System.Reflection;
using System.Text;

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

            if (Utils.ItIsFile(path))
            {
                var manipulate = new ProcessRequestFile();
                manipulate.Manipulate(response, path);
            } 
            else if (path == "/Cambio/MXN")
            {
                var controller = new CambioController();
                var pageContent = controller.MXN();

                var bufferFile = Encoding.UTF8.GetBytes(pageContent);

                response.StatusCode = 200;
                response.ContentType = "text/html; charset=utf-8";
                response.ContentLength64 = bufferFile.Length;

                response.OutputStream.Write(bufferFile, 0, bufferFile.Length);
                response.OutputStream.Close();
            }
            else if (path == "/Cambio/USD")
            {
                var controller = new CambioController();
                var pageContent = controller.USD();

                var bufferFile = Encoding.UTF8.GetBytes(pageContent);

                response.StatusCode = 200;
                response.ContentType = "text/html; charset=utf-8";
                response.ContentLength64 = bufferFile.Length;

                response.OutputStream.Write(bufferFile, 0, bufferFile.Length);
                response.OutputStream.Close();
            }

            httpListener.Stop();
        }
    }
}