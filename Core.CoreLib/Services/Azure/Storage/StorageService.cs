
using Azure;
using Azure.Storage.Blobs.Models;
using Core.CoreLib.Models.Azure.Blob;
using Core.CoreLib.Models.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Core.CoreLib.Services.Azure.Storage
{
    public partial class StorageService : IStorageService
    {
        const string AzureContainerName = "$web";
        const string SiteAgnosticContainerPath = "/brands/{brandId}/uploads";
        const string SiteSpecificContainerPath = "/brands/{brandId}/websites/{siteId}/uploads";
        const string CDNRootUrl = "https://f92core-nylwebsites.azureedge.net/";

        protected IBlobService _blobService;
        protected IConfiguration _configuration;

        public StorageService(
            IConfiguration configuration,
            IBlobService blobService)
        {
            _blobService = blobService;
            _configuration = configuration;
        }

        private async Task<BlobResponseDTO> UploadBlobAsync(
            string storageContainerPath,
            string fileName,
            Stream data)
        {
            // Create new upload response object that we can return to the requesting method
            var response = new BlobResponseDTO();

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                var client =
                    await
                    _blobService
                    .GetBlobClientAsync(
                        AzureContainerName,
                        $"{storageContainerPath}/{fileName}");

                using (data)
                    await client.UploadAsync(data, true);

                response.Status = $"File {fileName} uploaded successfully";
                response.Blob.Uri = $"{CDNRootUrl}{client.Name.Replace("brands/", "")}"; 
                response.Blob.Name = client.Name;
                response.Blob.FileName = fileName;
                response.Blob.BlobUri = client.Uri.AbsoluteUri;
            
                return response;
            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                throw new Exception($"File with name {fileName} already exists in container. Set another name to store the file in the container: '{storageContainerPath}.'", ex);
            }
            catch (RequestFailedException ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMedia(
            int brandId,
            string fileName)
        {
            var client =
                await
                _blobService
                .GetClientContainerAsync(AzureContainerName);
            
            return
                await
                _blobService
                .DeleteBlobAsync(
                    client,
                    $"{SiteAgnosticContainerPath.Replace("{brandId}", brandId.ToString())}/{fileName}");
        }

        public async Task<bool> DeleteSiteMedia(
           int brandId,
           int siteId,
           string fileName)
        {
            var client =
                await
                _blobService
                .GetClientContainerAsync(AzureContainerName);
            
            return
                await
                _blobService
                .DeleteBlobAsync(
                    client,
                    $"{SiteSpecificContainerPath.Replace("{brandId}", brandId.ToString()).Replace("{siteid}", siteId.ToString())}/{fileName}");
        }

        public async Task<BlobResponseDTO> UploadMediaAsync(
            int brandId,
            string fileName,
            Stream data)
        {
            return
                await
                UploadBlobAsync(
                    SiteAgnosticContainerPath.Replace("{brandId}", brandId.ToString()), 
                    fileName, 
                    data);
        }

        public async Task<BlobResponseDTO> UploadSiteMediaAsync(
            int brandId,
            int siteId,
            string fileName,
            Stream data)
        {
            if (siteId == 0)
                throw new BadRequestException($"Invalid site id: {siteId}");

            return
                await
                UploadBlobAsync(
                    $"{SiteSpecificContainerPath.Replace("{brandId}", brandId.ToString()).Replace("{siteid}", siteId.ToString())}",
                    fileName, 
                    data);
        }
    }
}