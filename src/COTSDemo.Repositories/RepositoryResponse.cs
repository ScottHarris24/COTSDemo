using COTSDemo.Abstractions.Enums;
using COTSDemo.Abstractions.Interfaces;

namespace COTSDemo.Repositories;

public class RepositoryResponse : IRepositoryResponse
{
    public RepositoryReturnStatusType ReturnStatus { get; set; }
    public int RecordsEffected { get; set; }
    public object Data { get; set; } = null!;
    public int ErrorCode { get; set; }
    public string ErrorTypeName { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;
}