namespace COTSDemo.Abstractions.Enums;

public enum OrderDetailStatusType
{
    /// <summary>
    ///   Get all order details
    /// </summary>
    All = 1,

    /// <summary>
    ///     Do not get any order details
    /// </summary>
    None = 2,

    /// <summary>
    ///    Get only in progress order details
    /// </summary>
    InProgress = 3,

    /// <summary>
    ///   Get only fulfilled order details
    /// </summary>
    Fulfilled = 4,

    /// <summary>
    ///   Get only backorder order details
    /// </summary>
    Backorder = 5,
}