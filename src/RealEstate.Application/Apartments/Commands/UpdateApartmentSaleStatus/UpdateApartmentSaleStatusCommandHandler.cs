using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class UpdateApartmentSaleStatusCommandHandler : IRequestHandler<UpdateApartmentSaleStatusCommand, ApartmentEntity?>
{
    private  readonly IApartmentRepository _apartmentRepository;

    public UpdateApartmentSaleStatusCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<ApartmentEntity?> Handle(UpdateApartmentSaleStatusCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.UpdateSaleStatus(request.id, request.status, cancellationToken);
        await _apartmentRepository.SaveChangesAsync(cancellationToken);

        return apartment;
    }
}
