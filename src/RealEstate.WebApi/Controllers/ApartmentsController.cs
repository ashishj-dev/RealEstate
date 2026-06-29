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
        public async Task<ActionResult<IReadOnlyList<ApartmentDTO>>> GetAllApartments([FromQuery] int pSize, [FromQuery] int pNumber, CancellationToken cancellationToken)
        {
            var apartments = await _mediator.Send(new GetAllApartmentsQuery(pNumber,pSize), cancellationToken);

            return Ok(apartments);
        }


        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<ApartmentDTO>> GetApartmentById (int id, CancellationToken cancellationToken)
        {
            var apartment = await _mediator.Send(new GetApartmentByIdQuery(id), cancellationToken);

            if(apartment is null)
            {
                return NotFound(new{message = $"Apartment with Id: {id} does not exist"});
            }

            return Ok(apartment);

        }


        [HttpGet("getbyfilter")]
        public async Task<ActionResult<List<ApartmentDTO>>> GetApartmentByFilter([FromQuery] int pSize, [FromQuery] int pNumber,  [FromQuery] int? floor,[FromQuery] ApartmentType? type, [FromQuery] FacingDirection? direction, CancellationToken cancellationToken)
        {
            var apartments = await _mediator.Send(new GetApartmentByFilterQuery(pSize, pNumber, floor, type, direction), cancellationToken);

            if(apartments.Count < 1)
            {
                return NotFound(new{message = $"Apartment with current filters do not exist"});
            }

            return Ok(apartments);
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<int>> DeleteApartmentById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteApartmentByIdCommand(id), cancellationToken);
            return result;
        }


        [HttpPut("updateprice/{id}")]
        public async Task<ActionResult<int?>> UpdateApartmentPrice(int id,[FromQuery] decimal price, CancellationToken cancellationToken)
        {
            var apartmentId = await _mediator.Send(new UpdateApartmentPriceCommand(id,price), cancellationToken);
            return apartmentId;
        }


        [HttpPut("updatesalestatus/{id}")]
        public async Task<ActionResult<int?>> UpdateApartmentSaleStatus(int id,[FromQuery] bool status, CancellationToken cancellationToken)
        {
            var apartmentId = await _mediator.Send(new UpdateApartmentSaleStatusCommand(id, status), cancellationToken);
            return apartmentId;
        }
        
    }
}
