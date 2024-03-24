using AutoMapper;
using Creche.Application.Queries.Unit;
using Creche.Application.Responses;
using Creche.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Creche.Application.Handlers.Unit;

public class GetUnitByIdCommandHandler : IRequestHandler<GetUnitByIdQuery, UnitResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitRepository _unitRepository;
    private readonly IDistributedCache _cache;


    public GetUnitByIdCommandHandler(
        IMapper mapper,
        IUnitRepository unitRepository,
        IDistributedCache cache
    )
    {
        _mapper = mapper;
        _unitRepository = unitRepository;
        _cache = cache;
    }

    public async Task<UnitResponse> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unitId = request.UnitId;

        var unitData = await _cache.GetStringAsync(unitId);

        if (!string.IsNullOrEmpty(unitData))
            return JsonSerializer.Deserialize<UnitResponse>(unitData);

        var unit = await _unitRepository.GetUnitByIdAsync(unitId);

        if (unit == null)
            return new UnitResponse();

        var unitDto = _mapper.Map<UnitResponse>(unit);

        var serializedunit = JsonSerializer.Serialize(unitDto);

        await _cache.SetStringAsync(unitId, serializedunit, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });

        return unitDto;
    }
}