using COTSDemo.Abstractions.Entities;
using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Interfaces;
using COTSDemo.Abstractions.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace COTSDemo.Services;

public class ServiceResponse : IServiceResponse
{
    public ServiceReturnStatusType ReturnStatus { get; set; }
    public int RecordsEffected { get; set; }
    public object Data { get; set; } = null!;
    public int ErrorCode { get; set; }
    public string ErrorTypeName { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;

    public static IServiceResponse ToServiceResponse(IRepositoryResponse repositoryResponse)
    {
        var serviceResponse = new ServiceResponse
        {
            RecordsEffected = repositoryResponse.RecordsEffected,
            ErrorCode = repositoryResponse.ErrorCode,
            ErrorTypeName = repositoryResponse.ErrorTypeName,
            ErrorMessage = repositoryResponse.ErrorMessage,
            ReturnStatus = repositoryResponse.ReturnStatus switch
            {
                RepositoryReturnStatusType.Success => ServiceReturnStatusType.Success,
                RepositoryReturnStatusType.NotFound => ServiceReturnStatusType.NotFound,
                RepositoryReturnStatusType.Exception => ServiceReturnStatusType.Exception,
                _ => ServiceReturnStatusType.Error
            },
        };

        return serviceResponse;
    }
}