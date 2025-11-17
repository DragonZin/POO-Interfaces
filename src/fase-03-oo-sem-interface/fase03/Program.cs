using System;
using System.Collections.Generic;

namespace Fase03_ProceduralAuth
{
    /// Modo de autenticação — este enum será o switch principal (procedural).
    /// Na Fase 3 esse switch deverá desaparecer e cada modo virar uma implementação concreta.
    public enum AuthMode
    {
        Physical_Biometry,
        Physical_Card,
        Online_Biometry,
        Online_Card,
        // Modo padrão / fallback
        Deny // default: negar acesso
    }


    /// Resultado simplificado de autenticação

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static AuthResult Ok(string msg = "Autenticado") => new AuthResult { Success = true, Message = msg };
        public static AuthResult Fail(string msg = "Negado") => new AuthResult { Success = false, Message = msg };
    }


    /// Representa uma tentativa de autenticação.
    /// Na fase procedural mantemos os dados em um DTO simples.

    public class AuthRequest
    {
        public string Username { get; init; } = string.Empty;
        public string? CardId { get; init; } // quando aplicável
        public byte[]? BiometricSample { get; init; } // representação simplificada
        public string ClientIp { get; init; } = string.Empty; // para online
        public AuthMode Mode { get; init; } = AuthMode.Deny;
    }


    /// "Banco" em memória de usuários / credenciais — simples para testes.

    public static class FakeUserStore
    {
        // usuários com cardId e "template" biométrico simplificado
        public static readonly Dictionary<string, (string CardId, byte[] BiometricTemplate, bool IsPhysicalAllowed, bool IsOnlineAllowed)> Users
            = new Dictionary<string, (string, byte[], bool, bool)>
        {
            { "alice", ("CARD-100", new byte[]{1,2,3,4}, true, true) },      // pode autenticar físico e online
            { "bob",   ("CARD-200", new byte[]{5,6,7,8}, true, false) },     // só físico
            { "eve",   ("CARD-999", new byte[]{9,9,9,9}, false, true) }      // só online
        };

        public static bool TryGet(string username, out (string CardId, byte[] BiometricTemplate, bool IsPhysicalAllowed, bool IsOnlineAllowed) entry)
        {
            return Users.TryGetValue(username.ToLowerInvariant(), out entry);
        }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Fase 2 — Exemplo procedural de Autenticação ===");
            Console.WriteLine();

            // Cenários de teste/exemplos (poderão virar casos de teste na Fase 2 deliverable)
            var scenarios = new List<AuthRequest>
            {
                // 1) Usuário físico com biometria válida
                new AuthRequest { Username = "alice", BiometricSample = new byte[]{1,2,3,4}, Mode = AuthMode.Physical_Biometry },

                // 2) Usuário físico com cartão válido
                new AuthRequest { Username = "bob", CardId = "CARD-200", Mode = AuthMode.Physical_Card },

                // 3) Usuário online com biometria (simulação via client IP + biometric) — alice
                new AuthRequest { Username = "alice", BiometricSample = new byte[]{1,2,3,4}, ClientIp = "203.0.113.5", Mode = AuthMode.Online_Biometry },

                // 4) Usuário online com cartão (ex.: token de cartão virtual) — eve (card mismatch)
                new AuthRequest { Username = "eve", CardId = "CARD-999", ClientIp = "198.51.100.10", Mode = AuthMode.Online_Card },

                // 5) Modo inválido / fallback
                new AuthRequest { Username = "mallory", Mode = AuthMode.Deny }
            };

            foreach (var req in scenarios)
            {
                Console.WriteLine($"--- Cenário: usuário='{req.Username}', modo={req.Mode} ---");
                var result = AuthenticateProcedural(req);
                Console.WriteLine($"Resultado: {(result.Success ? "OK" : "FAIL")} - {result.Message}");
                Console.WriteLine();
            }

            Console.WriteLine("=== Fim dos cenários ===");
        }


        /// Função procedural central: escolhe a estratégia via switch e executa.
        /// NA FASE 3: remover este switch — injetar/obter a implementação adequada.
        public static AuthResult AuthenticateProcedural(AuthRequest request)
        {
            if (request == null) return AuthResult.Fail("Requisição nula");

            // validações básicas
            if (string.IsNullOrWhiteSpace(request.Username))
                return AuthResult.Fail("Username ausente");

            // aqui temos o ponto procedural de decisão: switch sobre AuthMode
            switch (request.Mode)
            {
                case AuthMode.Physical_Biometry:
                    // simulamos leitura do sensor e checagem local
                    return AuthenticatePhysicalBiometry(request);

                case AuthMode.Physical_Card:
                    return AuthenticatePhysicalCard(request);

                case AuthMode.Online_Biometry:
                    // No mundo real, seria upload de amostra + verificação remota
                    return AuthenticateOnlineBiometry(request);

                case AuthMode.Online_Card:
                    return AuthenticateOnlineCard(request);

                case AuthMode.Deny:
                default:
                    return AuthResult.Fail("Modo desconhecido ou não permitido (padrão DENY).");
            }
        }

        #region Implementações Procedurais (simulações)

        private static AuthResult AuthenticatePhysicalBiometry(AuthRequest req)
        {
            // Regras simplificadas:
            // - usuário deve existir
            // - ser permitido acesso físico
            // - biometric sample deve existir e "combinar" com o template (simulado)
            if (!FakeUserStore.TryGet(req.Username, out var entry))
                return AuthResult.Fail("Usuário não encontrado");

            if (!entry.IsPhysicalAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação física");

            if (req.BiometricSample == null)
                return AuthResult.Fail("Amostra biométrica ausente");

            bool match = BiometricMatches(req.BiometricSample, entry.BiometricTemplate);
            return match ? AuthResult.Ok("Biometria física validada") : AuthResult.Fail("Biometria física inválida");
        }

        private static AuthResult AuthenticatePhysicalCard(AuthRequest req)
        {
            // Regras:
            // - usuário existe
            // - verifica se o cardId enviado bate com o cadastrado
            if (!FakeUserStore.TryGet(req.Username, out var entry))
                return AuthResult.Fail("Usuário não encontrado");

            if (!entry.IsPhysicalAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação física");

            if (string.IsNullOrWhiteSpace(req.CardId))
                return AuthResult.Fail("Cartão não apresentado");

            if (string.Equals(req.CardId, entry.CardId, StringComparison.OrdinalIgnoreCase))
                return AuthResult.Ok("Cartão físico validado");
            else
                return AuthResult.Fail("Cartão físico inválido");
        }

        private static AuthResult AuthenticateOnlineBiometry(AuthRequest req)
        {
            // Regras:
            // - usuário existe
            // - permite online
            // - tem amostra
            // - simula checagem de risco (client IP) — bloqueia se IP suspeito (simples)
            if (!FakeUserStore.TryGet(req.Username, out var entry))
                return AuthResult.Fail("Usuário não encontrado");

            if (!entry.IsOnlineAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação online");

            if (req.BiometricSample == null)
                return AuthResult.Fail("Amostra biométrica ausente");

            if (IsIpSuspicious(req.ClientIp))
                return AuthResult.Fail("Acesso de IP suspeito - bloqueado");

            bool match = BiometricMatches(req.BiometricSample, entry.BiometricTemplate);
            return match ? AuthResult.Ok("Biometria online validada") : AuthResult.Fail("Biometria online inválida");
        }

        private static AuthResult AuthenticateOnlineCard(AuthRequest req)
        {
            // Regras:
            // - usuário existe
            // - permite online
            // - verifica validade do token/cartão virtual (simulado)
            if (!FakeUserStore.TryGet(req.Username, out var entry))
                return AuthResult.Fail("Usuário não encontrado");

            if (!entry.IsOnlineAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação online");

            if (string.IsNullOrWhiteSpace(req.CardId))
                return AuthResult.Fail("Token/cartão não apresentado");

            if (IsOnlineCardTokenValid(req.CardId))
                return AuthResult.Ok("Cartão/token online validado");
            else
                return AuthResult.Fail("Token/cartão online inválido");
        }

        #endregion

        #region Helpers (simulações simples)

        // Simula verificação biométrica (comparação de arrays).
        // Na prática, isso seria chamada a um algoritmo de comparação/serviço.
        private static bool BiometricMatches(byte[] sample, byte[] template)
        {
            if (sample == null || template == null) return false;
            if (sample.Length != template.Length) return false;

            // comparação simples byte-a-byte (apenas para simulação)
            for (int i = 0; i < sample.Length; i++)
                if (sample[i] != template[i]) return false;

            return true;
        }

        // Simula checagem de IP suspeito (lista simples)
        private static bool IsIpSuspicious(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return false;
            // IPs "simulados" considerados suspeitos
            var suspicious = new HashSet<string> { "10.0.0.66", "203.0.113.66" };
            return suspicious.Contains(ip);
        }

        // Simula validação de token/cartão online
        private static bool IsOnlineCardTokenValid(string token)
        {
            // Neste exemplo consideramos tokens que começam com "CARD-" válidos
            return token.StartsWith("CARD-", StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
