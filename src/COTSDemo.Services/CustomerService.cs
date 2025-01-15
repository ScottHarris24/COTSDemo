using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Interfaces;
using COTSDemo.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace COTSDemo.Services;

public class CustomerService(IServiceProvider serviceProvider) : BaseRepositoryService, ICustomerService
{
    private readonly ICustomerRepository<CustomerEntity> _customerRepository = serviceProvider.GetRequiredService<ICustomerRepository<CustomerEntity>>();
    public async Task<IServiceResponse> GetCustomerAsync(int id, 
        OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.All,
        OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.All,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var repositoryResponse = await _customerRepository
            .GetCustomerAsync(id, orderShippedStatus, orderDetailStatus, cancellationToken)
            .ConfigureAwait(false);

        var serviceResponse = CreateServiceResponse(repositoryResponse);

        return serviceResponse;
    }

    public async Task<IServiceResponse> QueryCustomersAsync(Expression<Func<Customer, bool>> predicate, 
        OrderShippedStatusType orderShippedStatus = OrderShippedStatusType.All,
        OrderDetailStatusType orderDetailStatus = OrderDetailStatusType.All,
        CancellationToken cancellationToken = default(CancellationToken))
    {

        var where = predicate.ReplaceLambdaParameter<Customer, CustomerEntity>();
        var repositoryResponse = await _customerRepository
                .QueryCustomersAsync(where, orderShippedStatus, orderDetailStatus, cancellationToken)
                .ConfigureAwait(false);

        var serviceResponse = CreateServiceResponse(repositoryResponse);

        return serviceResponse;
    }

    public async Task<IServiceResponse> AddAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken))
    {
        var customerEntity = customer.ToEntity();
        var repositoryResponse = await _customerRepository
            .AddAsync(customerEntity, cancellationToken)
            .ConfigureAwait(false);

        var serviceResponse = CreateServiceResponse(repositoryResponse);
        return serviceResponse;
    }

    public async Task<IServiceResponse> AddRangeAsync(IEnumerable<Customer> customers, CancellationToken cancellationToken = default(CancellationToken))
    {
        var customerEntities = customers.Select(Customer.ToEntity).ToList();
        var repositoryResponse = await _customerRepository
            .AddRangeAsync(customerEntities, cancellationToken)
            .ConfigureAwait(false);

        var serviceResponse = CreateServiceResponse(repositoryResponse);
        return serviceResponse;
    }

    public async Task<IServiceResponse> DeleteAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken))
    {
        var customerEntity = customer.ToEntity();
        var repositoryResponse = await _customerRepository
            .DeleteAsync(customerEntity, cancellationToken)
            .ConfigureAwait(false);

        var serviceResponse = CreateServiceResponse(repositoryResponse);
        return serviceResponse;
    }

    public async  Task<IServiceResponse> DeleteRangeAsync(IEnumerable<Customer> customers, CancellationToken cancellationToken = default(CancellationToken))
    {
        var customerEntities = customers.Select(Customer.ToEntity).ToList();
        var repositoryResponse = await _customerRepository
            .DeleteRangeAsync(customerEntities, cancellationToken)
            .ConfigureAwait(false);

        var serviceResponse = CreateServiceResponse(repositoryResponse);
        return serviceResponse;
    }
}