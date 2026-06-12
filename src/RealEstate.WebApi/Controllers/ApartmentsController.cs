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

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<ApartmentEntity>> GetApartmentById (int id, CancellationToken cancellationToken)
        {
            var apartment = await _mediator.Send(new GetApartmentByIdQuery(id), cancellationToken);

            if(apartment is null)
            {
                return NotFound(new{message = $"Apartment with Id: {id} does not exist"});
            }

            return Ok(apartment);

        }


        [HttpGet("getbyfilter")]
        public async Task<ActionResult<List<ApartmentEntity>>> GetApartmentByFilter([FromQuery] int? floor,[FromQuery] ApartmentType? type, [FromQuery] FacingDirection? direction, CancellationToken cancellationToken)
        {
            var apartments = await _mediator.Send(new GetApartmentByFilterQuery(floor,type,direction), cancellationToken);

            if(apartments.Count < 1)
            {
                return NotFound(new{message = $"Apartment with current filters do not exist"});
            }

            return Ok(apartments);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<int>> DeleteApartmentById(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteApartmentByIdCommand(id), cancellationToken);
            return id;
        }

        [HttpPut("updateprice/{id}")]
        public async Task<ActionResult<ApartmentEntity?>> UpdateApartmentPrice(int id,[FromQuery] decimal price, CancellationToken cancellationToken)
        {
            var apartment = await _mediator.Send(new UpdateApartmentPriceCommand(id,price), cancellationToken);
            return apartment;
        }

        [HttpPut("updatesalestatus/{id}")]
        public async Task<ActionResult<ApartmentEntity?>> UpdateApartmentSaleStatus(int id,[FromQuery] bool status, CancellationToken cancellationToken)
        {
            var apartment = await _mediator.Send(new UpdateApartmentSaleStatusCommand(id, status), cancellationToken);
            return apartment;
        }
        
    }
}
