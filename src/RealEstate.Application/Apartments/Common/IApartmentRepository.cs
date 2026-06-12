using System;
using RealEstate.Domain;

namespace RealEstate.Application;

public interface IApartmentRepository
{
    Task<ApartmentEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ApartmentEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(ApartmentEntity apartment, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<List<ApartmentEntity>> GetFilteredAsync(ApartmentType? type, int? floor, FacingDirection? direction, CancellationToken cancellationToken);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    
    Task <ApartmentEntity?> UpdatePrice(int id, decimal price, CancellationToken cancellationToken);
    Task <ApartmentEntity?> UpdateSaleStatus(int id, bool status, CancellationToken cancellationToken);

}
