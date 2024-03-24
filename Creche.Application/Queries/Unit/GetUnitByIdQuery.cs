using Creche.Application.Responses;
using MediatR;

namespace Creche.Application.Queries.Unit;

public class GetUnitByIdQuery : IRequest<UnitResponse>
{
    public string UnitId { get; }

    public GetUnitByIdQuery(string unitId)
    {
        UnitId = unitId;
    }
}