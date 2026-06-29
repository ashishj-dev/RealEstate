using System;
using MediatR;
using System.Linq;
using RealEstate.Domain;

namespace RealEstate.Application;

public class GetApartmentByFilterQueryHandler : IRequestHandler<GetApartmentByFilterQuery, List<ApartmentDTO>>
{
    private readonly IRealEstateDbContext _context;
    public GetApartmentByFilterQueryHandler(IRealEstateDbContext realEstateDbContext)
    {
        _context = realEstateDbContext;
    }
    public async Task<List<ApartmentDTO>> Handle(GetApartmentByFilterQuery request, CancellationToken cancellationToken)
    {
       //return await _apartmentRepository.GetFilteredAsync(request.Type, request.floor, request.Direction, cancellationToken);
       int pageSize = request.pageSize > 50 ? 50 : request.pageSize;
        int skip = (request.pageNumber - 1) * pageSize;

        var query = _context.Apartments.AsQueryable();

        if(request.Type.HasValue)
        {
            query = query.Where(a => a.Type == request.Type.Value);
        }

        if(request.Direction.HasValue)
        {
            query = query.Where(a => a.Direction == request.Direction.Value);
        }

        if(request.floor.HasValue)
        {
            query = query.Where(a => a.Floor == request.floor.Value);
        }

        var apartmentDTOs = await query.Select(a => new ApartmentDTO
            {
                UnitNumber = a.UnitNumber,
                Floor = a.Floor,
                Type = a.Type,
                Direction = a.Direction,
                IsSold = a.IsSold
            }).ToAsyncEnumerable()
            .ToListAsync(cancellationToken);
        
        return apartmentDTOs;

    }

    
}
