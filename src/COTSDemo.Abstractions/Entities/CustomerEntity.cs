namespace COTSDemo.Abstractions.Entities;

public class CustomerEntity : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string BillingAddress { get; set; } = null!;
    public string ShippingAddress { get; set; } = null!;

    public ICollection<OrderEntity> Orders { get; set; } = null!;
}