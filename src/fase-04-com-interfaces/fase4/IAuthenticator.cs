namespace Fase04.Auth
{
    /// <summary>
    /// Contrato para autenticação de identidades em diferentes meios
    /// (biometria, cartão + senha, etc.).
    /// </summary>
    public interface IAuthenticator
    {
        bool Authenticate(string identity, string? secret = null);
    }
}
