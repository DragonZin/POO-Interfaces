using System;
using Fase04.Auth;

namespace Fase04.Tests.Doubles
{
    /// <summary>
    /// Dublê (fake) para IAuthenticator, usado em testes do AccessController.
    /// </summary>
    public sealed class FakeAuthenticator : IAuthenticator
    {
        private readonly Func<string, string?, bool> _fn;

        public FakeAuthenticator(Func<string, string?, bool> fn)
        {
            _fn = fn;
        }

        public bool Authenticate(string identity, string? secret = null) =>
            _fn(identity, secret);
    }
}
