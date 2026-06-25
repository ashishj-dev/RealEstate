using System;
using Moq;
using RealEstate.Application;
using RealEstate.Domain;

namespace RealEstate.UnitTests;

public class CommandsTests
{
    private readonly Mock<IApartmentRepository> _arpartmentRepositoryMock;

    public CommandsTests()
    {
        _arpartmentRepositoryMock = new Mock<IApartmentRepository>();
    }


    // Create Apartment Tests
    [Fact]
    public async Task CreateApartmentCommand_WithValidInputs_ShouldReturnNewApartmentID()
    {


        var command = new CreateApartmentCommand( "909-A", 9, ApartmentType.Penthouse, FacingDirection.North, 150000m);

        var expectedId = 1;
 
        var handler = new CreateApartmentCommandHandler(_arpartmentRepositoryMock.Object);

       _arpartmentRepositoryMock
        .Setup(repo => repo.AddAsync(It.IsAny<ApartmentEntity>(), It.IsAny<CancellationToken>()))
        .Callback<ApartmentEntity, CancellationToken>((apartment, token) => 
        {
            apartment.Id = expectedId;
        })
        .Returns(Task.CompletedTask);


        var result = await handler.Handle(command, CancellationToken.None);
        
        Assert.Equal(expectedId, result);

        _arpartmentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<ApartmentEntity>(a => a.UnitNumber == command.unitNumber), It.IsAny<CancellationToken>()), Times.Once);
        

    }


    // Delete Tests

    [Fact]
    public async Task DeleteById_WithValidId_DeletesAndReturnsID()
    {
        var idToDelte = 1;

        var dummyApartment = new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
        {
            Id = idToDelte
        };

        var command = new DeleteApartmentByIdCommand(idToDelte);
        var handler = new DeleteApartmentByIdCommandHandler(_arpartmentRepositoryMock.Object);

        //Process of handelr, getbyId, delete, savechanges
        _arpartmentRepositoryMock.Setup(repo => repo.GetById(idToDelte, It.IsAny<CancellationToken>())).ReturnsAsync(dummyApartment);

        _arpartmentRepositoryMock.Setup(repo => repo.DeleteByIdAsync(idToDelte, It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

        _arpartmentRepositoryMock
        .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);



        //Act 
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert and verify that delete and savechanges are called exactly once
        Assert.Equal(idToDelte, result);
        
        _arpartmentRepositoryMock.Verify(repo => repo.DeleteByIdAsync(idToDelte, It.IsAny<CancellationToken>()), Times.Once);
        _arpartmentRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Once);
    }

    [Fact]
    public async Task DeleteById_WithInValidId_ThrowsException()
    {
        var invalidId = 99;

        var command = new DeleteApartmentByIdCommand(invalidId);

        var handler = new DeleteApartmentByIdCommandHandler(_arpartmentRepositoryMock.Object);

        _arpartmentRepositoryMock.Setup(repo => repo.GetById(invalidId, It.IsAny<CancellationToken>())).ReturnsAsync((ApartmentEntity?)null);

        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<ItemNotFoundException>(async () => await handler.Handle(command, CancellationToken.None) );

        _arpartmentRepositoryMock.Verify(repo => repo.DeleteByIdAsync(invalidId, It.IsAny<CancellationToken>()), Times.Never);
        _arpartmentRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

    }


    // Upadate Price Tests

    [Fact]
    public async Task UpdateApartmentPrice_WithValidPrice_ReturnsID()
    {
        var updatedPrice = 145000m;
        var idToUpdate = 1;
        var dummyApartment = new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
        {
            Id = idToUpdate
        };

        var command = new UpdateApartmentPriceCommand(idToUpdate, updatedPrice);
        var handler = new UpdateApartmentPriceCommandHandler(_arpartmentRepositoryMock.Object);

        _arpartmentRepositoryMock.Setup(repo => repo.GetById(idToUpdate, It.IsAny<CancellationToken>())).ReturnsAsync(dummyApartment);
        _arpartmentRepositoryMock.Setup(repo => repo.UpdatePrice(idToUpdate, updatedPrice, It.IsAny<CancellationToken>())).ReturnsAsync(idToUpdate);
        _arpartmentRepositoryMock
            .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);


        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(idToUpdate, result);

        _arpartmentRepositoryMock.Verify(repo => repo.UpdatePrice(idToUpdate, updatedPrice, It.IsAny<CancellationToken>()), Times.Once);
        _arpartmentRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(1, 99999999)]
    [InlineData(1, 99)]
    public async Task UpdateApartmentPrice_WithInValidPrice_ReturnsID(int idToUpdate, decimal invalidPrice)
    {
       
        var dummyApartment = new ApartmentEntity("101-A", 1, ApartmentType.OneBhk, FacingDirection.North, 200000m)
        {
            Id = idToUpdate
        };

        var command = new UpdateApartmentPriceCommand(idToUpdate, invalidPrice);
        var handler = new UpdateApartmentPriceCommandHandler(_arpartmentRepositoryMock.Object);

         _arpartmentRepositoryMock.Setup(repo => repo.GetById(idToUpdate, It.IsAny<CancellationToken>())).ReturnsAsync(dummyApartment);
        _arpartmentRepositoryMock.Setup(repo => repo.UpdatePrice(idToUpdate, invalidPrice, It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidPriceException(invalidPrice));

        await Assert.ThrowsAsync<InvalidPriceException>(async () => await handler.Handle(command, CancellationToken.None));

        _arpartmentRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Never);


    }


}
