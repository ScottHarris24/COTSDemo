using COTSDemo.Abstractions.Entities;
using static COTSDemo.Abstractions.Models.Order;

namespace COTSDemo.Abstractions.Models;

public class Customer : BaseModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string BillingAddress { get; set; } = null!;
    public string ShippingAddress { get; set; } = null!;

    public List<Order> Orders { get; set; } = new();

    public CustomerEntity ToEntity()
    {
        var customerEntity = ToEntity(this);
        return customerEntity;
    }
    public static CustomerEntity ToEntity(Customer customer)
    {
        var customerEntity = new CustomerEntity
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BillingAddress = customer.BillingAddress,
            ShippingAddress = customer.ShippingAddress,
            Orders = customer.Orders?
                .Select(o => o.ToEntity())
                .ToList()!
        };

        ToEntity(customerEntity, customer);
        return customerEntity;
    }

    public static Customer ToModel(CustomerEntity customerEntity)
    {
        var customer = new Customer
        {
            FirstName = customerEntity!.FirstName,
            LastName = customerEntity.LastName,
            BillingAddress = customerEntity.BillingAddress,
            ShippingAddress = customerEntity.ShippingAddress,
            Orders = customerEntity.Orders?
                .Select(Order.ToModel)
                .ToList()!
        };

        ToModel(customer, customerEntity);
        return customer;
    }

}