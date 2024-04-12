namespace Reflection.Service
{
    public interface ICambioService
    {
        decimal Calcular(string coinOrigin, string coinDestination, decimal value);
    }
}
