## 🚀 Fase 4 — Interface plugável e testável [↗](src/fase-04-com-interfaces)

### ✅ Objetivo da Fase
- Enunciado: Defina um contrato claro e refatore o cliente para depender dele.
- Descrição: Explique como alternar implementações sem mudar o cliente e como dobrar a
dependência em testes (injeção simples).

---

## Controle e autenticação de acesso físico e digital

### 1) Contrato (interface)

```csharp
public interface IAuthenticator
{
    bool Authenticate(string identity, string? secret = null);
}
```

### 2) Implementações

```csharp
public sealed class BiometricAuthenticator : IAuthenticator
{
    public bool Authenticate(string identity, string? secret = null)
        => !string.IsNullOrWhiteSpace(identity) && identity.StartsWith("bio-");
}

public sealed class RfidPasswordAuthenticator : IAuthenticator
{
    public bool Authenticate(string identity, string? secret = null)
        => identity.StartsWith("card-") && secret == "1234";
}
```

### 3) Cliente

```csharp
public class AccessController
{
    private readonly IAuthenticator _auth;
    public AccessController(IAuthenticator auth) => _auth = auth;

    public string RequestAccess(string identity, string? secret = null)
        => _auth.Authenticate(identity, secret) ? "Acesso liberado" : "Acesso negado";
}
```

### 4) Fábrica

```csharp
public static class AuthenticatorFactory
{
    public static IAuthenticator Resolve(string mode) =>
        mode switch {
            "biometria" => new BiometricAuthenticator(),
            "cartao-senha" => new RfidPasswordAuthenticator(),
            _ => throw new ArgumentException()
        };
}
```

### 5) Testes com dublê

```csharp
public sealed class FakeAuthenticator : IAuthenticator
{
    private readonly Func<string,string?,bool> _fn;
    public FakeAuthenticator(Func<string,string?,bool> fn)=>_fn=fn;
    public bool Authenticate(string i,string? s=null)=>_fn(i,s);
}
```

```csharp
public class AccessControllerTests
{
    [Fact]
    public void TestFake()
    {
        var fake = new FakeAuthenticator((i,s)=>i=="valid");
        var c = new AccessController(fake);
        Assert.Equal("Acesso liberado", c.RequestAccess("valid"));
        Assert.Equal("Acesso negado", c.RequestAccess("x"));
    }
}
```

---

## Monitoramento de acessos e auditoria de segurança

### 1) Contrato

```csharp
public interface IAccessLogger
{
    void LogAccess(string identity, DateTime when, string method, bool success);
}
```

### 2) Implementações

```csharp
public sealed class BiometricAccessLogger : IAccessLogger
{
    public void LogAccess(string identity, DateTime when, string method, bool success)
    {
    }
}

public sealed class CredentialAccessLogger : IAccessLogger
{
    public void LogAccess(string identity, DateTime when, string method, bool success)
    {
    }
}
```

### 3) Cliente

```csharp
public class AuditService
{
    private readonly IAccessLogger _logger;
    public AuditService(IAccessLogger logger)=>_logger=logger;

    public void RegisterAccess(string identity,string method,bool success)
        => _logger.LogAccess(identity, DateTime.UtcNow, method, success);
}
```

### 4) Fábrica

```csharp
public static class AccessLoggerFactory
{
    public static IAccessLogger Resolve(string mode)=>
        mode switch {
            "logs-biometricos"=>new BiometricAccessLogger(),
            "logs-credenciais"=>new CredentialAccessLogger(),
            _=>throw new ArgumentException()
        };
}
```

### 5) Teste com dublê

```csharp
public sealed class FakeLogger : IAccessLogger
{
    public List<(string id,DateTime w,string m,bool s)> Calls = new();
    public void LogAccess(string id,DateTime w,string m,bool s)
        => Calls.Add((id,w,m,s));
}
```

```csharp
public class AuditServiceTests
{
    [Fact]
    public void Logs()
    {
        var fake = new FakeLogger();
        var svc = new AuditService(fake);
        svc.RegisterAccess("u","bio",true);
        Assert.Single(fake.Calls);
    }
}
```
