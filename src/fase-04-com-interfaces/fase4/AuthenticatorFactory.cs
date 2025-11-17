namespace Fase04.Auth
{
    /// <summary>
    /// Ponto único de composição para autenticação.
    /// Converte a política (mode) em implementação concreta.
    /// </summary>
    public static class AuthenticatorFactory
    {
        public static IAuthenticator Resolve(string mode) =>
            mode switch
            {
                "biometria" => new BiometricAuthenticator(),
                "cartao-senha" => new RfidPasswordAuthenticator(),
                _ => throw new ArgumentException($"Modo de autenticação desconhecido: {mode}")
            };
    }
}
