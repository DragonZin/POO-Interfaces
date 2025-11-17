using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Serviço de domínio para trabalhar com eventos de acesso,
/// dependendo APENAS do contrato IRepository.
/// </summary>
public static class AccessEventService
{
    public static AccessEvent RegisterEvent(
        IRepository<AccessEvent, int> repository,
        AccessEvent accessEvent)
    {
        // Aqui poderiam entrar validações de domínio:
        // - BadgeId obrigatório
        // - AreaName obrigatório
        // - etc.
        if (string.IsNullOrWhiteSpace(accessEvent.BadgeId))
            throw new ArgumentException("BadgeId é obrigatório.", nameof(accessEvent));

        if (string.IsNullOrWhiteSpace(accessEvent.AreaName))
            throw new ArgumentException("AreaName é obrigatório.", nameof(accessEvent));

        return repository.Add(accessEvent);
    }

    public static IReadOnlyList<AccessEvent> ListAll(
        IRepository<AccessEvent, int> repository)
    {
        return repository.ListAll();
    }

    public static IReadOnlyList<AccessEvent> ListByBadge(
        IRepository<AccessEvent, int> repository,
        string badgeId)
    {
        return repository
            .ListAll()
            .Where(e => string.Equals(e.BadgeId, badgeId, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static bool UpdateEvent(
        IRepository<AccessEvent, int> repository,
        AccessEvent updatedEvent)
    {
        return repository.Update(updatedEvent);
    }

    public static bool RemoveEvent(
        IRepository<AccessEvent, int> repository,
        int id)
    {
        return repository.Remove(id);
    }
}
