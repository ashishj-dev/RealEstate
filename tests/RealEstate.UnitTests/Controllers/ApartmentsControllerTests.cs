using System;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using RealEstate.Application;
using RealEstate.Infrastructure;
using RealEstate.Domain;
using Microsoft.AspNetCore.Mvc;


namespace RealEstate.UnitTests;

public class ApartmentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ApartmentsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetApartmentById_WhenApartmentExists_ReturnsApartment()
    {
        //Arrange
        using(var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            var testApartment = new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,150000m)
            {
                IsSold = false
            };

            db.Apartments.Add(testApartment);
            await db.SaveChangesAsync();
        }
        
        //Act
        var response = await _client.GetAsync("/api/apartments/getbyid/1");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        var dto = await response.Content.ReadFromJsonAsync<ApartmentDTO>(jsonOptions);
        Assert.NotNull(dto);
        Assert.Equal(1, dto.Floor);
        Assert.Equal(150000, dto.Price);
        Assert.Equal(ApartmentType.OneBhk, dto.Type);
        Assert.Equal(FacingDirection.North, dto.Direction);
        Assert.Equal("101-A", dto.UnitNumber);

    }

    [Fact]
    public async Task GetApartmentById_WhenApartmentDoesNotExist_ReturnsHttpNotFound()
    {
        //Arrange
        using(var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            var testApartment = new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,150000m)
            {
                IsSold = false
            };

            db.Apartments.Add(testApartment);
            await db.SaveChangesAsync();
        }
        
        //Act
        var response = await _client.GetAsync("/api/apartments/getbyid/99");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task GetAll_WhenDataExists_ReturnsData()
    {
        //Arrange
        using(var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();


            db.Apartments.AddRange(
                new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,150000m),
                new ApartmentEntity("201-A",2,ApartmentType.TwoBhk,FacingDirection.East,190000m)

            );
            await db.SaveChangesAsync();


            //Act
            var response = await _client.GetAsync("/api/apartments/getall?pSize=10&pNumber=1");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            

            var apartments = await response.Content.ReadFromJsonAsync<List<ApartmentDTO>>(jsonOptions);
            Assert.NotNull(apartments);
            Assert.Equal(2, apartments.Count);

            Assert.Contains(apartments, a => a.UnitNumber == "101-A");
            Assert.Contains(apartments, a => a.UnitNumber == "201-A");

        }
    }

    [Fact]
    public async Task GetAll_WhenDataDoesNotExist_ReturnsEmptyList()
    {
        //Arrange
        using(var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();


            //Act
            var response = await _client.GetAsync("/api/apartments/getall?pSize=10&pNumber=1");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            

            var apartments = await response.Content.ReadFromJsonAsync<List<ApartmentDTO>>(jsonOptions);
            Assert.NotNull(apartments);
            Assert.Empty(apartments);

        }
    }

    [Fact]
    public async Task AddApartment_WhenDataIsValid_ReturnsNewID()
    {
        //Arrange
        using(var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();


            db.Apartments.AddRange(
                new ApartmentEntity("101-A",1,ApartmentType.OneBhk,FacingDirection.North,150000m),
                new ApartmentEntity("201-A",2,ApartmentType.TwoBhk,FacingDirection.East,190000m)

            );
            await db.SaveChangesAsync();

            //Act
            var command = new CreateApartmentCommand(
                "103-A",
                1,
                ApartmentType.OneBhk,
                FacingDirection.East,
                145000m);

            var response = await _client.PostAsJsonAsync("/api/apartments/add", command);

            //Assert
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // This will force the test to fail and output the exact validation problems
                Assert.Fail($"API returned Bad Request. Error detail: {errorContent}");
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(3, createdId);
            Assert.True(createdId > 2);
            
        }
    }


}
