using Amazon.DynamoDBv2.DataModel;

namespace Creche.Application.Entities;

[DynamoDBTable("Funcionario")]
public class Funcionario
{
    [DynamoDBHashKey]
    public string PK { get; set; }

    [DynamoDBRangeKey]
    public string SK { get; set; }

    public string Nome { get; set; }
    public string Cargo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
}