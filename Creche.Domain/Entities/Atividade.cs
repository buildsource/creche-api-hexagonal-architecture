using Amazon.DynamoDBv2.DataModel;

namespace Creche.Application.Entities;

[DynamoDBTable("Atividade")]
public class Atividade
{
    [DynamoDBHashKey]
    public string PK { get; set; }

    [DynamoDBRangeKey]
    public string SK { get; set; }

    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string DataHoraInicio { get; set; } // Use string para datas
    public string DataHoraFim { get; set; } // Use string para datas
}