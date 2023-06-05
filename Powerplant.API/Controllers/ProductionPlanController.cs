using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Powerplant.API.Contracts;
using Powerplant.Domain.Interfaces;
using Powerplant.Domain.Models;
using Powerplant.Infrastructure.Exceptions;

namespace Powerplant.API.Controllers
{
    [ApiController]
    [Route("/productionplan")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly IProductionPlanService _productionPlanService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductionPlanController> _logger;

        public ProductionPlanController(
            IProductionPlanService productionPlanService,
            IMapper mapper,
            ILogger<ProductionPlanController> logger)
        {
            _productionPlanService = productionPlanService ?? throw new ArgumentNullException(nameof(productionPlanService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PowerplantRequest payload)
        {
            try
            {
                return Ok(await _productionPlanService.CalculateProductionPlan(
                    payload.PowerPlants,
                    payload.Load,
                    _mapper.Map<FuelInfoModel>(payload.FuelInfo)
                    ));
            }
            catch (FulfillmentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
