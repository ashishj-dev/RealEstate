using System;
using RealEstate.Domain;

namespace RealEstate.UnitTests;

public class ApartmentEntityTests
{

    [Fact]
    public void UpdatePrice_WhenPriceLessThanLimit_ThrowsException()
    {
        //Arrange
        var apartment = new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,150000);
        decimal updateprice = 10000m;

        //Act and Assert
        var exception = Assert.Throws<InvalidPriceException>(()=> apartment.UpdatePrice(updateprice));
    }

    [Fact]
    public void UpdatesPrice_WhenPriceIsValid_UpdatesPriceInApartmentEntity()
    {
        //Arrange
        var apartment = new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,200000m);
        decimal updateprice = 150000m;

        //Act
        apartment.UpdatePrice(updateprice);

        //Assert
        Assert.Equal(updateprice, apartment.Price);
    }

    [Fact]
    public void UpdateSaleStatus_WhenIsFalseAndGivenTrue_UpdatesAsSold()
    {
        //Arrange
        var apartment = new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,200000m);
        bool newStatus = true;

        //Act
        apartment.UpdateSaleStatus(newStatus);

        //Assert
        Assert.True(apartment.IsSold);
    }

    [Fact]
    public void UpdateSaleStatus_WhenIsTrueAndGivenFalse_UpdatesAsNotSold()
    {
        //Arrange
        var apartment = new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
        {
            IsSold = true
        };
        bool newStatus = false;

        //Act
        apartment.UpdateSaleStatus(newStatus);

        //Assert
        Assert.False(apartment.IsSold);
    }    

    
}
