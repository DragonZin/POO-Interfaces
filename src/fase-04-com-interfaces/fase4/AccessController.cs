namespace Fase04.Auth
{
    /// <summary>
    /// Cliente que depende apenas do contrato IAuthenticator.
    /// Ele não conhece as implementações concretas.
    /// </summary>
    public class AccessController
    {
        private readonly IAuthenticator _auth;

        public AccessController(IAuthenticator auth)
        {
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));
        }

        public string RequestAccess(string identity, string? secret = null) =>
            _auth.Authenticate(identity, secret)
                ? "Acesso liberado"
                : "Acesso negado";
    }
}
