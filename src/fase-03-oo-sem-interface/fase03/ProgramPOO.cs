// src/fase-03-oo-sem-interface/Program.cs
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Fase03_OO_SemInterface_Auth
{
    public enum AuthMode
    {
        Physical_Biometry,
        Physical_Card,
        Online_Biometry,
        Online_Card,
        Deny
    }

    public record AuthRequest
    {
        public string Username { get; init; } = string.Empty;
        public string? CardId { get; init; }
        public byte[]? BiometricSample { get; init; }
        public string ClientIp { get; init; } = string.Empty;
        public AuthMode Mode { get; init; } = AuthMode.Deny;
    }

    public record AuthResult(bool Success, string Message)
    {
        public static AuthResult Ok(string msg = "Autenticado") => new(true, msg);
        public static AuthResult Fail(string msg = "Negado") => new(false, msg);
    }

    // Representa a entidade persistida do usuário
    public sealed class UserRecord
    {
        public string Username { get; }
        public string CardId { get; } // cartão associado
        public byte[] BiometricTemplate { get; }
        public bool IsPhysicalAllowed { get; }
        public bool IsOnlineAllowed { get; }

        public UserRecord(string username, string cardId, byte[] biometricTemplate, bool physicalAllowed, bool onlineAllowed)
        {
            Username = username;
            CardId = cardId;
            BiometricTemplate = biometricTemplate;
            IsPhysicalAllowed = physicalAllowed;
            IsOnlineAllowed = onlineAllowed;
        }
    }

    // O "cadastro" armazenado no cartão. No mundo real isso seria um payload assinado, não texto plano.
    public sealed class CardPayload
    {
        public string RegisteredUsername { get; }
        public string CardAuthToken { get; } // token/ticket associado ao cartão (simulação)
        public DateTime IssuedAt { get; }
        public DateTime? ExpiresAt { get; }

        public CardPayload(string registeredUsername, string cardAuthToken, DateTime issuedAt, DateTime? expiresAt = null)
        {
            RegisteredUsername = registeredUsername;
            CardAuthToken = cardAuthToken;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
        }

        public bool IsExpired() => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    }

    // "Banco" de usuários (simulação)
    public static class FakeUserStore
    {
        public static readonly Dictionary<string, UserRecord> Users = new(StringComparer.OrdinalIgnoreCase)
        {
            ["alice"] = new UserRecord("alice", "CARD-100", new byte[]{1,2,3,4}, true, true),
            ["bob"]   = new UserRecord("bob",   "CARD-200", new byte[]{5,6,7,8}, true, false),
            ["eve"]   = new UserRecord("eve",   "CARD-999", new byte[]{9,9,9,9}, false, true)
        };

        public static bool TryGet(string username, out UserRecord? user)
        {
            return Users.TryGetValue(username, out user);
        }

        public static bool TryGetByCard(string cardId, out UserRecord? user)
        {
            user = Users.Values.FirstOrDefault(u => string.Equals(u.CardId, cardId, StringComparison.OrdinalIgnoreCase));
            return user != null;
        }
    }

    // "Banco" de cartões: o cartão físico contém um payload que pode ser lido
    public static class FakeCardStore
    {
        // cardId -> payload
        public static readonly Dictionary<string, CardPayload> Cards = new(StringComparer.OrdinalIgnoreCase)
        {
            // Para simular: o token é um valor que o servidor conhece/aceita
            ["CARD-100"] = new CardPayload("alice", "TOKEN-ALICE-100", DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(365)),
            ["CARD-200"] = new CardPayload("bob",   "TOKEN-BOB-200",   DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(365)),
            ["CARD-999"] = new CardPayload("eve",   "TOKEN-EVE-999",   DateTime.UtcNow.AddDays(-1),  DateTime.UtcNow.AddDays(365))
        };

        public static bool TryRead(string cardId, out CardPayload? payload)
        {
            return Cards.TryGetValue(cardId ?? string.Empty, out payload);
        }
    }

    // Base abstract: Template Method pattern
    public abstract class AuthenticatorBase
    {
        // Template method: validações comuns -> delega o core
        public AuthResult Authenticate(AuthRequest request)
        {
            if (request == null) return AuthResult.Fail("Requisição nula");
            if (string.IsNullOrWhiteSpace(request.Username)) return AuthResult.Fail("Username ausente");

            // validações comuns (ex.: formato)
            var pre = PreValidate(request);
            if (!pre.Success) return pre;

            // execução específica da estratégia
            var core = AuthenticateCore(request);
            if (!core.Success) return core;

            // pós-processamento (logs, auditoria, etc) - aqui só uma simulação
            PostProcess(request, core);
            return core;
        }

        protected virtual AuthResult PreValidate(AuthRequest request)
        {
            // validações padrão - subclasses podem sobrescrever se necessário
            return AuthResult.Ok();
        }

        protected abstract AuthResult AuthenticateCore(AuthRequest request);

        protected virtual void PostProcess(AuthRequest request, AuthResult result)
        {
            // Em produção: gravar evento de auditoria, métricas, etc.
            // Aqui, apenas simulação (não imprime para não poluir lógica)
        }
    }

    // Autenticador físico por biometria
    public sealed class PhysicalBiometryAuthenticator : AuthenticatorBase
    {
        protected override AuthResult AuthenticateCore(AuthRequest request)
        {
            if (!FakeUserStore.TryGet(request.Username, out var user) || user == null)
                return AuthResult.Fail("Usuário não encontrado");

            if (!user.IsPhysicalAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação física");

            if (request.BiometricSample == null)
                return AuthResult.Fail("Amostra biométrica ausente");

            bool match = Helpers.BiometricMatches(request.BiometricSample, user.BiometricTemplate);
            return match ? AuthResult.Ok("Biometria física validada") : AuthResult.Fail("Biometria física inválida");
        }
    }

    // Autenticador físico por cartão
    public sealed class PhysicalCardAuthenticator : AuthenticatorBase
    {
        protected override AuthResult AuthenticateCore(AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CardId))
                return AuthResult.Fail("Cartão não apresentado (físico)");

            // lê o payload do cartão físico
            if (!FakeCardStore.TryRead(request.CardId!, out var payload) || payload == null)
                return AuthResult.Fail("Cartão desconhecido");

            if (payload.IsExpired())
                return AuthResult.Fail("Cartão expirado");

            // verifica se o cartão "declara" o usuário esperado
            if (!string.Equals(payload.RegisteredUsername, request.Username, StringComparison.OrdinalIgnoreCase))
                return AuthResult.Fail("Cartão não pertence ao usuário apresentado");

            // Também confirmamos que o usuário permite acesso físico
            if (!FakeUserStore.TryGet(request.Username, out var user) || user == null)
                return AuthResult.Fail("Usuário não encontrado");

            if (!user.IsPhysicalAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação física");

            // PASSOU: autenticação física por cartão ok
            return AuthResult.Ok("Cartão físico validado");
        }
    }

    // Autenticador online por biometria
    public sealed class OnlineBiometryAuthenticator : AuthenticatorBase
    {
        protected override AuthResult AuthenticateCore(AuthRequest request)
        {
            if (!FakeUserStore.TryGet(request.Username, out var user) || user == null)
                return AuthResult.Fail("Usuário não encontrado");

            if (!user.IsOnlineAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação online");

            if (request.BiometricSample == null)
                return AuthResult.Fail("Amostra biométrica ausente");

            if (Helpers.IsIpSuspicious(request.ClientIp))
                return AuthResult.Fail("Acesso de IP suspeito - bloqueado");

            bool match = Helpers.BiometricMatches(request.BiometricSample, user.BiometricTemplate);
            return match ? AuthResult.Ok("Biometria online validada") : AuthResult.Fail("Biometria online inválida");
        }
    }

    // Autenticador online por cartão — usa o cadastro lido do cartão como prova/credencial
    public sealed class OnlineCardAuthenticator : AuthenticatorBase
    {
        // Observação: aqui assumimos que o cliente enviou o cardId e um "CardAuthToken" (simulado via CardId apenas).
        // Em um sistema real, o cartão deveria executar um challenge-response e prover um token assinado.
        protected override AuthResult AuthenticateCore(AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CardId))
                return AuthResult.Fail("Cartão/token online não apresentado");

            // Lê o payload do cartão (o "cadastro" que está dentro do cartão)
            if (!FakeCardStore.TryRead(request.CardId!, out var payload) || payload == null)
                return AuthResult.Fail("Cartão/token desconhecido");

            if (payload.IsExpired())
                return AuthResult.Fail("Token do cartão expirado");

            // Verifica se o payload corresponde ao usuário informado
            if (!string.Equals(payload.RegisteredUsername, request.Username, StringComparison.OrdinalIgnoreCase))
                return AuthResult.Fail("Token do cartão não corresponde ao usuário");

            // Verificações adicionais de risco
            if (Helpers.IsIpSuspicious(request.ClientIp))
                return AuthResult.Fail("Acesso de IP suspeito - bloqueado");

            // Verifica se o usuário permite autenticação online
            if (!FakeUserStore.TryGet(request.Username, out var user) || user == null)
                return AuthResult.Fail("Usuário não encontrado");

            if (!user.IsOnlineAllowed)
                return AuthResult.Fail("Usuário sem permissão para autenticação online");

            // Simulação: conferimos se o token do cartão é "válido" (o servidor o conhece)
            if (!Helpers.IsServerKnownCardToken(payload.CardAuthToken))
                return AuthResult.Fail("Token do cartão inválido no servidor");

            // Autenticação online por cartão passou
            return AuthResult.Ok("Cartão online validado usando cadastro armazenado no cartão");
        }
    }

    // Helpers e políticas simplificadas
    public static class Helpers
    {
        public static bool BiometricMatches(byte[] sample, byte[] template)
        {
            if (sample == null || template == null) return false;
            if (sample.Length != template.Length) return false;
            for (int i = 0; i < sample.Length; i++)
                if (sample[i] != template[i]) return false;
            return true;
        }

        public static bool IsIpSuspicious(string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return false;
            var suspicious = new HashSet<string> { "10.0.0.66", "203.0.113.66" };
            return suspicious.Contains(ip);
        }

        public static bool IsServerKnownCardToken(string token)
        {
            // Simulação: servidor conhece tokens que começam com "TOKEN-"
            return token?.StartsWith("TOKEN-", StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }

    // Cliente que ainda compõe a concreta (ponto de composição local)
    public static class AuthClient
    {
        // Fábrica simples — na Fase 4 isto virará injeção/fábrica configurável
        public static AuthenticatorBase CreateAuthenticator(AuthMode mode) => mode switch
        {
            AuthMode.Physical_Biometry => new PhysicalBiometryAuthenticator(),
            AuthMode.Physical_Card    => new PhysicalCardAuthenticator(),
            AuthMode.Online_Biometry  => new OnlineBiometryAuthenticator(),
            AuthMode.Online_Card      => new OnlineCardAuthenticator(),
            _ => throw new ArgumentException("Modo inválido para criação de autenticador", nameof(mode))
        };

        public static AuthResult Authenticate(AuthRequest request)
        {
            var auth = CreateAuthenticator(request.Mode);
            return auth.Authenticate(request);
        }
    }

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== Fase 3 — OO sem interface (Autenticação) ===\n");

            var scenarios = new List<AuthRequest>
            {
                new AuthRequest { Username = "alice", BiometricSample = new byte[]{1,2,3,4}, Mode = AuthMode.Physical_Biometry },
                new AuthRequest { Username = "bob", CardId = "CARD-200", Mode = AuthMode.Physical_Card },
                new AuthRequest { Username = "alice", BiometricSample = new byte[]{1,2,3,4}, ClientIp = "203.0.113.5", Mode = AuthMode.Online_Biometry },
                new AuthRequest { Username = "eve", CardId = "CARD-999", ClientIp = "198.51.100.10", Mode = AuthMode.Online_Card },
                // cenário de mismatch (cartão não pertence ao usuário declarado)
                new AuthRequest { Username = "alice", CardId = "CARD-200", Mode = AuthMode.Online_Card }
            };

            foreach (var req in scenarios)
            {
                Console.WriteLine($"Cenário: user='{req.Username}', modo={req.Mode}, card='{req.CardId}', ip='{req.ClientIp}'");
                var result = AuthClient.Authenticate(req);
                Console.WriteLine($"Resultado: {(result.Success ? "OK" : "FAIL")} - {result.Message}\n");
            }

            Console.WriteLine("=== Fim dos cenários ===");
            Console.WriteLine("\nObservação: este código já separa o fluxo (Template Method) e extraiu cada opção para uma classe concreta.\nNa Fase 4 converta AuthenticatorBase para uma interface e injete implementações via DI/fábrica configurável.");
        }
    }
}
