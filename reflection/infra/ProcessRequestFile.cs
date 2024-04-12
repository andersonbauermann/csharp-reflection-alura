using System.Net;
using System.Reflection;

namespace reflection.infra
{
    public class ProcessRequestFile
    {
        public void Manipulate(HttpListenerResponse response, string path)
        {
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
                using (resourceStream)
                {
                    var bytesResource = new byte[resourceStream.Length];

                    resourceStream.Read(bytesResource, 0, (int)resourceStream.Length);

                    response.ContentType = Utils.GetContentFile(path);
                    response.StatusCode = 200;
                    response.ContentLength64 = resourceStream.Length;

                    response.OutputStream.Write(bytesResource, 0, bytesResource.Length);
                    response.OutputStream.Close();
                }
            }
        }
    }
}
