namespace Fase04.Logging
{
    /// <summary>
    /// Implementação de logger para acessos via credenciais (cartão + senha).
    /// </summary>
    public sealed class CredentialAccessLogger : IAccessLogger
    {
        public void LogAccess(string identity, DateTime when, string method, bool success)
        {
            // Implementação simulada (sem I/O real).
        }
    }
}
