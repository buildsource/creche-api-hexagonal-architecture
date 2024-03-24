using AutoMapper;
using Creche.Application.Commands.Unit;
using Creche.Application.DTOs;
using Creche.Application.Entities;
using Creche.Application.Handlers.Unit;
using Creche.Application.Queries.Unit;
using Creche.Application.Responses;
using Creche.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;

namespace Creche.Tests.UnitTest;

public class UnitTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitRepository> _unitRepositoryMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IMessageProducer> _producerMock;
    private readonly CreateUnitCommandHandler _createUnitHandler;
    private readonly GetUnitByIdCommandHandler _getUnitByIdHandler;

    //Arrange
    private readonly UnitDto unitDto = new UnitDto
    {
        Name = "Ben Fonseca",
        Email = "teste@teste.com",
        Phone = "+55991993395",
        Director = "Carsos Silva"
    };
    private readonly UnitEntity unitEntity = new UnitEntity
    {
        PK = Guid.NewGuid().ToString(),
        Name = "Ben Fonseca",
        Email = "teste@teste.com",
        Phone = "+55991993395",
        Director = "Carsos Silva"
    };
    private readonly UnitResponse unitResponse = new UnitResponse
    {
        PK = Guid.NewGuid().ToString(),
        Name = "Ben Fonseca",
        Email = "teste@teste.com",
        Phone = "+55991993395",
        Director = "Carsos Silva"
    };

    public UnitTests()
    {
        _mapperMock = new Mock<IMapper>();
        _unitRepositoryMock = new Mock<IUnitRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _producerMock = new Mock<IMessageProducer>();
        _createUnitHandler = new CreateUnitCommandHandler(_mapperMock.Object, _unitRepositoryMock.Object, _producerMock.Object, _cacheMock.Object);
        _getUnitByIdHandler = new GetUnitByIdCommandHandler(_mapperMock.Object, _unitRepositoryMock.Object, _cacheMock.Object);
    }

    [Fact]
    public async Task CreateUnitCommand_ShouldReturnUnitResponse()
    {
        var command = new CreateUnitCommand(unitDto);

        _mapperMock.Setup(m => m.Map<UnitEntity>(It.IsAny<UnitDto>())).Returns(unitEntity);
        _unitRepositoryMock.Setup(repo => repo.CreateUnitAsync(It.IsAny<UnitEntity>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<UnitResponse>(It.IsAny<UnitEntity>())).Returns(unitResponse);

        // Act
        var result = await _createUnitHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(unitResponse, result);
        _mapperMock.Verify(mapper => mapper.Map<UnitEntity>(unitDto), Times.Once());
        _unitRepositoryMock.Verify(repo => repo.CreateUnitAsync(unitEntity), Times.Once());
        _mapperMock.Verify(mapper => mapper.Map<UnitResponse>(unitEntity), Times.Once());
    }

    [Fact]
    public async Task GetUnitByIdQuery_ShouldReturnUnitResponse_FromCache()
    {
        // Arrange
        var unitId = unitResponse.PK;
        var serializedCachedUnitResponse = JsonSerializer.Serialize(unitResponse);
        var query = new GetUnitByIdQuery(unitId);

        _cacheMock.Setup(cache => cache.GetAsync(unitId, It.IsAny<CancellationToken>()))
          .ReturnsAsync(Encoding.UTF8.GetBytes(serializedCachedUnitResponse));

        // Act
        var result = await _getUnitByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(unitResponse.PK, result.PK);
        Assert.Equal(unitResponse.Name, result.Name);
        Assert.Equal(unitResponse.Email, result.Email);
        Assert.Equal(unitResponse.Phone, result.Phone);
        Assert.Equal(unitResponse.Director, result.Director);

        _cacheMock.Verify(cache => cache.GetAsync(unitId, It.IsAny<CancellationToken>()), Times.Once());
        _unitRepositoryMock.Verify(repo => repo.GetUnitByIdAsync(It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task GetUnitByIdQuery_ShouldReturnUnitResponse_FromRepository_AndSetCache()
    {
        // Arrange
        var unitId = unitResponse.PK;
        var query = new GetUnitByIdQuery(unitId);

        _cacheMock.Setup(cache => cache.GetAsync(unitId, It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _unitRepositoryMock.Setup(repo => repo.GetUnitByIdAsync(unitId))
                           .ReturnsAsync(unitEntity);
        _mapperMock.Setup(mapper => mapper.Map<UnitResponse>(It.IsAny<UnitEntity>()))
                   .Returns(unitResponse);

        string serializedUnitResponse = JsonSerializer.Serialize(unitResponse);

        _cacheMock.Setup(cache => cache.SetAsync(
            unitId,
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            It.IsAny<CancellationToken>()
        )).Returns(Task.CompletedTask);


        // Act
        var result = await _getUnitByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(unitResponse.PK, result.PK);
        Assert.Equal(unitResponse.Name, result.Name);
        Assert.Equal(unitResponse.Email, result.Email);
        Assert.Equal(unitResponse.Phone, result.Phone);
        Assert.Equal(unitResponse.Director, result.Director);

        _unitRepositoryMock.Verify(repo => repo.GetUnitByIdAsync(unitId), Times.Once());
        _mapperMock.Verify(mapper => mapper.Map<UnitResponse>(unitEntity), Times.Once());
        _cacheMock.Verify(cache => cache.SetAsync(
            unitId,
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            It.IsAny<CancellationToken>()
        ), Times.Once());

    }
}