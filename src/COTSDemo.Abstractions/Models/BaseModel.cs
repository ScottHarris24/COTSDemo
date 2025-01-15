using COTSDemo.Abstractions.Entities;

namespace COTSDemo.Abstractions.Models;

public class BaseModel
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime LastUpdated { get; set; }
    public string LastUpdatedBy { get; set; } = null!;

    protected static void ToEntity(BaseEntity entity, BaseModel model)
    {
        entity.Id = model.Id;

        entity.Created = model.Created;
        entity.CreatedBy = model.CreatedBy;
        entity.LastUpdated = model.LastUpdated;
        entity.LastUpdatedBy = model.LastUpdatedBy;
    }

    protected static void ToModel(BaseModel model, BaseEntity entity)
    {
        model.Id = entity.Id;

        model.Created = entity.Created;
        model.CreatedBy = entity.CreatedBy;
        model.LastUpdated = entity.LastUpdated;
        model.LastUpdatedBy = entity.LastUpdatedBy;
    }
}