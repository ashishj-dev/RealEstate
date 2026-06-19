using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class UpdateApartmentPriceCommandHandler : IRequestHandler<UpdateApartmentPriceCommand, ApartmentDTO?>
{
    private  readonly IApartmentRepository _apartmentRepository;

    public UpdateApartmentPriceCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<ApartmentDTO?> Handle(UpdateApartmentPriceCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.UpdatePrice(request.id, request.price, cancellationToken);
        await _apartmentRepository.SaveChangesAsync(cancellationToken);

        return apartment;
    }
}
