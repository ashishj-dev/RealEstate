using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class ApartmentRepository : IApartmentRepository
{
    private readonly RealEstateDbContext _context;
    public ApartmentRepository(RealEstateDbContext context)
    {
        _context = context;
    }



    public async Task AddAsync(ApartmentEntity apartment, CancellationToken cancellationToken)
    {
        await _context.Apartments.AddAsync(apartment, cancellationToken);
    }

    public async Task<IReadOnlyList<ApartmentEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Apartments.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<ApartmentEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public Task<List<ApartmentEntity>> GetFilteredAsync(ApartmentType? type, int? floor, FacingDirection? direction, CancellationToken cancellationToken)
    {
        var query = _context.Apartments.AsQueryable();

        if(type.HasValue)
        {
            query = query.Where(a => a.Type == type.Value);
        }

        if(direction.HasValue)
        {
            query = query.Where(a => a.Direction == direction.Value);
        }

        if(floor.HasValue)
        {
            query = query.Where(a => a.Floor == floor.Value);
        }

        return query.ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
