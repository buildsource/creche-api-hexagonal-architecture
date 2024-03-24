using Creche.Application.Commands.Unit;
using Creche.Application.DTOs;
using Creche.Application.Entities;
using Creche.Application.Handlers.Unit;
using Creche.Application.Queries.Unit;
using Creche.Application.Responses;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Creche.Tests.IntegrationTest;

public class UnitIntegrationTests : IClassFixture<ApiTestFixture>
{
    private readonly ApiTestFixture _fixture;

    public UnitIntegrationTests(ApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateUnitCommand_ShouldAddUnit_AndCacheIt()
    {
        // Preparação
        var unitDto = new UnitDto
        {
            Name = "Test Unit",
            Email = "unit@test.com",
            Phone = "+1234567890",
            Director = "Test Director"
        };

        var command = new CreateUnitCommand(unitDto);

        var response = await _fixture.Mediator.Send(command);

        // Verificar se a unidade foi criada no DynamoDB
        var createdUnit = await _fixture.UnitRepository.GetUnitByIdAsync(response.PK);
        Assert.NotNull(createdUnit);
        Assert.Equal(unitDto.Name, createdUnit.Name);

        // Verificar se a unidade foi cacheada no Redis
        var cachedUnit = await _fixture.DistributedCache.GetStringAsync(response.PK);
        Assert.NotNull(cachedUnit);
    }

    [Fact]
    public async Task GetUnitById_ShouldRetrieveUnitFromDynamoDB_AndCacheIt_WhenNotCached()
    {
        // Configuração do ambiente de teste (assumindo que ApiTestFixture faz isso)
        var testFixture = new ApiTestFixture();

        var unitEntity = new UnitEntity
        {
            PK = Guid.NewGuid().ToString(),
            Name = "Test Unit",
            Email = "test@unit.com",
            Phone = "+123456789",
            Director = "Director Name"
        };

        await testFixture.UnitRepository.CreateUnitAsync(unitEntity);

        // Limpar o cache para garantir que a unidade será recuperada do DynamoDB
        await testFixture.DistributedCache.RemoveAsync(unitEntity.PK);

        // Executar o GetUnitByIdQuery para recuperar a unidade
        var getUnitQuery = new GetUnitByIdQuery(unitEntity.PK);
        var handler = new GetUnitByIdCommandHandler(testFixture.Mapper, testFixture.UnitRepository, testFixture.DistributedCache);
        var retrievedUnitResponse = await handler.Handle(getUnitQuery, CancellationToken.None);

        // Verificar se a unidade recuperada corresponde à unidade inserida
        Assert.NotNull(retrievedUnitResponse);
        Assert.Equal(unitEntity.Name, retrievedUnitResponse.Name);
        Assert.Equal(unitEntity.Email, retrievedUnitResponse.Email);
        Assert.Equal(unitEntity.Phone, retrievedUnitResponse.Phone);
        Assert.Equal(unitEntity.Director, retrievedUnitResponse.Director);

        // Verificar se a unidade foi cacheada
        var cachedUnitJson = await testFixture.DistributedCache.GetStringAsync(unitEntity.PK);
        Assert.NotNull(cachedUnitJson);
        var cachedUnit = JsonSerializer.Deserialize<UnitResponse>(cachedUnitJson);
        Assert.NotNull(cachedUnit);
        Assert.Equal(unitEntity.Name, cachedUnit.Name);
        Assert.Equal(unitEntity.Email, cachedUnit.Email);
        Assert.Equal(unitEntity.Phone, cachedUnit.Phone);
        Assert.Equal(unitEntity.Director, cachedUnit.Director);
    }
}