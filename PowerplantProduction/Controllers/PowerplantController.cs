using MediatR;
using Microsoft.AspNetCore.Mvc;
using PowerplantProduction.Services.ProductionPlan;
using System.ComponentModel.DataAnnotations;

namespace PowerplantProduction.Hosting.Controllers
{
    [ApiController]
    public class PowerplantController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PowerplantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("productionplan")]
        public Task<ProductionPlanResponse> GetProductionPlan([FromBody][Required] ProductionPlanRequest productionPlanRequest,
        CancellationToken token)
        {
            return _mediator.Send(productionPlanRequest, token);
        }
    }
}
