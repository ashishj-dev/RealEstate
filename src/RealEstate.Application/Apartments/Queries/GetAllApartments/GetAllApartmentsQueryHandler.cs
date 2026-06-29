using System;
using MediatR;
using RealEstate.Domain;
using System.Linq;

namespace RealEstate.Application;

public class GetAllApartmentsQueryHandler : IRequestHandler<GetAllApartmentsQuery, IReadOnlyList<ApartmentDTO>>
{
    private readonly IRealEstateDbContext _context;

    public GetAllApartmentsQueryHandler(IRealEstateDbContext realEstateDbContext)
    {
        
        _context = realEstateDbContext;
    }

    public async Task<IReadOnlyList<ApartmentDTO>> Handle(GetAllApartmentsQuery request, CancellationToken cancellationToken)
    {
       //return await _apartmentRepository.GetAllAsync(cancellationToken);

        int pageSize = request.pageSize > 50 ? 50 : request.pageSize;
        int skip = (request.pageNumber - 1) * pageSize;

        var apartmentDTOs = await  _context.Apartments.OrderBy(a=> a.Id)
        .Skip(skip)
        .Take(pageSize)
        .Select(a =>  new ApartmentDTO
        {
            UnitNumber = a.UnitNumber,
            Floor = a.Floor,
            Type = a.Type,
            Direction = a.Direction,
            Price = a.Price,
            IsSold = a.IsSold
        }).ToAsyncEnumerable().ToListAsync(cancellationToken);

        return apartmentDTOs;
       
    }
}
