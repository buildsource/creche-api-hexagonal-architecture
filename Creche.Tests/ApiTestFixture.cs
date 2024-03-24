using Amazon.DynamoDBv2;
using AutoMapper;
using Creche.Application.Handlers.Unit;
using Creche.Infrastructure.Interfaces;
using Creche.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Creche.Tests;

public class ApiTestFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }
    public IMapper Mapper => ServiceProvider.GetRequiredService<IMapper>();
    public IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();
    public IUnitRepository UnitRepository => ServiceProvider.GetRequiredService<IUnitRepository>();
    public IDistributedCache DistributedCache => ServiceProvider.GetRequiredService<IDistributedCache>();

    public ApiTestFixture()
    {
        var services = new ServiceCollection();

        // Carregar configurações
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var assemblyPath = Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(assemblyPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Configurar AWS DynamoDB
        services.AddAWSService<IAmazonDynamoDB>(configuration.GetAWSOptions());

        // Configurar MediatR
        services.AddMediatR(typeof(CreateUnitCommandHandler).GetTypeInfo().Assembly);

        // Configurar Redis (usando cache em memória para simulação em testes)
        services.AddSingleton<IDistributedCache, MemoryDistributedCache>();

        // Configurar o mock do IMessageProducer
        var messageProducerMock = new Mock<IMessageProducer>();
        services.AddSingleton<IMessageProducer>(messageProducerMock.Object);

        // Configurar o repositório (assumindo que UnitRepository utilize IAmazonDynamoDB e/ou IDistributedCache)
        services.AddSingleton<IUnitRepository, UnitRepository>();

        services.AddAutoMapper(typeof(Startup).Assembly);

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        (ServiceProvider as IDisposable)?.Dispose();
    }
}