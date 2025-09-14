
using Core.CoreLib.Models.DTO.Brand;
using Dapper;
using System.Data;
using Core.CoreLib.Services.Database.Base;

namespace Core.CoreLib.Services.Database.Brand
{
    public partial class BrandService : DBBase, IBrandService
    {
        public BrandService(
            DapperContext dapperContext) : base(dapperContext)
        {
        }

        public async Task<IEnumerable<GetBrandListDTO>> GetBrandList()
        {
            var brands = 
                await ExecuteQuery<GetBrandListDTO>("SELECT * FROM Brands WHERE IsDeleted = 0");

            return brands.ToList();
        }

        public async Task<GetBrandDTO> GetBrand(int brandId) =>
            (await ExecuteQuery<GetBrandDTO>("SELECT * FROM Brands WHERE Id = @BrandId", new { @BrandId = brandId })).FirstOrDefault() ?? 
            throw new Exception($"Unable to retrieve brand for {brandId}");

        public async Task<GetBrandDTO> GetBrandByHost(string host) =>
            (await ExecuteQuery<GetBrandDTO>("SELECT * FROM Brands WHERE Host = @Host", new { @Host = host })).FirstOrDefault() ??
            throw new Exception($"Unable to retrieve host for {host}");

        public async Task UpdateBrand(int brandId, UpdateBrandDTO brand)
        {
            var query = 
                @"UPDATE Brands 
                SET
                    Handle = @Handle,
                    Host = @Host,
                    Title = @Title,
                    Description = @Description,
                    Settings = @Settings
                WHERE Id = @BrandId";

            var brandParams = new DynamicParameters();
            brandParams.Add("BrandId", brandId, DbType.Int32);
            brandParams.Add("Handle", brand.Handle, DbType.String);
            brandParams.Add("Host", brand.Host, DbType.String);
            brandParams.Add("Title", brand.Title, DbType.String);
            brandParams.Add("Description", brand.Description, DbType.String);
            brandParams.Add("Settings", brand.Settings, DbType.String);

            await ExecuteQuery(query, brandParams);
        }
    }
}