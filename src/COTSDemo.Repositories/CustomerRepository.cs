using System.Linq.Expressions;
using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Interfaces;
using COTSDemo.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace COTSDemo.Repositories;

public class CustomerRepository(IServiceProvider serviceProvider) : Repository<CustomerEntity>(serviceProvider), ICustomerRepository<CustomerEntity>
{
    #region Protected Properties

    public async Task<IRepositoryResponse> GetCustomerAsync(int id, OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.All, OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.All, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var customer = await _dbContext
                .Customers
                .FindAsync(id, cancellationToken)
                .ConfigureAwait(false);

            if (customer != null)
            {
                await LoadOrders(customer, orderShippedStatus, orderDetailStatus, cancellationToken);
            }

            response = new RepositoryResponse()
            {
                ReturnStatus = customer == null
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                Data = customer!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    public async Task<IRepositoryResponse> QueryCustomersAsync(Expression<Func<CustomerEntity, bool>> predicate, OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.All,  OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.All, CancellationToken cancellationToken = default(CancellationToken))
    {
        IRepositoryResponse? response;

        try
        {
            var customers = await _dbContext
                .Customers
                .Where(predicate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            await LoadOrders(customers, orderShippedStatus, orderDetailStatus, cancellationToken);

            response = new RepositoryResponse()
            {
                ReturnStatus = customers.Count == 0
                    ? RepositoryReturnStatusType.NotFound
                    : RepositoryReturnStatusType.Success,

                Data = customers!,
            };
        }
        catch (Exception e)
        {
            response = CreateRepositoryResponse(e);
        }

        return response;
    }

    #endregion Protected Properties

    #region Private Support Functions

    private async Task LoadOrders(IEnumerable<CustomerEntity> customers, OrderShippedStatusType orderShippedStatus, OrderDetailStatusType orderDetailStatus, CancellationToken cancellationToken)
    {
        if (orderShippedStatus == OrderShippedStatusType.None)
        {
            return;
        }

        foreach (var customer in customers)
        {
            await LoadOrders(customer, orderShippedStatus, orderDetailStatus, cancellationToken);
        }

    }

    private async Task LoadOrders(CustomerEntity? customer, OrderShippedStatusType orderShippedStatus, OrderDetailStatusType orderDetailStatus, CancellationToken cancellationToken)
    {
        if (customer == null | orderShippedStatus == OrderShippedStatusType.None )
        {
            return;
        }

        await _dbContext
            .Orders
            .Where(x => x.CustomerId == customer!.Id &&
                        (
                            orderShippedStatus == OrderShippedStatusType.All ||
                            (orderShippedStatus == OrderShippedStatusType.Shipped
                                ? x.ShippedDate != null
                                : x.ShippedDate == null)
                        )
            )
            .LoadAsync(cancellationToken);


        await LoadOrderDetails(customer!.Orders, orderDetailStatus, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task LoadOrderDetails(ICollection<OrderEntity> orders, OrderDetailStatusType orderDetailStatus, CancellationToken cancellationToken)
    {
        if (orderDetailStatus == OrderDetailStatusType.None)
        {
            return;
        }

        foreach (var order in orders)
        {
            await _dbContext
                .OrderDetails
                .Where(x => x.OrderId == order.Id &&
                            (
                                orderDetailStatus == OrderDetailStatusType.All
                                    ? x.OrderDetailStatus != OrderDetailStatusType.All
                                    : x.OrderDetailStatus == orderDetailStatus
                            )
                )
                .LoadAsync(cancellationToken);
        }
    }

    #endregion Private Support Functions
}