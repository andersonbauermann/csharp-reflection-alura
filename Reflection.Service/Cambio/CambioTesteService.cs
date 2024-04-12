namespace Reflection.Service.Cambio
{
    public class CambioTesteService : ICambioService
    {
        private readonly Random _random = new Random();
        public decimal Calcular(string coinOrigin, string coinDestination, decimal value)
            => value * (decimal)_random.NextDouble();
    }
}
