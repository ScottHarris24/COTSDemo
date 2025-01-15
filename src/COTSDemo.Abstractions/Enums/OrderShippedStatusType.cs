namespace COTSDemo.Abstractions.Enums;

public enum OrderShippedStatusType
{
    /// <summary>
    ///    Get all orders 
    /// </summary>
    All = 1,

    /// <summary>
    ///  Do not get any orders
    /// </summary>
    None = 2,

    /// <summary>
    ///     Get only shipped orders
    /// </summary>
    Shipped = 3 ,

    /// <summary>
    ///    Get only not shipped orders
    /// </summary>
    NotShipped = 4
}