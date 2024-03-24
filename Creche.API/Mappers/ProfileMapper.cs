using AutoMapper;
using Creche.Application.DTOs;
using Creche.Application.Entities;
using Creche.Application.Responses;

namespace Creche.API.Mappers;

public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<UnitResponse, UnitEntity>().ReverseMap();
        CreateMap<UnitDto, UnitEntity>();
    }
}