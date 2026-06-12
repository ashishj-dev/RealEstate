using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class GetApartmentByIdQueryHandler : IRequestHandler<GetApartmentByIdQuery, ApartmentEntity?>
{
    private readonly IApartmentRepository _apartmentRepository;
    public GetApartmentByIdQueryHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }
    public async Task<ApartmentEntity?> Handle(GetApartmentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _apartmentRepository.GetByIdAsync(request.id, cancellationToken);
    }
}
