using System;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
        
    }

    public DbSet<ApartmentEntity> Apartments {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RealEstateDbContext).Assembly);
    }
}
