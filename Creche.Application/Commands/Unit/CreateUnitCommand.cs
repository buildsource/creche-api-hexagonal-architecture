using Creche.Application.DTOs;
using Creche.Application.Responses;
using MediatR;

namespace Creche.Application.Commands.Unit;

public class CreateUnitCommand : IRequest<UnitResponse>
{
    public UnitDto UnitDto { get; set; }

    public CreateUnitCommand(UnitDto unitDto)
    {
        UnitDto = unitDto;
    }
}