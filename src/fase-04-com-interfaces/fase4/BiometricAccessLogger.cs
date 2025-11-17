namespace Fase04.Logging
{
    /// <summary>
    /// Implementação de logger para acessos biométricos.
    /// Nesta fase, deixamos o método "vazio" apenas para ilustrar
    /// a composição plugável.
    /// </summary>
    public sealed class BiometricAccessLogger : IAccessLogger
    {
        public void LogAccess(string identity, DateTime when, string method, bool success)
        {
            // Implementação simulada (sem I/O real).
            // Poderia, por exemplo, acumular em memória ou enviar para outro serviço.
        }
    }
}
