using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class GetApartmentByFilterQueryHandler : IRequestHandler<GetApartmentByFilterQuery, List<ApartmentDTO>>
{
    private readonly IApartmentRepository _apartmentRepository;
    public GetApartmentByFilterQueryHandler(IApartmentRepository repository)
    {
        _apartmentRepository = repository;
    }
    public async Task<List<ApartmentDTO>> Handle(GetApartmentByFilterQuery request, CancellationToken cancellationToken)
    {
       return await _apartmentRepository.GetFilteredAsync(request.Type, request.floor, request.Direction, cancellationToken);

    }
}
