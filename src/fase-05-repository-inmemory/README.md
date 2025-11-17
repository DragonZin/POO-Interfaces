# Fase 5 — Repository InMemory (SecureGate)

## Visão geral

Nesta fase introduzimos o padrão **Repository** como ponto único de acesso aos dados
de eventos de acesso do SecureGate (`AccessEvent`), usando uma implementação
InMemory baseada em coleção (`InMemoryRepository`).

O cliente (serviço / demo) fala **apenas** com o `IRepository`, nunca com a
coleção diretamente.

## Diagrama

Cliente (AccessEventService / Fase05Demo)
        ↓
  IRepository<AccessEvent, int>
        ↓
InMemoryRepository<AccessEvent, int>
        ↓
  Dictionary<int, AccessEvent> (coleção in-memory)

## Arquivos principais

- `IRepository.cs` – contrato genérico de acesso a dados
- `InMemoryRepository.cs` – implementação InMemory baseada em `Dictionary`
- `AccessEvent.cs` – modelo de domínio (evento de acesso)
- `AccessEventService.cs` – serviço de domínio que depende apenas do Repository
- `Fase05Demo.cs` – exemplo de uso em código de cliente

## Testes

Arquivo de testes (em projeto de testes):

- `InMemoryRepositoryTests.cs`

Cobrem:

- Add + ListAll
- GetById existente e ausente
- Update existente e ausente
- Remove existente e ausente

## Como rodar os testes

Na raiz da solução (onde estiver o `.sln`):

```bash
dotnet test
