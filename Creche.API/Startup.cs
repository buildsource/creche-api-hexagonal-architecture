using Amazon.DynamoDBv2;
using Creche.API.Mappers;
using Creche.API.Options;
using Creche.Application.Handlers.Unit;
using Creche.Infrastructure.Interfaces;
using Creche.Infrastructure.Messaging;
using Creche.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Creche;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddAutoMapper(typeof(ProfileMapper));

        services.AddControllers();

        services.AddMediatR(typeof(CreateUnitCommandHandler).GetTypeInfo().Assembly);



        services.AddAWSService<IAmazonDynamoDB>();

        services.AddScoped<IUnitRepository, UnitRepository>();

        var rabbitMQOptions = Configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        services.AddSingleton<IMessageProducer>(new RabbitMQMessageProducer(rabbitMQOptions.Hostname, rabbitMQOptions.QueueName));

        var redisConfiguration = Configuration.GetSection("Redis").Get<RedisOptions>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration.ConnectionString;
            options.InstanceName = redisConfiguration.InstanceName;
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Creche API", Version = "v1" });

            // Configura o Swagger para incluir um campo de autenticação no cabeçalho
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header usando o esquema Bearer."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        if (env.IsDevelopment())
        {
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Creche API");
                //c.InjectStylesheet("/swagger-dark.css");
            });
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Creche.API running...");
            });
        });
    }
}