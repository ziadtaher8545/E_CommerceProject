using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Data.DBContexts;
using E_Commerce.Infrastructure.Data.Repositories;
using E_Commerce.Infrastructure.DataSeeders;
using E_Commerce.Infrastructure.Identity.Data.Contexts;
using E_Commerce.Infrastructure.Identity.Data.Entities;
using E_Commerce.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.Unicode;
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
        services.AddDbContext<StoreIdentityDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
        });
        services.AddKeyedScoped<IDataSeeder, DataSeeder>("Catalog");
        services.AddKeyedScoped<IDataSeeder, IdentityDataSedder>("Identity");
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddSingleton<IConnectionMultiplexer>(opt =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);

        });
        services.AddSingleton<ICacheRepository, CacheRepository>();
        services.AddIdentityCore<ApplicationUser>()
                  .AddRoles<IdentityRole>()
                  .AddEntityFrameworkStores<StoreIdentityDbContext>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();

        var jwtsettings = configuration.GetSection("JWT").Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt Settings is Missing");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            opt.SaveToken = true;
            opt.RequireHttpsMetadata = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtsettings.Issuer,
                ValidAudience = jwtsettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.SecretKey))
            };

        });



        return services;
    }
}
