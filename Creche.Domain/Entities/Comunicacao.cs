using Amazon.DynamoDBv2.DataModel;

namespace Creche.Application.Entities;

[DynamoDBTable("Comunicacao")]
public class Comunicacao
{
    [DynamoDBHashKey]
    public string PK { get; set; } // Ex: "CRECHE#id"

    [DynamoDBRangeKey]
    public string SK { get; set; } // Ex: "COMUNICACAO#timestamp"

    public string Titulo { get; set; }
    public string Mensagem { get; set; }
    public string DataHoraEnvio { get; set; } // Use string para datas
}