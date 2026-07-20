using E_Commerce.Domain.Contracts;

namespace E_CommerceProject.Extensions
{
    public static class WebAppExtension
    {
        public static async Task<WebApplication> IntializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            await seeder.SeeedDataAsync();
            var identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await identityseeder.SeeedDataAsync();

            return app;
        }

    }
}
