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

    
    //Create new Apartment
    public async Task AddAsync(ApartmentEntity apartment, CancellationToken cancellationToken)
    {
        await _context.Apartments.AddAsync(apartment, cancellationToken);
    }

    //Delete Apartment By Id
    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken)
    {
        var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

        if(apartment is not null)
            _context.Apartments.Remove(apartment);
        else
            throw new InvalidDataException();
    }

    //Get All Apartments 
    public async Task<IReadOnlyList<ApartmentDTO>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Apartments.AsNoTracking()
        .Select(a => new ApartmentDTO
        {
            UnitNumber = a.UnitNumber,
            Floor = a.Floor,
            Type = a.Type,
            Direction = a.Direction,
            IsSold = a.IsSold
        })
        .ToListAsync(cancellationToken);
    }

    // Get Apartment by Id
    public async Task<ApartmentDTO?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        //return await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        return await _context.Apartments.Where(a => a.Id == id)
        .Select(p => new ApartmentDTO
        {
            UnitNumber = p.UnitNumber,
            Floor = p.Floor,
            Type = p.Type,
            Direction = p.Direction,
            IsSold = p.IsSold
        }).FirstOrDefaultAsync(cancellationToken);
    }

    //Get Apartment by Filter
    public Task<List<ApartmentDTO>> GetFilteredAsync(ApartmentType? type, int? floor, FacingDirection? direction, CancellationToken cancellationToken)
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

        return query.Select(a => new ApartmentDTO
        {
            UnitNumber = a.UnitNumber,
            Floor = a.Floor,
            Type = a.Type,
            Direction = a.Direction,
            IsSold = a.IsSold
        })
        .ToListAsync(cancellationToken);
    }

    // Save changes to database
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ApartmentDTO?> UpdatePrice(int id, decimal price, CancellationToken cancellationToken)
    {
        var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);
        

        if(apartment is not null)
        {
            apartment.UpdatePrice(price);

            var apartmentDto = new ApartmentDTO
            {
                UnitNumber = apartment.UnitNumber,
                Floor = apartment.Floor,
                Type = apartment.Type,
                Direction = apartment.Direction,
                Price = apartment.Price,
                IsSold = apartment.IsSold
            };
            
            return apartmentDto;
        }
        
        return null;

    }

    public async Task<ApartmentDTO?> UpdateSaleStatus(int id, bool status, CancellationToken cancellationToken)
    {
        var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

        if(apartment is not null)
        {
            apartment.UpdateSaleStatus(status);

            var apartmentDto = new ApartmentDTO
            {
                UnitNumber = apartment.UnitNumber,
                Floor = apartment.Floor,
                Type = apartment.Type,
                Direction = apartment.Direction,
                Price = apartment.Price,
                IsSold = apartment.IsSold
            };
            
            return apartmentDto;
        }
        
        return null;
    }
}
