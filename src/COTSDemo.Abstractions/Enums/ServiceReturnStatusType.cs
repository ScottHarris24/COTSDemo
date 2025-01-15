namespace COTSDemo.Abstractions.Enums;

public enum ServiceReturnStatusType
{
    /// <summary>
    ///     The process was successful
    /// </summary>
    Success = 0,

    /// <summary>
    ///     The id or search criteria returned no entity or list 
    /// </summary>
    NotFound = 1,

    /// <summary>
    ///     An exception was captured during processing
    /// </summary>
    Exception = 2,

    /// <summary>
    ///    A non exception error was captured during processing
    /// </summary>
    Error = 3,

}