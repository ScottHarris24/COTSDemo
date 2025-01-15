namespace COTSDemo.Abstractions.Entities;

public class ProductEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int QuantityOnHand { get; set; }
}