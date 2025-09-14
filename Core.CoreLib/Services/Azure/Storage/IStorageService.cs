using Core.CoreLib.Models.Azure.Blob;

namespace Core.CoreLib.Services.Azure.Storage
{
    public interface IStorageService
    {
        Task<BlobResponseDTO> UploadMediaAsync(int brandId, string fileName, Stream data);

        Task<BlobResponseDTO> UploadSiteMediaAsync(int brandId, int siteId, string fileName, Stream data);

        Task<bool> DeleteMedia(int brandId, string fileName);
    }
}