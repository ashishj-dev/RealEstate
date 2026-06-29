using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class ApartmentRepository : IApartmentRepository
{
    //Repository for all write tasks
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
            throw new InvalidDataException("No apartment exists");
    }

    public async Task<ApartmentEntity?> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

        return result;
    }


    // Save changes to database
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int?> UpdatePrice(int id, decimal price, CancellationToken cancellationToken)
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
            
            return apartment.Id;
        }
        
        return null;

    }

    public async Task<int?> UpdateSaleStatus(int id, bool status, CancellationToken cancellationToken)
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
            
            return apartment.Id;
        }
        
        return null;
    }
}
