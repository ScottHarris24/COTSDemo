using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Interfaces;
using COTSDemo.Abstractions.Models;

namespace COTSDemo.Services;

public abstract class BaseRepositoryService
{
    protected IServiceResponse CreateServiceResponse(IRepositoryResponse repositoryResponse)
    {
        var serviceResponse = ServiceResponse.ToServiceResponse(repositoryResponse);
        serviceResponse.Data = (repositoryResponse.Data switch
        {
            IEnumerable<CustomerEntity> customers => customers.Select(Customer.ToModel).ToList(),
            CustomerEntity customer => Customer.ToModel(customer),
            _ => null
        })!;

        return serviceResponse;
    }
}
