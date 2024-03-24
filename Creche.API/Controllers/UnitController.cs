using Creche.Application.Commands.Unit;
using Creche.Application.DTOs;
using Creche.Application.Exceptions;
using Creche.Application.Queries.Unit;
using Creche.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Creche.Controllers;

[Route("api/[controller]")]
public class UnitController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UnitResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnit([FromBody] UnitDto unitDto)
    {
        try
        {
            var command = new CreateUnitCommand(unitDto);
            var unit = await _mediator.Send(command);

            return Ok(new ApiResponse<UnitResponse>(unit, "Unit created successfully"));
        }
        catch (ValidationException vex)
        {
            return BadRequest(new ApiResponse<List<string>>(vex));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<List<string>>("An error occurred when creating the unit", new List<string> { ex.Message }));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UnitResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUnitById(string id)
    {
        try
        {
            var query = new GetUnitByIdQuery(id);
            var unidade = await _mediator.Send(query);

            if (unidade == null)
                return NotFound(new ApiResponse<List<string>>("Unit not found", new List<string>()));

            return Ok(new ApiResponse<UnitResponse>(unidade, "Unit obtained successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<List<string>>("An error occurred when obtaining the unit", new List<string> { ex.Message }));
        }
    }
}