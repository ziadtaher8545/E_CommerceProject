using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;
        public CacheService(ICacheRepository repository)
        {
            _cacheRepository = repository;
        }



        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        => await _cacheRepository.GetAsync(cacheKey, ct);

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive, CancellationToken ct = default)
        {
            var result = JsonSerializer.Serialize(cacheValue);
            await _cacheRepository.SetAsync(cacheKey, result, timeToLive, ct);
        }
    }
}
