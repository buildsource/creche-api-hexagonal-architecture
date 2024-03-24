using Creche.Application.Entities;

namespace Creche.Infrastructure.Interfaces;

public interface IUnitRepository
{
    Task CreateUnitAsync(UnitEntity unit);
    Task<UnitEntity> GetUnitByIdAsync(string unitId);
}