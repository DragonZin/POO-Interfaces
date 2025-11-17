using Fase04.Auth;
using Fase04.Tests.Doubles;
using Xunit;

namespace Fase04.Tests
{
    public class AccessControllerTests
    {
        [Fact]
        public void RequestAccess_DeveRetornarAcessoLiberado_QuandoAuthenticatorRetornaTrue()
        {
            // arrange
            var fake = new FakeAuthenticator((id, secret) => id == "valid");
            var controller = new AccessController(fake);

            // act
            var result = controller.RequestAccess("valid");

            // assert
            Assert.Equal("Acesso liberado", result);
        }

        [Fact]
        public void RequestAccess_DeveRetornarAcessoNegado_QuandoAuthenticatorRetornaFalse()
        {
            var fake = new FakeAuthenticator((id, secret) => false);
            var controller = new AccessController(fake);

            var result = controller.RequestAccess("qualquer");

            Assert.Equal("Acesso negado", result);
        }
    }
}
