using System.ComponentModel.DataAnnotations.Schema;

namespace COTSDemo.Abstractions.Entities;

public class OrderEntity : BaseEntity
{
    [ForeignKey("CustomerEntity")]
    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;

    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }

    public ICollection<OrderDetailEntity> OrderDetails { get; set; } = null!;
}