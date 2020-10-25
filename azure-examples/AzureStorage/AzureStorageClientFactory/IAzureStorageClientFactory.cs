using System.Threading.Tasks;
using AzureStorage.BlobStorageService;

namespace AzureStorage.AzureStorageClientFactory
{
    interface IAzureStorageClientFactory
    {
        Task<IBlobStorageService> GetBlobStorageClientAsync(string containerName);
        Task<IBlobStorageService> GetBlobStorageClientAsync(string connectionString, string containerName);
    }
}
