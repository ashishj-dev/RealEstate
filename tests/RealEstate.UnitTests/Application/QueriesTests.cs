using System;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RealEstate.Application;
using RealEstate.Domain;
using Mockable.Moq;
using Moq.EntityFrameworkCore;

namespace RealEstate.UnitTests;

public class QueriesTests
{
    [Fact]
    public async Task GetApartmentByIdQuery_WithValidId_ReturnsApartment()
    {
        //Arrange
        var targetId = 1;
        //set up return apartment var
        var apartment = new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
        {
            Id = targetId,
            IsSold = false
        };

        var apartments = new List<ApartmentEntity>
        {
            apartment
        };

        //mock realestatedbcontext
        var mockDbContext = new Mock<IRealEstateDbContext>();
        mockDbContext.Setup(x => x.Apartments).Returns(apartments.AsQueryable());

        //setup query and handler
        var query = new GetApartmentByIdQuery(targetId);
        var handler = new GetApartmentByIdQueryHandler(mockDbContext.Object);

        //Act
        //put result of function into variable
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        //check the variable to match with return apartment var
        Assert.NotNull(result);
        Assert.Equal(apartment.UnitNumber, result.UnitNumber);
        Assert.Equal(apartment.Floor, result.Floor);
    }


    [Fact]
    public async Task GetApartmentByIdQuery_WithInValidId_ThrowsException()
    {
        //Arrange
        var targetId = 1;
        //set up return apartment var
        var apartment = new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
        {
            Id = 3,
            IsSold = false
        };

        var apartments = new List<ApartmentEntity>
        {
            apartment
        };

        //mock realestatedbcontext
        var mockDbContext = new Mock<IRealEstateDbContext>();
        mockDbContext.Setup(x => x.Apartments).Returns(apartments.AsQueryable());

        //setup query and handler
        var query = new GetApartmentByIdQuery(targetId);
        var handler = new GetApartmentByIdQueryHandler(mockDbContext.Object);

        //Act
        //put result of function into variable
        var exception = await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        Assert.Contains(targetId.ToString(), exception.Message);
    }



    //By Filter
    [Theory]
    [InlineData(3, null, null, 1)]
    [InlineData(1, ApartmentType.OneBhk, null, 2)]
    [InlineData(null, null, FacingDirection.East, 2)]
    [InlineData(1, ApartmentType.OneBhk, FacingDirection.North, 1)]
    public async Task GetApartmentByFilter_WithValidFilter_ReturnsApartments(int? floor, ApartmentType? type, FacingDirection? direction, int expectedMatchCount)
    {
        //Arrange

        //set up return apartment var
       

        var apartments = new List<ApartmentEntity>
        {
            new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
            {
                Id = 1
            },

            new ApartmentEntity("201-A", 2, ApartmentType.TwoBhk, FacingDirection.North, 200000m)
            {
                Id = 2
            },

            new ApartmentEntity("102-A", 1, ApartmentType.OneBhk, FacingDirection.East, 200000m)
            {
                Id = 3
            },

            new ApartmentEntity("202-A", 2, ApartmentType.TwoBhk, FacingDirection.East, 200000m)
            {
                Id = 4
            },

            new ApartmentEntity("301-A", 3, ApartmentType.Penthouse, FacingDirection.North, 200000m)
            {
                Id = 5
            },

        };

        //mock realestatedbcontext
        var mockDbContext = new Mock<IRealEstateDbContext>();
        mockDbContext.Setup(x => x.Apartments).Returns(apartments.AsQueryable());

        //setup query and handler
        var query = new GetApartmentByFilterQuery(10, 1, floor, type, direction);
        var handler = new GetApartmentByFilterQueryHandler(mockDbContext.Object);

        //Act
        //put result of function into variable
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        //check the variable to match with return apartment var
        Assert.NotNull(result);
        Assert.Equal(expectedMatchCount, result.Count);
    }


    [Theory]
    [InlineData(null, null, null, 5)]
    [InlineData(7, null, null, 0)]
    [InlineData(1, ApartmentType.Penthouse, null, 0)]
    [InlineData(null, null, FacingDirection.West, 0)]
    [InlineData(9, ApartmentType.ThreeBhk, FacingDirection.West, 0)]
    public async Task GetApartmentByFilter_WithVInvalidFilter_ReturnsNothing(int? floor, ApartmentType? type, FacingDirection? direction, int expectedMatchCount)
    {
        //Arrange

        //set up return apartment var
       

        var apartments = new List<ApartmentEntity>
        {
            new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
            {
                Id = 1
            },

            new ApartmentEntity("201-A", 2, ApartmentType.TwoBhk, FacingDirection.North, 200000m)
            {
                Id = 2
            },

            new ApartmentEntity("102-A", 1, ApartmentType.OneBhk, FacingDirection.East, 200000m)
            {
                Id = 3
            },

            new ApartmentEntity("202-A", 2, ApartmentType.TwoBhk, FacingDirection.East, 200000m)
            {
                Id = 4
            },

            new ApartmentEntity("301-A", 3, ApartmentType.Penthouse, FacingDirection.North, 200000m)
            {
                Id = 5
            },

        };

        //mock realestatedbcontext
        var mockDbContext = new Mock<IRealEstateDbContext>();
        mockDbContext.Setup(x => x.Apartments).Returns(apartments.AsQueryable());

        //setup query and handler
        var query = new GetApartmentByFilterQuery(10, 1, floor, type, direction);
        var handler = new GetApartmentByFilterQueryHandler(mockDbContext.Object);

        //Act
        //put result of function into variable
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        //check the variable to match with return apartment var
        Assert.NotNull(result);
        Assert.Equal(expectedMatchCount, result.Count);
    }


    [Theory]
    [InlineData(null, null, null, 5)]
    public async Task GetApartmentByFilter_WithNoFilter_ReturnsAll(int? floor, ApartmentType? type, FacingDirection? direction, int expectedMatchCount)
    {
        //Arrange

        //set up return apartment var
       

        var apartments = new List<ApartmentEntity>
        {
            new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
            {
                Id = 1
            },

            new ApartmentEntity("201-A", 2, ApartmentType.TwoBhk, FacingDirection.North, 200000m)
            {
                Id = 2
            },

            new ApartmentEntity("102-A", 1, ApartmentType.OneBhk, FacingDirection.East, 200000m)
            {
                Id = 3
            },

            new ApartmentEntity("202-A", 2, ApartmentType.TwoBhk, FacingDirection.East, 200000m)
            {
                Id = 4
            },

            new ApartmentEntity("301-A", 3, ApartmentType.Penthouse, FacingDirection.North, 200000m)
            {
                Id = 5
            },

        };

        //mock realestatedbcontext
        var mockDbContext = new Mock<IRealEstateDbContext>();
        mockDbContext.Setup(x => x.Apartments).Returns(apartments.AsQueryable());

        //setup query and handler
        var query = new GetApartmentByFilterQuery(10, 1, floor, type, direction);
        var handler = new GetApartmentByFilterQueryHandler(mockDbContext.Object);

        //Act
        //put result of function into variable
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        //check the variable to match with return apartment var
        Assert.NotNull(result);
        Assert.Equal(expectedMatchCount, result.Count);
    }




}
