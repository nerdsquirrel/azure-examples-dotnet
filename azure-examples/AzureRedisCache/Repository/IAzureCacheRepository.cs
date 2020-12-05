using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace AzureRedisCache.Repository
{
    public interface IAzureCacheRepository
    {
        Task<Result<string>> GetValueAsync(string key);
        Task<Result<byte[]>> GetBinaryValueAsync(string key);
        Task<Result<bool>> SetCacheAsync(string key, string value, TimeSpan? expiry);
        Task<Result<bool>> SetCacheAsync(string key, byte[] value, TimeSpan? expiry);
        Task<Result<bool>> IsKeyExistsAsync(string key);
        Task<Result<bool>> RemoveKeyAsync(string key);
    }
}