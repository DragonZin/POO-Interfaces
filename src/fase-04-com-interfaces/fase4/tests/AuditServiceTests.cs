using Fase04.Logging;
using Fase04.Tests.Doubles;
using Xunit;

namespace Fase04.Tests
{
    public class AuditServiceTests
    {
        [Fact]
        public void RegisterAccess_DeveRegistrarUmaChamadaNoLogger()
        {
            // arrange
            var fakeLogger = new FakeLogger();
            var service = new AuditService(fakeLogger);

            // act
            service.RegisterAccess("user-1", "biometria", true);

            // assert
            Assert.Single(fakeLogger.Calls);
            Assert.Equal("user-1", fakeLogger.Calls[0].Id);
            Assert.Equal("biometria", fakeLogger.Calls[0].Method);
            Assert.True(fakeLogger.Calls[0].Success);
        }
    }
}
