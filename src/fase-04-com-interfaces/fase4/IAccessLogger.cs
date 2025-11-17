namespace Fase04.Logging
{
    /// <summary>
    /// Contrato para registrar tentativas de acesso.
    /// </summary>
    public interface IAccessLogger
    {
        void LogAccess(string identity, DateTime when, string method, bool success);
    }
}
