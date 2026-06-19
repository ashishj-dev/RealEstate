using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class GetAllApartmentsQueryHandler : IRequestHandler<GetAllApartmentsQuery, IReadOnlyList<ApartmentDTO>>
{
    private readonly IApartmentRepository _apartmentRepository;

    public GetAllApartmentsQueryHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<IReadOnlyList<ApartmentDTO>> Handle(GetAllApartmentsQuery request, CancellationToken cancellationToken)
    {
       return await _apartmentRepository.GetAllAsync(cancellationToken);
    }
}
