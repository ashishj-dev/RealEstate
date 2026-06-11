using System;
using MediatR;

namespace RealEstate.Application;

public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, Guid>
{
    public Task<Guid> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        // 1. In a future step, this is where we will call your constructor:
        // var apartment = new ApartmentEntity(request.UnitNumber, request.Floor, request.Type, request.Direction, request.Price);
        // (This will automatically run your domain validation logic/exceptions!)

        // 2. Temporarily generate a fake ID to simulate a successful database save
        Guid fakeSavedId = Guid.NewGuid();

        // 3. Return the ID wrapped in a completed Task
        return Task.FromResult(fakeSavedId);
    }
}
