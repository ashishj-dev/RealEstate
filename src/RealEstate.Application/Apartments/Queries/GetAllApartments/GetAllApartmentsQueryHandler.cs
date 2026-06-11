using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public class GetAllApartmentsQueryHandler : IRequestHandler<GetAllApartmentsQuery, IReadOnlyList<ApartmentEntity>>
{
    public Task<IReadOnlyList<ApartmentEntity>> Handle(GetAllApartmentsQuery request, CancellationToken cancellationToken)
    {
        //temp return empty list wrapped in a completed task. will change when db is added
        IReadOnlyList<ApartmentEntity> emptyList = new List<ApartmentEntity>();

        return Task.FromResult(emptyList);
    }
}
