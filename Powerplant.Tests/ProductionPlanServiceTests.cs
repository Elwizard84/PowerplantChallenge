using AutoMapper;
using Powerplant.API.Contracts;
using Powerplant.API.MapperProfiles;
using Powerplant.Domain.Models;
using Powerplant.Infrastructure.Services;
using System.Text.Json;

namespace Powerplant.Tests
{
    public class ProductionPlanServiceTests
    {
        [Fact]
        public async void CalculateProductionPlan_ShouldReturn_3_Last_3684()
        {
            // Arrange
            string filePath = "Payloads/payload1.json";
            PowerplantRequest payload = ReadPayloadFromFile(filePath);

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductionPlantProfile());
            });
            IMapper mapper = new Mapper(mapperConfig);

            var service = new ProductionPlanService();

            // Act
            var productionPlans = await service.CalculateProductionPlan(mapper.Map<List<PowerplantModel>>(payload.PowerPlants), payload.Load, mapper.Map<FuelInfoModel>(payload.FuelInfo));

            // Assert
            Assert.True(productionPlans.Count() == 3);
            Assert.True(productionPlans.Single(p => p.Name == "gasfiredbig1").P == 368.4M);
        }

        [Fact]
        public async void CalculateProductionPlan_ShouldReturn_2_Last_100()
        {
            // Arrange
            string filePath = "Payloads/payload2.json";
            PowerplantRequest payload = ReadPayloadFromFile(filePath);

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductionPlantProfile());
            });
            IMapper mapper = new Mapper(mapperConfig);

            var service = new ProductionPlanService();

            // Act
            var productionPlans = await service.CalculateProductionPlan(mapper.Map<List<PowerplantModel>>(payload.PowerPlants), payload.Load, mapper.Map<FuelInfoModel>(payload.FuelInfo));

            // Assert
            Assert.True(productionPlans.Count() == 2);
            Assert.True(productionPlans.Single(p => p.Name == "gasfiredbig2").P == 100M);
        }

        [Fact]
        public async void CalculateProductionPlan_ShouldReturn_4_Last_3384()
        {
            // Arrange
            string filePath = "Payloads/payload3.json";
            PowerplantRequest payload = ReadPayloadFromFile(filePath);

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductionPlantProfile());
            });
            IMapper mapper = new Mapper(mapperConfig);

            var service = new ProductionPlanService();

            // Act
            var productionPlans = await service.CalculateProductionPlan(mapper.Map<List<PowerplantModel>>(payload.PowerPlants), payload.Load, mapper.Map<FuelInfoModel>(payload.FuelInfo));

            // Assert
            Assert.True(productionPlans.Count() == 4);
            Assert.True(productionPlans.Single(p => p.Name == "gasfiredbig2").P == 338.4M);
        }

        private PowerplantRequest ReadPayloadFromFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<PowerplantRequest>(content);
        }
    }
}