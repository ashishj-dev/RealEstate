using System;
using RealEstate.Domain;

namespace RealEstate.Application;

public interface IApartmentRepository
{
    Task<ApartmentEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ApartmentEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(ApartmentEntity apartment, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

}
