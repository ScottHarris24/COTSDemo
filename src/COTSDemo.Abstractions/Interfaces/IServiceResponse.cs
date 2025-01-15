﻿using COTSDemo.Abstractions.Enums;

namespace COTSDemo.Abstractions.Interfaces;

/// <summary>
///     Returned from all service function calls to ensure consistency
///     in processing service data and functions
/// </summary>
/// <notes>
///     Returning a consistent object allows the calling function to check the status of the call
///     and take proper actions for different types of situations.  For example the ReturnStatus
///     can indicate if an exception occured, if a record was not found, etc.
/// </notes>
public interface IServiceResponse
{
    /// <summary>
    ///     Enum type indicating the status of the response from the repository call
    /// </summary>
    ServiceReturnStatusType ReturnStatus { get; set; }

    /// <summary>
    ///     Number of records that were effects from an insert, update, etc.
    /// </summary>
    int RecordsEffected { get; set; }

    /// <summary>
    ///     Data returned from repository.  This can be an object, value
    ///     or enumerated list of objects
    /// </summary>
    object Data { get; set; }

    /// <summary>
    ///     Internal error code usually returned from exceptions
    /// </summary>
    int ErrorCode { get; set; }

    /// <summary>
    ///     Type name of exception if an exception was caught by the repository code
    /// </summary>
    string ErrorTypeName { get; set; }

    /// <summary>
    ///     Message can be exception message or a message generated by the function that was called
    /// </summary>
    string ErrorMessage { get; set; }

    /// <summary>
    ///    Convert a repository response to a service response
    /// </summary>
    //IServiceResponse ToServiceResponse(IRepositoryResponse repositoryResponse, Func<object, object> toModel)

}