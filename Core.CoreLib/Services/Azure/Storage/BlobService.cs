
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Core.CoreLib.Services.Azure.Storage
{
    public class BlobService : IBlobService
    {
        protected IConfiguration _config;

        private string _storageConnectionString;

        public BlobService(
            IConfiguration config,
            string connectionStringName = "AzureStorage")
        {
            _config = config;
            _storageConnectionString =
                _config.GetConnectionString(connectionStringName) ?? string.Empty;
        }

        public async Task<T> GetDataFromBlobAsync<T>(
            BlobContainerClient containerClient,
            BlobItem blob)
        {
            var blobStreamResult =
                await containerClient.GetBlobClient(blob.Name).DownloadStreamingAsync();

            using (var reader = new StreamReader(blobStreamResult.Value.Content))
                return
                    JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }

        public BlobItem GetBlobItem(
            BlobContainerClient containerClient,
            string blobName)
        {
            return
                containerClient?.GetBlobs()?.Where(w => w.Name == blobName).FirstOrDefault() ?? null;
        }

        public async Task<bool> DeleteBlobAsync(
            BlobContainerClient containerClient,
            string blobName)
        {
            var result =
                await
                containerClient
                .GetBlobClient(blobName)
                .DeleteAsync();

            return 
                result.Status == 202;
        }

        public async Task<BlobContainerClient> GetClientContainerAsync(
            string containerName)
        {
            if (string.IsNullOrWhiteSpace(_storageConnectionString))
                throw new Exception("Azure connection string missing.");

            var clientContainer =
                new BlobServiceClient(_storageConnectionString)
                .GetBlobContainerClient(containerName);

            await clientContainer.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            return clientContainer;
        }

        public async Task<BlobClient> GetBlobClientAsync(
            string containerName,
            string fileName)
        {
            return
                (await
                GetClientContainerAsync(containerName))
                .GetBlobClient(fileName);
        }

        public async Task<(int Count, long Bytes, List<string> Names)> ListBlobContainerContentsAsync(
            string containerName)
        {
            var i = 0;
            long bytes = 0;
            var names = new List<string>();

            await foreach (var blob in
                (await GetClientContainerAsync(containerName)).GetBlobsAsync())
            {
                i++;
                bytes += blob.Properties.ContentLength.GetValueOrDefault(0);
                names.Add(blob.Name);
            }

            return (Count: i, Bytes: bytes, Names: names);
        }
    }
}