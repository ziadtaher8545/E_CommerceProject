
using E_Commerce.Application;
using E_Commerce.Application.Common;
using E_Commerce.Application.Profiles;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Identity.Data.Entities;
using E_Commerce.Infrastructure.Identity.Services;
using E_CommerceProject.Extensions;
using Microsoft.Extensions.FileProviders;

namespace E_CommerceProject;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.Configure<UrlSetting>(builder.Configuration.GetSection("UrlSettings"));
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
        builder.Services.Configure<PaymentGatwaySettings>(builder.Configuration.GetSection("Stripe"));
        var app = builder.Build();
        await app.IntializeDatabaseAsync();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Files")),
            RequestPath = "/Files"
        });
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
