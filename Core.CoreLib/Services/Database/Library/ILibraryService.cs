
using Core.CoreLib.Models.DTO.Library;

namespace Core.CoreLib.Services.Database.Library
{
    public interface ILibraryService
    {
        public Task<IEnumerable<CategoryDTO>> GetCategoryList(int brandId);
        public Task<CategoryDTO> GetCategory(int categoryId);
        public Task<IEnumerable<string>> GetCategoryTypes(int brandId);
        public Task<IEnumerable<CategoryDTO>> GetCategoriesByType(string type, int brandId);
        public Task<CategoryDTO> CreateCategory(int brandId, CategoryDTO category);
        public Task<CategoryDTO> UpdateCategory(int brandId, CategoryDTO category);
        public Task DeleteCategory(int categoryId);

        public Task<IEnumerable<GetThemeDTO>> GetSiteThemes(int siteId);
        public Task<IEnumerable<GetThemeDTO>> GetThemeList(int brandId);
        public Task<GetThemeDTO> GetTheme(int themeId);
        public Task CreateSiteThemes(int siteId, List<int> themeIds);
        public Task DeleteSiteThemes(int siteId);

        public Task<IEnumerable<PresetDTO>> GetSitePresetList(int? siteId);
        public Task<IEnumerable<PresetDTO>> GetPresetList(int brandId);
        public Task<IEnumerable<PresetDTO>> GetPresetList(int brandId, string type);
        public Task<IEnumerable<string>> GetPresetTypes(int brandId);
        public Task<PresetDTO> GetPreset(int presetId, bool includeDeleted = false);
        public Task<PresetDTO> CreatePreset(int brandId, PresetDTO preset);
        //Task<PresetDTO> UpdatePreset(string data);
        public Task DeletePreset(int presetId);
    }
}