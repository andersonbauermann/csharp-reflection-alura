using System.Net;
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
            var httpListener = new HttpListener();

            foreach (var prefix in _prefixes)
            {
                httpListener.Prefixes.Add(prefix);
            }

            httpListener.Start();

            var context = httpListener.GetContext();
            var request = context.Request;
            var response = context.Response;

            var contentResponse = "Hello World!";
            var contentResponseBytes = Encoding.UTF8.GetBytes(contentResponse);

            response.ContentType = "text/html; charset=utf-8";
            response.StatusCode = 200;
            response.ContentLength64 = contentResponseBytes.Length;

            response.OutputStream.Write(contentResponseBytes, 0, contentResponseBytes.Length);

            response.OutputStream.Close();

            httpListener.Stop();
        }
    }
}