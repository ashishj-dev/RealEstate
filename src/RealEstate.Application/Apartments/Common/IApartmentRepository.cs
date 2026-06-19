using System;
using RealEstate.Domain;

namespace RealEstate.Application;

public interface IApartmentRepository
{
    // Repository for all write tasks

    Task AddAsync(ApartmentEntity apartment, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    Task <int?> UpdatePrice(int id, decimal price, CancellationToken cancellationToken);
    Task <int?> UpdateSaleStatus(int id, bool status, CancellationToken cancellationToken);
    Task<ApartmentEntity?> GetById(int id, CancellationToken cancellationToken);

}
