using System.Reflection;
using System.Runtime.CompilerServices;

namespace reflection.infra
{
    public abstract class ControllerBase
    {
        protected string View([CallerMemberName]string nameFile = null)
        {
            var type = GetType();
            var dirName = type.Name.Replace("Controller", "");
            var fullNameResource = $"reflection.View.{dirName}.{nameFile}.html";
            var assembly = Assembly.GetExecutingAssembly();
            var streamRecurso = assembly.GetManifestResourceStream(fullNameResource);

            var streamReader = new StreamReader(streamRecurso);
            var textPage = streamReader.ReadToEnd();
            return textPage;
        }
    }
}
