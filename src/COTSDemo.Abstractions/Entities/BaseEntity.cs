namespace COTSDemo.Abstractions.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime LastUpdated { get; set; }
    public string LastUpdatedBy { get; set; } = null!;
}