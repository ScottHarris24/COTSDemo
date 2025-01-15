using COTSDemo.Abstractions.Entities;

namespace COTSDemo.Abstractions.Models;

public class Order : BaseModel
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = new();


    public OrderEntity ToEntity()
    {
        var orderEntity = ToEntity(this);
        return orderEntity;
    }

    public static OrderEntity ToEntity(Order order)
    {
        var orderEntity = new OrderEntity
        {
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            ShippedDate = order.ShippedDate,
            OrderDetails = order.OrderDetails?
                .Select(x => x.ToEntity())
                .ToList()!
        };

        ToEntity(orderEntity, order);
        return orderEntity;
    }

    public static Order ToModel(OrderEntity orderEntity)
    {
        var order = new Order
        {
            CustomerId = orderEntity!.CustomerId,
            OrderDate = orderEntity.OrderDate,
            ShippedDate = orderEntity.ShippedDate,
            OrderDetails = orderEntity.OrderDetails?
                .Select(OrderDetail.ToModel)
                .ToList()!
        };

        ToModel(order, orderEntity);
        return order;
    }

}