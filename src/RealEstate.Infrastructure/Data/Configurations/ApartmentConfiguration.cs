using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class ApartmentConfiguration : IEntityTypeConfiguration<ApartmentEntity>
{
    public void Configure(EntityTypeBuilder<ApartmentEntity> builder)
    {
        //Set primary key
        builder.HasKey(a => a.Id);

        //Convert apartment type enum to sring
        builder.Property(a => a.Type).HasConversion<string>().HasMaxLength(50);
        //Convert direction enum to string
        builder.Property(a => a.Direction).HasConversion<string>().HasMaxLength(50);

        builder.Property(a => a.UnitNumber).IsRequired().HasMaxLength(20);
        //convert price from decimal to string since sqlite doesnt have decimal type
        builder.Property(a => a.Price).HasColumnType("TEXT");
    }
}
