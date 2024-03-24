using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Creche.Application.Commands.Unit;
using Creche.Application.Entities;
using Creche.Application.Exceptions;
using Creche.Application.Responses;
using Creche.Application.Validators;
using Creche.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Creche.Application.Handlers.Unit;

public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, UnitResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitRepository _unidadeRepository;
    private readonly IMessageProducer _messageProducer;
    private readonly IDistributedCache _cache;

    public CreateUnitCommandHandler(
        IMapper mapper,
        IUnitRepository unidadeRepository,
        IMessageProducer messageProducer,
        IDistributedCache cache
    )
    {
        _mapper = mapper;
        _unidadeRepository = unidadeRepository;
        _messageProducer = messageProducer;
        _cache = cache;
    }

    public async Task<UnitResponse> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unitDto = request.UnitDto;

        var validator = new UnitDtoValidator();
        var validationResult = validator.Validate(unitDto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage.TrimEnd('.')).ToList());

        try
        {
            var unit = _mapper.Map<UnitEntity>(unitDto);

            await _unidadeRepository.CreateUnitAsync(unit);

            var objetoParaSerializar = new
            {
                Nome = unit.Name,
                Email = unit.Email,
                Message = "Unit created successfully!"
            };

            var unidadeJson = JsonSerializer.Serialize(objetoParaSerializar);

            var responseDto = _mapper.Map<UnitResponse>(unit);

            await _cache.SetStringAsync(responseDto.PK, unidadeJson, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });


            await _messageProducer.SendAsync(unidadeJson);

            return responseDto;
        }
        catch
        {
            throw;
        }
    }
}