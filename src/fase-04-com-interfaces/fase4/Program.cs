using Fase04.Auth;
using Fase04.Logging;

namespace Fase04
{
    /// <summary>
    /// Programa de exemplo apenas para demonstrar a composição.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Demonstração Fase 4 — Interface plugável e testável ===");
            Console.Write("Informe o modo de autenticação (biometria, cartao-senha): ");
            var mode = Console.ReadLine() ?? string.Empty;

            Console.Write("Identidade (ex: bio-123 ou card-999): ");
            var identity = Console.ReadLine() ?? string.Empty;

            string? secret = null;
            if (mode == "cartao-senha")
            {
                Console.Write("Senha: ");
                secret = Console.ReadLine();
            }

            // Composição em um ponto único
            var authenticator = AuthenticatorFactory.Resolve(mode);
            var controller = new AccessController(authenticator);

            var result = controller.RequestAccess(identity, secret);
            Console.WriteLine($"Resultado autenticação: {result}");

            // Exemplo de logger plugável (só para ilustrar)
            var loggerMode = mode == "biometria" ? "logs-biometricos" : "logs-credenciais";
            var logger = AccessLoggerFactory.Resolve(loggerMode);
            var audit = new AuditService(logger);
            audit.RegisterAccess(identity, mode, result == "Acesso liberado");

            Console.WriteLine("Registro de auditoria efetuado (simulado, sem I/O real).");
        }
    }
}
