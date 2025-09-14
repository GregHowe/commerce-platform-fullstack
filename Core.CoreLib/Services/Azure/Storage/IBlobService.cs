using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Core.CoreLib.Services.Azure.Storage
{
    public interface IBlobService
    {
        Task<T> GetDataFromBlobAsync<T>(
            BlobContainerClient containerClient,
            BlobItem blob);

        Task<BlobContainerClient> GetClientContainerAsync(
            string containerName);

        Task<BlobClient> GetBlobClientAsync(
            string containerName,
            string fileName);

        Task<bool> DeleteBlobAsync(
            BlobContainerClient containerClient,
            string blobName);

        BlobItem GetBlobItem(
            BlobContainerClient containerClient,
            string blobName);

        Task<(int Count, long Bytes, List<string> Names)> ListBlobContainerContentsAsync(
            string containerName);
    }
}