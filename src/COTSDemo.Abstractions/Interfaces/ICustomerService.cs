using System.Linq.Expressions;
using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Models;

namespace COTSDemo.Abstractions.Interfaces;

public interface ICustomerService
{
    /// <summary>
    ///     Get the customer record for the given id along with related orders and order details
    /// </summary>
    Task<IServiceResponse> GetCustomerAsync(int id, OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.None, OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.None,  CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Gets customer items along with the related orders and order details based on the given lambda expression
    /// </summary>
    Task<IServiceResponse> QueryCustomersAsync(Expression<Func<Customer, bool>> predicate, OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.None, OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.None, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Adds the given item and any child items to the database
    /// </summary>
    Task<IServiceResponse> AddAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Adds multiple items and any child items to the database
    /// </summary>
    Task<IServiceResponse> AddRangeAsync(IEnumerable<Customer> customers, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Deletes the given item and any child items from the database
    /// </summary>
    Task<IServiceResponse> DeleteAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Deletes the given items and any child items from the database
    /// </summary>
    Task<IServiceResponse> DeleteRangeAsync(IEnumerable<Customer> customers, CancellationToken cancellationToken = default(CancellationToken));
}