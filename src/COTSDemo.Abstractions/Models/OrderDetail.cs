using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;

namespace COTSDemo.Abstractions.Models;

public class OrderDetail : BaseModel
{
    public int OrderId { get; set; }
    public OrderDetailStatusType OrderDetailStatus { get; set; } = OrderDetailStatusType.InProgress;
    public int Quantity { get; set; }
    public int ProductId { get; set; }

    public OrderDetailEntity ToEntity()
    {
        var orderDetailEntity = ToEntity(this);
        return orderDetailEntity;
    }

    public static OrderDetailEntity ToEntity(OrderDetail orderDetail)
    {
        var orderDetailEntity = new OrderDetailEntity
        {
            OrderId = orderDetail.OrderId,
            OrderDetailStatus = orderDetail.OrderDetailStatus,
            Quantity = orderDetail.Quantity,
            ProductId = orderDetail.ProductId,
        };

        ToEntity(orderDetailEntity, orderDetail);
        return orderDetailEntity;
    }

    public static OrderDetail ToModel(OrderDetailEntity orderDetailEntity)
    {
        var orderDetail = new OrderDetail
        {
            OrderId = orderDetailEntity!.OrderId,
            OrderDetailStatus = orderDetailEntity.OrderDetailStatus,
            Quantity = orderDetailEntity.Quantity,
            ProductId = orderDetailEntity.ProductId,
        };

        ToModel(orderDetail, orderDetailEntity);
        return orderDetail;
    }

}