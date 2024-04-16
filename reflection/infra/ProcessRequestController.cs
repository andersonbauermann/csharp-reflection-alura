using System.Net;
using System.Text;

namespace reflection.infra
{
    public class ProcessRequestController
    {
        public void Manipulate(HttpListenerResponse response, string path)
        {
            var parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var controllerName = parts[0];
            var actionName = parts[1];

            var controllerFullName = $"reflection.Controller.{controllerName}Controller";
            var controllerWrapper = Activator.CreateInstance("reflection", controllerFullName, new object[0]);
            var controller = controllerWrapper.Unwrap();

            var methodInfo = controller.GetType().GetMethod(actionName);

            var resultAction = (string)methodInfo.Invoke(controller, new object[0]);

            var buffer = Encoding.UTF8.GetBytes(resultAction);
            response.StatusCode = 200;
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = buffer.Length;

            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
