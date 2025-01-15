using System.ComponentModel.DataAnnotations.Schema;
using COTSDemo.Abstractions.Enums;

namespace COTSDemo.Abstractions.Entities;

public class OrderDetailEntity : BaseEntity
{
    [ForeignKey("OrderId")]
    public int OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;

    public OrderDetailStatusType OrderDetailStatus { get; set; } = OrderDetailStatusType.InProgress;
    public int Quantity { get; set; }
    public int ProductId { get; set; }

}