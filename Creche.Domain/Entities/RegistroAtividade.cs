using Amazon.DynamoDBv2.DataModel;

namespace Creche.Application.Entities;

[DynamoDBTable("CrecheTable")]
public class RegistroAtividade
{
    [DynamoDBHashKey]
    public string PK { get; set; } // Ex: "ATIVIDADE#id"

    [DynamoDBRangeKey]
    public string SK { get; set; } // Ex: "CRIANCA#id"

    public string Observacoes { get; set; }
}