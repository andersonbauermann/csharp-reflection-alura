using reflection.infra;
using Reflection.Service;
using Reflection.Service.Cambio;

namespace reflection.Controller
{
    public class CambioController : ControllerBase
    {
        private ICambioService _cambioSerive;
        public CambioController() 
        {
            _cambioSerive = new CambioTesteService();
        }

        public string MXN()
        {
            var finalValue = _cambioSerive.Calcular("MXN", "BRL", 1);
            var textPage = View();
            var textResult = textPage.Replace("VALOR_EM_REAIS", finalValue.ToString());

            return textResult;
        }

        public string USD()
        {
            var finalValue = _cambioSerive.Calcular("USD", "BRL", 1);
            var textPage = View();
            var textResult = textPage.Replace("VALOR_EM_REAIS", finalValue.ToString());

            return textResult;
        }

        public string Calculation(string originCurrency, string destinationCurrency, decimal value)
        {
            var finalValue = _cambioSerive.Calcular(originCurrency, destinationCurrency, value);
            var textPage = View();
            var textResult = textPage
                                .Replace("VALOR_MOEDA_ORIGEM", value.ToString())
                                .Replace("MOEDA_ORIGEM", finalValue.ToString())
                                .Replace("VALOR_MOEDA_DESTINO", originCurrency)
                                .Replace("MOEDA_DESTINO", destinationCurrency);


            //VALOR_MOEDA_ORIGEM MOEDA_ORIGEM = VALOR_MOEDA_DESTINO MOEDA_DESTINO
            return textResult;
      }
    }
}
