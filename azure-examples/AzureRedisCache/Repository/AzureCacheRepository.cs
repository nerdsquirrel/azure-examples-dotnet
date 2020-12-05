using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using StackExchange.Redis;

namespace AzureRedisCache.Repository
{
    public class AzureCacheRepository : IAzureCacheRepository, IDisposable
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private IDatabase MultiplexerDatabase => _lazyConnection.Value.GetDatabase();

        public AzureCacheRepository()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("secret-key"));
        }

        public async Task<Result<string>> GetValueAsync(string key)
        {
            try
            {
                return Result.Ok<string>(await MultiplexerDatabase.StringGetAsync(key));
            }
            catch (Exception ex)
            {
                _logger.Log(new SFCommonException($"Error getting value from azure cache.{ex.GetExceptionDetailMessage()}", ex));
                return Result.Fail<string>("Error getting value from azure cache");
            }
        }

        public async Task<Result<byte[]>> GetBinaryValueAsync(string key)
        {
            try
            {
                return Result.Ok<byte[]>(await MultiplexerDatabase.StringGetAsync(key));
            }
            catch (Exception ex)
            {
                _logger.Log(new SFCommonException($"Error getting value from azure cache.{ex.GetExceptionDetailMessage()}", ex));
                return Result.Fail<byte[]>("Error getting value from azure cache");
            }
        }

        public async Task<Result<bool>> SetCacheAsync(string key, string value, TimeSpan? expiry)
        {
            try
            {
                return Result.Ok(await MultiplexerDatabase.StringSetAsync(key, value, expiry));
            }
            catch (Exception ex)
            {
                _logger.Log(new SFCommonException($"Error to set value in azure cache. {ex.GetExceptionDetailMessage()}", ex));
                return Result.Fail<bool>("Error to set value in azure cache.");
            }
        }

        public async Task<Result<bool>> SetCacheAsync(string key, byte[] value, TimeSpan? expiry)
        {
            try
            {
                return Result.Ok(await MultiplexerDatabase.StringSetAsync(key, value, expiry));
            }
            catch (Exception ex)
            {
                _logger.Log(new SFCommonException($"Error to set value in azure cache. {ex.GetExceptionDetailMessage()}", ex));
                return Result.Fail<bool>("Error to set value in azure cache.");
            }
        }

        public async Task<Result<bool>> RemoveKeyAsync(string key)
        {
            try
            {
                return Result.Ok(await MultiplexerDatabase.KeyDeleteAsync(key));
            }
            catch (Exception ex)
            {
                _logger.Log(new SFCommonException($"Error to remove key from azure cache. {ex.GetExceptionDetailMessage()}", ex));
                return Result.Fail<bool>("Error to remove key from azure cache.");
            }
        }        

        public async Task<Result<bool>> IsKeyExistsAsync(string key)
        {
            try
            {
                return Result.Ok(await MultiplexerDatabase.KeyExistsAsync(key));
            }
            catch (Exception ex)
            {
                _logger.Log(new SFCommonException($"Error to communicate with azure cache. {ex.GetExceptionDetailMessage()}", ex));
                return Result.Fail<bool>("Error to communicate with azure cache.");
            }
        }

        public void Dispose()
        {
            _lazyConnection?.Value?.Dispose();
        }
    }
}
