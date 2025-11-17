using System;

public static class Fase05Demo
{
    public static void Run()
    {
        // Fábrica simples do Repository para AccessEvent
        IRepository<AccessEvent, int> repo =
            new InMemoryRepository<AccessEvent, int>(e => e.Id);

        // Registrando alguns eventos de acesso
        AccessEventService.RegisterEvent(
            repo,
            new AccessEvent(
                Id: 1,
                BadgeId: "BADGE-001",
                AreaName: "Sala Servidores",
                Timestamp: DateTimeOffset.Now,
                AccessGranted: true
            )
        );

        AccessEventService.RegisterEvent(
            repo,
            new AccessEvent(
                Id: 2,
                BadgeId: "BADGE-002",
                AreaName: "Recepção",
                Timestamp: DateTimeOffset.Now,
                AccessGranted: false
            )
        );

        // Listando tudo
        var allEvents = AccessEventService.ListAll(repo);

        Console.WriteLine("=== Fase 5 - Repository InMemory (SecureGate) ===");
        Console.WriteLine("Eventos cadastrados:");

        foreach (var e in allEvents)
        {
            Console.WriteLine(
                $"#{e.Id} - Badge: {e.BadgeId} | Área: {e.AreaName} | " +
                $"Concedido: {e.AccessGranted} | Data: {e.Timestamp:dd/MM/yyyy HH:mm:ss}"
            );
        }

        // Exemplo de filtro por badge
        Console.WriteLine();
        Console.WriteLine("Eventos do BADGE-001:");

        var badgeEvents = AccessEventService.ListByBadge(repo, "BADGE-001");

        foreach (var e in badgeEvents)
        {
            Console.WriteLine(
                $"#{e.Id} - Área: {e.AreaName} | Concedido: {e.AccessGranted}"
            );
        }
    }
}
