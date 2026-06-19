using System;
using RealEstate.Domain;

namespace RealEstate.Application;

public interface IApartmentRepository
{
    Task<ApartmentDTO?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ApartmentDTO>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(ApartmentEntity apartment, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<List<ApartmentDTO>> GetFilteredAsync(ApartmentType? type, int? floor, FacingDirection? direction, CancellationToken cancellationToken);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
    
    Task <ApartmentDTO?> UpdatePrice(int id, decimal price, CancellationToken cancellationToken);
    Task <ApartmentDTO?> UpdateSaleStatus(int id, bool status, CancellationToken cancellationToken);

}
