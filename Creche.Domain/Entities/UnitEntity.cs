using Amazon.DynamoDBv2.DataModel;

namespace Creche.Application.Entities;

[DynamoDBTable("creche-unit")]
public class UnitEntity
{
    [DynamoDBHashKey]
    public string PK { get; set; }

    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Director { get; set; }

    public UnitEntity() {
        PK = Guid.NewGuid().ToString();
    }
}