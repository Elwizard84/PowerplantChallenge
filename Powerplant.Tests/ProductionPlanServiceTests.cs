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
        [Theory]
        [InlineData("Payloads/payload1.json", 3, "gasfiredbig1", 368.4)]
        [InlineData("Payloads/payload2.json", 2, "gasfiredbig2", 100)]
        [InlineData("Payloads/payload3.json", 4, "gasfiredbig2", 338.4)]
        public async void CalculateProductionPlan_ShouldReturn_Success(string filePath, int count, string plantName, decimal result)
        {
            // Arrange
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
            Assert.Equal(count, productionPlans.Count());
            Assert.True(productionPlans.Single(p => p.Name == plantName).P == result);
        }

        private PowerplantRequest ReadPayloadFromFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<PowerplantRequest>(content);
        }
    }
}