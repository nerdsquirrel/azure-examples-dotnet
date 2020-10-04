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

        public Result<string> GetValue(string key)
        {
            try
            {
                return Result.Ok<string>(MultiplexerDatabase.StringGet(key));
            }
            catch (Exception ex)
            {
                return Result.Fail<string>($"Error getting value from azure cache. Exception: {ex.Message}");
            }
        }

        public Result<byte[]> GetBinaryValue(string key)
        {
            try
            {
                return Result.Ok<byte[]>(MultiplexerDatabase.StringGet(key));
            }
            catch (Exception ex)
            {
                return Result.Fail<byte[]>($"Error getting value from azure cache. Exception: {ex.Message}");
            }
        }

        public Result<bool> SetCache(string key, string value, TimeSpan? expiry)
        {
            try
            {
                return Result.Ok(MultiplexerDatabase.StringSet(key, value, expiry));
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>($"Error to set value in azure cache. Exception: {ex.Message}");
            }
        }

        public Result<bool> SetCache(string key, byte[] value, TimeSpan? expiry)
        {
            try
            {
                return Result.Ok(MultiplexerDatabase.StringSet(key, value, expiry));
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>($"Error to set value in azure cache. Exception: {ex.Message}");
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
                return Result.Fail<bool>($"Error to communicate with azure cache. Exception: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _lazyConnection?.Value?.Dispose();
        }
    }
}
