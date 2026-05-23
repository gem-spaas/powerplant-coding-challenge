using AutoMapper;
using Powerplant.Api.Domain;
using Powerplant.Api.Generated;

namespace Powerplant.Api.Mapping;

public sealed class ProductionPlanMappingProfile : Profile
{
    public ProductionPlanMappingProfile()
    {
        CreateMap<Fuels, FuelPrices>()
            .ConvertUsing<FuelsToFuelPricesMapper>();
        CreateMap<PowerPlantDto, PowerPlant>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => MapPowerPlantType(src.Type)))
            .ForMember(dest => dest.PMin, opt => opt.MapFrom(src => Convert.ToDecimal(src.Pmin)))
            .ForMember(dest => dest.PMax, opt => opt.MapFrom(src => Convert.ToDecimal(src.Pmax)))
            .ForMember(dest => dest.Efficiency, opt => opt.MapFrom(src => Convert.ToDecimal(src.Efficiency)));
        CreateMap<ProductionPlanEntry, ProductionPlanResponseItem>()
            .ForMember(dest => dest.P, opt => opt.MapFrom(src => Convert.ToDouble(src.Power)));
    }

    private static PowerPlantType MapPowerPlantType(PowerPlantDtoType type)
    {
        return type switch
        {
            PowerPlantDtoType.Gasfired => PowerPlantType.GasFired,
            PowerPlantDtoType.Turbojet => PowerPlantType.TurboJet,
            PowerPlantDtoType.Windturbine => PowerPlantType.WindTurbine,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported powerplant type.")
        };
    }
}
