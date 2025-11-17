using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementação simples de Repository em memória,
/// usando Dictionary como "banco" e um delegate para extrair o Id.
/// </summary>
public sealed class InMemoryRepository<T, TId> : IRepository<T, TId>
    where TId : notnull
{
    private readonly Dictionary<TId, T> _store = new();
    private readonly Func<T, TId> _getId;

    public InMemoryRepository(Func<T, TId> getId)
    {
        _getId = getId ?? throw new ArgumentNullException(nameof(getId));
    }

    public T Add(T entity)
    {
        var id = _getId(entity);
        _store[id] = entity;
        return entity;
    }

    public T? GetById(TId id)
    {
        return _store.TryGetValue(id, out var entity) ? entity : default;
    }

    public IReadOnlyList<T> ListAll()
    {
        // ToList() devolve uma cópia, evitando expor coleção mutável
        return _store.Values.ToList();
    }

    public bool Update(T entity)
    {
        var id = _getId(entity);

        if (!_store.ContainsKey(id))
            return false;

        _store[id] = entity;
        return true;
    }

    public bool Remove(TId id)
    {
        return _store.Remove(id);
    }
}
