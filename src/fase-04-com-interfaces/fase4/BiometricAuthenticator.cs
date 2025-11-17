namespace Fase04.Auth
{
    /// <summary>
    /// Implementação simples que simula um autenticador biométrico.
    /// Qualquer identidade não vazia iniciando com "bio-" é aceita.
    /// </summary>
    public sealed class BiometricAuthenticator : IAuthenticator
    {
        public bool Authenticate(string identity, string? secret = null) =>
            !string.IsNullOrWhiteSpace(identity) && identity.StartsWith("bio-");
    }
}
