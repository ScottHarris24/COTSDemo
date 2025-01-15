using System.Linq.Expressions;
using COTSDemo.Abstractions.Enums;

namespace COTSDemo.Abstractions.Interfaces;

public interface ICustomerRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Get the customer record for the given id along with related orders and order details
    /// </summary>
    Task<IRepositoryResponse> GetCustomerAsync (int id, OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.None, OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.None, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Gets customer items along with the related orders and order details based on the given lambda expression
    /// </summary>
    Task<IRepositoryResponse> QueryCustomersAsync(Expression<Func<TEntity, bool>> predicate, OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.None, OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.None, CancellationToken cancellationToken = default(CancellationToken));
}


