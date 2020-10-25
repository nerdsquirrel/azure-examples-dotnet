using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace AzureStorage.BlobStorageService
{
    public interface IBlobStorageService
    {
        Task<bool> IsFileExistsInBlobAsync(string fileName);
        Task<BlobProperties> GetBlobPropertiesAsync(string fileName);
        Task<bool> UploadFromFilePathAsync(string blobName, string filePath);
        Task<bool> DownloadToFileAsync(string blobName, string downloadPath);
    }
}
