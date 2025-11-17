namespace Fase04.Logging
{
    /// <summary>
    /// Ponto único de composição para loggers de acesso.
    /// </summary>
    public static class AccessLoggerFactory
    {
        public static IAccessLogger Resolve(string mode) =>
            mode switch
            {
                "logs-biometricos" => new BiometricAccessLogger(),
                "logs-credenciais" => new CredentialAccessLogger(),
                _ => throw new ArgumentException($"Modo de logger desconhecido: {mode}")
            };
    }
}
