using System;
using MediatR;
using RealEstate.Application;

namespace RealEstate.Application;

public class DeleteApartmentByIdCommandHandler : IRequestHandler<DeleteApartmentByIdCommand, int>
{
    private  readonly IApartmentRepository _apartmentRepository;

    public DeleteApartmentByIdCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }
    public async Task<int> Handle(DeleteApartmentByIdCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetById(request.id, cancellationToken);

        if(apartment is not null)
        {
            await _apartmentRepository.DeleteByIdAsync(request.id, cancellationToken);
            await _apartmentRepository.SaveChangesAsync(cancellationToken);
            return request.id;
        }
        else
        {
            throw new ItemNotFoundException(request.id);
        }
        
    }
}
