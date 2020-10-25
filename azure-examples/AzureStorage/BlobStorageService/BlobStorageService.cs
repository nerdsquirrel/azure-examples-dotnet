using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureStorage.BlobStorageService
{
    // https://github.com/Azure/azure-sdk-for-net/tree/Azure.Storage.Blobs_12.6.0/sdk/storage/Azure.Storage.Blobs/samples
    // https://docs.microsoft.com/en-us/dotnet/api/overview/azure/storage.blobs-readme
    // https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
    public class BlobStorageService : IBlobStorageService
    {
        readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient ?? throw new ArgumentNullException($"{nameof(blobContainerClient)} can't be null");
        }

        public async Task<bool> IsFileExistsInBlobAsync(string fileName)
        {
            if (!await _blobContainerClient.ExistsAsync())
                return false;
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            return await blobClient.ExistsAsync();
        }

        public async Task<BlobProperties> GetBlobPropertiesAsync(string fileName)
        {
            if (!await _blobContainerClient.ExistsAsync())
                return null;
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            return await blobClient.GetPropertiesAsync();
        }

        public async Task<bool> UploadFromFilePathAsync(string blobName, string filePath)
        {
            if (!await _blobContainerClient.ExistsAsync())
                await _blobContainerClient.CreateAsync();
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            try
            {
                await blobClient.UploadAsync(filePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DownloadToFileAsync(string blobName, string downloadPath)
        {
            if (!await _blobContainerClient.ExistsAsync())
                return false;
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            try
            {
                var downloadInfo = await blobClient.DownloadAsync();
                using (var file = File.OpenWrite(downloadPath))
                {
                    await downloadInfo.Value.Content.CopyToAsync(file);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
