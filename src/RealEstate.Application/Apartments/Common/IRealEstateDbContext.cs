using System;
using RealEstate.Domain;

namespace RealEstate.Application;

public interface IRealEstateDbContext
{
    IQueryable<ApartmentEntity> Apartments {get;}
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
