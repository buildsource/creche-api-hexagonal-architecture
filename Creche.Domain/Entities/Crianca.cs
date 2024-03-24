using Amazon.DynamoDBv2.DataModel;

namespace Creche.Application.Entities;

[DynamoDBTable("Crianca")]
public class Crianca
{
    [DynamoDBHashKey]
    public string PK { get; set; }

    [DynamoDBRangeKey]
    public string SK { get; set; }

    public string Nome { get; set; }
    public string DataNascimento { get; set; } // DynamoDB não tem tipo date, use string ou timestamp
    public string InformacoesAdicionais { get; set; }
}