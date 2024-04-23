using System.Net;

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

            var path = request.Url.PathAndQuery;

            if (Utils.ItIsFile(path))
            {
                var manipulate = new ProcessRequestFile();
                manipulate.Manipulate(response, path);
            } 
            else
            {
                var manipulator = new ProcessRequestController();
                manipulator.Manipulate(response, path);
            }

            httpListener.Stop();
        }
    }
}