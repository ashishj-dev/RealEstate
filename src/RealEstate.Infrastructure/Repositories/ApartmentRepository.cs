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

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
