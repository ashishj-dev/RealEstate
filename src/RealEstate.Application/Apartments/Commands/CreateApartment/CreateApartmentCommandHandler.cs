using System;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, int>
{
    private  readonly IApartmentRepository _apartmentRepository;

    public CreateApartmentCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }
    public async Task<int> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = new ApartmentEntity(
            request.unitNumber,
            request.floor,
            request.type,
            request.direction,
            request.price
        );
        
        await _apartmentRepository.AddAsync(apartment, cancellationToken);
        await _apartmentRepository.SaveChangesAsync(cancellationToken);

        return apartment.Id;
    }
}
