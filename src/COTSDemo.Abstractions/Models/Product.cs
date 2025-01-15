using COTSDemo.Abstractions.Entities;

namespace COTSDemo.Abstractions.Models;

public class Product : BaseModel
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int QuantityOnHand { get; set; }

    public ProductEntity ToEntity()
    {
        var productEntity = ToEntity(this);
        return productEntity;
    }

    public static ProductEntity ToEntity(Product product)
    {
        var productEntity = new ProductEntity
        {
            Name = product.Name,
            Price = product.Price,
            QuantityOnHand = product.QuantityOnHand,
        };

        ToEntity(productEntity, product);
        return productEntity;
    }

    public Product ToModel(ProductEntity productEntity)
    {
        var product = new Product
        {
            Name = productEntity.Name,
            Price = productEntity.Price,
            QuantityOnHand = productEntity.QuantityOnHand,
        };

        ToModel(product, productEntity);
        return product;
    }
}