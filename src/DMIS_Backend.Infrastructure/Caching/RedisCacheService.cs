using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DMIS_Backend.Infrastructure.Caching;

/// <summary>
/// Redis 快取服務
/// </summary>
public class RedisCacheService : ICacheService
{
  private readonly IDistributedCache _distributedCache;

  public RedisCacheService(IDistributedCache distributedCache)
  {
    _distributedCache = distributedCache;
  }

  public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
      where T : class
  {
    var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
    if (string.IsNullOrEmpty(cachedValue))
    {
      return null;
    }

    return JsonSerializer.Deserialize<T>(cachedValue);
  }

  public async Task SetAsync<T>(
      string key,
      T value,
      DistributedCacheEntryOptions? options = null,
      CancellationToken cancellationToken = default)
      where T : class
  {
    var serializedValue = JsonSerializer.Serialize(value);
    await _distributedCache.SetStringAsync(key, serializedValue, options ?? new DistributedCacheEntryOptions(), cancellationToken);
  }

  public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
  {
    await _distributedCache.RemoveAsync(key, cancellationToken);
  }
}

/// <summary>
/// 快取服務介面
/// </summary>
public interface ICacheService
{
  Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
  Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default) where T : class;
  Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
