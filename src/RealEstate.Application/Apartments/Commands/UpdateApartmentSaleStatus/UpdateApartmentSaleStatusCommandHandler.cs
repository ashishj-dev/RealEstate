using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class UpdateApartmentSaleStatusCommandHandler : IRequestHandler<UpdateApartmentSaleStatusCommand, int?>
{
    private  readonly IApartmentRepository _apartmentRepository;

    public UpdateApartmentSaleStatusCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<int?> Handle(UpdateApartmentSaleStatusCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetById(request.id, cancellationToken);

        if(apartment is null)
        {
            throw new ItemNotFoundException(request.id);
        }
        else
        {
            var apartmentId = await _apartmentRepository.UpdateSaleStatus(request.id, request.status, cancellationToken);
            await _apartmentRepository.SaveChangesAsync(cancellationToken);

            return apartmentId;
        }
        
    }
}
