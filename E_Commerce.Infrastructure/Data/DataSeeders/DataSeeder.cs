using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models;
using E_Commerce.Infrastructure.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.DataSeeders
{
    internal class DataSeeder(StoreDBContext _dBContext, ILogger<DataSeeder> logger) : IDataSeeder
    {
        public async Task SeeedDataAsync(CancellationToken ct = default)
        {
            var pendingMigration = await _dBContext.Database.GetPendingMigrationsAsync(ct);
            if (pendingMigration.Any())
                await _dBContext.Database.MigrateAsync(ct);
            var rootpath = Path.Combine(AppContext.BaseDirectory, "SeedFiles");
            await SeedData<ProductType, int>(rootpath, "types.json", ct);
            await SeedData<ProductBrand, int>(rootpath, "brands.json", ct);
            await SeedData<Product, int>(rootpath, "products.json", ct);
        }
        private async Task SeedData<TEntity, Tkey>(string rootpath, string FileName, CancellationToken ct) where TEntity : BaseEntity<Tkey>, new()
        {
            if (await _dBContext.Set<TEntity>().AnyAsync(ct))
            {
                logger.LogInformation($"{typeof(TEntity).Name}s Are Arealdy Seeded");
                return;
            }
            var filePath = Path.Combine(rootpath, FileName);
            if (!File.Exists(filePath))
            {
                logger.LogInformation($"{filePath} Wasn't Found");
                return;
            }
            using var fileStreem = File.OpenRead(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,

            };
            var items = await JsonSerializer.DeserializeAsync<List<TEntity>>(fileStreem, options, ct);
            if (!items?.Any() ?? false)
                return;

            _dBContext.Set<TEntity>().AddRange(items!);

            var result = await _dBContext.SaveChangesAsync(ct);
            if (result > 0)
                logger.LogInformation($"{result} {typeof(TEntity).Name} Seeded");



        }
    }

}
