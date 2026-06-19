using System;
using MediatR;
using RealEstate.Domain;
using System.Linq;

namespace RealEstate.Application;

public class GetApartmentByIdQueryHandler : IRequestHandler<GetApartmentByIdQuery, ApartmentDTO?>
{
    private readonly IRealEstateDbContext _context;
    public GetApartmentByIdQueryHandler(IRealEstateDbContext realEstateDbContext)
    {
        _context = realEstateDbContext;
    }
    public async Task<ApartmentDTO?> Handle(GetApartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var apartmentDto = await _context.Apartments.Where(a => a.Id == request.id)
        .Select(a => new ApartmentDTO
        {
            UnitNumber = a.UnitNumber,
            Floor = a.Floor,
            Type = a.Type,
            Direction = a.Direction,
            Price = a.Price,
            IsSold = a.IsSold
        }).ToAsyncEnumerable().FirstOrDefaultAsync(cancellationToken);

        if(apartmentDto is null)
        {
            throw new FileNotFoundException("Apartment with id: {id} not found", request.id.ToString());
        }

        return apartmentDto;
    }
}
