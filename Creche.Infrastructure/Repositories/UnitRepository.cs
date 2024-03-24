using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Creche.Application.Entities;
using Creche.Infrastructure.Interfaces;

namespace Creche.Infrastructure.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly DynamoDBContext _context;

    public UnitRepository(
        IAmazonDynamoDB dynamoDbClient
    )
    {
        _context = new DynamoDBContext(dynamoDbClient);
    }

    public async Task CreateUnitAsync(UnitEntity unit)
    {
        await _context.SaveAsync(unit);
    }

    public async Task<UnitEntity> GetUnitByIdAsync(string unitId)
    {
        try
        {
            return await _context.LoadAsync<UnitEntity>(unitId);
        }
        catch (AmazonDynamoDBException _)
        {
            throw;
        }
        catch (Exception _)
        {
            throw;
        }
    }
}