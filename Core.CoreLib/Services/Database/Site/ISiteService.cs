
using Core.CoreLib.Models.DTO.Library;
using Core.CoreLib.Models.DTO.Site;

namespace Core.CoreLib.Services.Database.Site
{
    public interface ISiteService
    {
        public Task<IEnumerable<GetSiteListDTO>> GetSiteListByBrand(int brandId);
        public Task<IEnumerable<GetSiteListDTO>> GetSiteListByUser(string userId);
        public Task<SiteDTO?> CreateSite(int brandId, string userId, string userObjectId, int environmentId);
        public Task<SiteDTO?> GetSite(int siteId);
        public Task<SiteDTO?> UpdateSite(int siteId, SiteDTO site);
        public Task<bool> PublishSite(int siteId, SiteDTO site);
        public Task DeleteSite(int siteId);

        //public Task<GetPageDTO> CreateSitePage(string handle, int siteId);
        public Task<PageDTO> CreateSitePage(string handle, int siteId);
        public Task<PageDTO> GetSitePage(int siteId, int pageId);
        public Task DeleteSitePage(int siteId, int pageId);
        
        public Task<SiteDTO> DuplicateSiteUseWithCaution(SiteDTO site);
        public Task<SiteDTO?> AssignSiteToNewUser(int siteId, string userEmail, string userObjectId);
        public Task HardDeleteSiteUseWithCaution(int siteId, string handle);

        //public Task<IEnumerable<PresetDTO>> GetSitePresetList(int? siteId);
        //public Task<PresetDTO> GetSitePreset(int siteId, int presetId);
        //public Task<PresetDTO> CreateSitePreset(int siteId, PresetDTO preset);
        //public Task UpdateSitePreset(int siteId, int presetId, PresetDTO preset);
        //public Task DeleteSitePreset(int siteId, int presetId);
    }
}