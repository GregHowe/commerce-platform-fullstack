using Core.CoreLib.Models.DTO.Brand;

namespace Core.CoreLib.Services.Database.Brand
{
    public interface IBrandService
    {
        public Task<IEnumerable<GetBrandListDTO>> GetBrandList();
        public Task<GetBrandDTO> GetBrand(int brandId);
        public Task<GetBrandDTO> GetBrandByHost(string host);
        public Task UpdateBrand(int brandId, UpdateBrandDTO brand);
    }
}