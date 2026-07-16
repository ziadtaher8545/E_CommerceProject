using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Data.DataSeeders;
using E_Commerce.Infrastructure.Data.DBContexts;
using E_Commerce.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure;

public static class InfrastructureServicesRegistration
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreDBContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddKeyedScoped<IDataSeeder, DataSeeder>("Catalog");
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddSingleton<IConnectionMultiplexer>(opt =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);

        });
        services.AddSingleton<ICacheRepository, CacheRepository>();
        return services;
    }
}
