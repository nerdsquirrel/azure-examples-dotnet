using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using AzureStorage.BlobStorageService;

namespace AzureStorage.AzureStorageClientFactory
{
    public class AzureStorageClientFactory: IAzureStorageClientFactory
    {
        readonly BlobServiceClient _blobServiceClient;

        public AzureStorageClientFactory()
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<IBlobStorageService> GetBlobStorageClientAsync(string containerName)
        {
            var blobContainerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
            return new BlobStorageService.BlobStorageService(blobContainerClient);
        }

        public async Task<IBlobStorageService> GetBlobStorageClientAsync(string connectionString, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            return new BlobStorageService.BlobStorageService(blobContainerClient);
        }
    }
}
