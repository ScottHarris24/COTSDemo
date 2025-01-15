using System.Linq.Expressions;

namespace COTSDemo.Abstractions.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Gets the top level item for the given id
    /// </summary>
    Task<IRepositoryResponse> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Gets all top level the items 
    /// </summary>
    Task<IRepositoryResponse> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Gets the top level items based on the given lambda expression
    /// </summary>
    Task<IRepositoryResponse> QueryAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Adds the given item and any child items to the database
    /// </summary>
    Task<IRepositoryResponse> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Adds multiple items and any child items to the database
    /// </summary>
    Task<IRepositoryResponse> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Deletes the given item and any child items from the database
    /// </summary>
    Task<IRepositoryResponse> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Deletes the given items and any child items from the database
    /// </summary>
    Task<IRepositoryResponse> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
}