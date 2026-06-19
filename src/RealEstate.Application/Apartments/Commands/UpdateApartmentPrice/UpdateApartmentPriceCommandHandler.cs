using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class UpdateApartmentPriceCommandHandler : IRequestHandler<UpdateApartmentPriceCommand, int?>
{
    private  readonly IApartmentRepository _apartmentRepository;

    public UpdateApartmentPriceCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<int?> Handle(UpdateApartmentPriceCommand request, CancellationToken cancellationToken)
    {   
        var apartment = await _apartmentRepository.GetById(request.id, cancellationToken);

        if(apartment is null)
        {
            throw new ItemNotFoundException(request.id);
        }
        else
        {
            var apartmentId = await _apartmentRepository.UpdatePrice(request.id, request.price, cancellationToken);
            await _apartmentRepository.SaveChangesAsync(cancellationToken);

            return apartmentId;
        }

        
    }
}
