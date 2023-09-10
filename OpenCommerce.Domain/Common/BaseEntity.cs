using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OpenCommerce.Domain.Common;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; } = null!;
    public DateTimeOffset? createdDate { get; set; }
    public string? createdBy { get; set; }
    public string? createdByName { get; set; }
    public DateTimeOffset? latestUpdatedDate { get; set; }
    public string? latestUpdatedBy { get; set; }
    public string? latestUpdatedByName { get; set; }
    public bool isDeleted { get; set; }
    public DateTimeOffset? deletedDate { get; set; }
}