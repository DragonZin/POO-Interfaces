using System.Collections.Generic;

public interface IRepository<T, TId>
{
    /// <summary>
    /// Adiciona uma entidade ao repositório.
    /// </summary>
    T Add(T entity);

    /// <summary>
    /// Busca uma entidade pelo identificador.
    /// Retorna null (ou default) se não encontrar.
    /// </summary>
    T? GetById(TId id);

    /// <summary>
    /// Lista todas as entidades.
    /// Retorna uma coleção somente leitura para não expor coleções mutáveis.
    /// </summary>
    IReadOnlyList<T> ListAll();

    /// <summary>
    /// Atualiza uma entidade existente.
    /// Retorna true se conseguiu atualizar, false se o id não existir.
    /// </summary>
    bool Update(T entity);

    /// <summary>
    /// Remove uma entidade pelo id.
    /// Retorna true se removeu, false se não encontrou.
    /// </summary>
    bool Remove(TId id);
}
