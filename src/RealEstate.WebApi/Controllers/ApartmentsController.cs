using System.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application;
using RealEstate.Domain;

namespace RealEstate.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISender _mediator;

        public ApartmentsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<ActionResult<int>> CreateApartment(CreateApartmentCommand command, CancellationToken cancellationToken)
        {
            int apartmentId = await _mediator.Send(command, cancellationToken);

            return Ok(apartmentId);
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IReadOnlyList<ApartmentEntity>>> GetAllApartments(CancellationToken cancellationToken)
        {
            var apartments = await _mediator.Send(new GetAllApartmentsQuery(), cancellationToken);

            return Ok(apartments);
        }

        
    }
}
