using Reflection.Service;
using Reflection.Service.Cambio;
using System.Reflection;

namespace reflection.Controller
{
    public class CambioController
    {
        private ICambioService _cambioSerive;
        public CambioController() 
        {
            _cambioSerive = new CambioTesteService();
        }

        public string MXN()
        {
            var valorFinal = _cambioSerive.Calcular("MXN", "BRL", 1);
            var fullNameResource = "reflection.View.cambio.MXN.html";
            var assembly = Assembly.GetExecutingAssembly();
            var streamRecurso = assembly.GetManifestResourceStream(fullNameResource);

            var streamReader = new StreamReader(streamRecurso);
            var textPage = streamReader.ReadToEnd();
            var textResult = textPage.Replace("VALOR_EM_REAIS", valorFinal.ToString());

            return textResult;
        }

        public string USD()
        {
            return null;
        }
    }
}
