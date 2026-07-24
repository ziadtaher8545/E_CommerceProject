using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Models.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Repositories
{
    internal class BasketRepository : IBasketRepository

          
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();

        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(basket);
            var result = await _database.StringSetAsync(basket.Id, value, timeToLive ?? TimeSpan.FromHours(1));
            return result ? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _database.StringGetAsync(basketId);
            return result.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(result!);
        }
    }
}
