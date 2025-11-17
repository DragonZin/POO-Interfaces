namespace Fase04.Auth
{
    /// <summary>
    /// Implementação simples de cartão RFID + senha.
    /// Aceita identidades que começam com "card-" e senha exata "1234".
    /// </summary>
    public sealed class RfidPasswordAuthenticator : IAuthenticator
    {
        public bool Authenticate(string identity, string? secret = null) =>
            !string.IsNullOrWhiteSpace(identity) &&
            identity.StartsWith("card-") &&
            secret == "1234";
    }
}
