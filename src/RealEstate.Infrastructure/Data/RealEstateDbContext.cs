using System;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class RealEstateDbContext : DbContext, IRealEstateDbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
        
    }

    public DbSet<ApartmentEntity> Apartments {get; set;}

    IQueryable<ApartmentEntity> IRealEstateDbContext.Apartments => Apartments;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RealEstateDbContext).Assembly);
    }
}
