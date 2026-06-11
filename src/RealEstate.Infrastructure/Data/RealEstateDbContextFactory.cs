using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RealEstate.Infrastructure;

public class RealEstateDbContextFactory : IDesignTimeDbContextFactory<RealEstateDbContext>
{
    public RealEstateDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RealEstateDbContext>();
        optionsBuilder.UseSqlite("DataSource=RealEstate.db");

        return new RealEstateDbContext(optionsBuilder.Options);
    }
}
