using AutoMapper;
using Powerplant.API.Contracts;
using Powerplant.Domain.Models;

namespace Powerplant.API.MapperProfiles
{
    public class ProductionPlantProfile : Profile
    {
        public ProductionPlantProfile()
        {
            CreateMap<FuelInfo, FuelInfoModel>();
            CreateMap<Contracts.Powerplant, PowerplantModel>();
        }
    }
}
