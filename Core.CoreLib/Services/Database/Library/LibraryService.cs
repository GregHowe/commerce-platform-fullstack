
using Core.CoreLib.Extensions;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.DTO.Library;
using Core.CoreLib.Models.Exceptions;
using Core.CoreLib.Services.Database.Base;

namespace Core.CoreLib.Services.Database.Library
{
    public partial class LibraryService : DBBase, ILibraryService
    {
        public LibraryService(
            DapperContext dapperContext) : base(dapperContext)
        { }

        public async Task<IEnumerable<GetThemeDTO>> GetThemeList(int brandId) =>
            (await
            ExecuteQuery<GetThemeDTO>(
                $"SELECT * FROM {DatabaseTables.Theme} WHERE IsDeleted = 0 and BrandId = {brandId}"))
            .ToList();

        public async Task<GetThemeDTO> GetTheme(int themeId) =>
            (await
            ExecuteQuery<GetThemeDTO>($"SELECT * FROM {DatabaseTables.Theme} WHERE IsDeleted = 0 AND Id = {themeId}"))
            .FirstOrDefault() ?? 
            throw new DataNotFoundException($"Unable to retrieve Theme for Id {themeId}");

        public async Task<IEnumerable<GetThemeDTO>> GetSiteThemes(int siteId) =>
            await 
            ExecuteQuery<GetThemeDTO>(
                $"SELECT {DatabaseTables.Theme}.* FROM {DatabaseTables.SiteTheme} LEFT JOIN {DatabaseTables.Theme} on {DatabaseTables.SiteTheme}.ThemeId = {DatabaseTables.Theme}.Id WHERE {DatabaseTables.SiteTheme}.SiteId = {siteId}");

        public async Task DeleteSiteThemes(int siteId) =>
            await ExecuteQuery($"DELETE FROM {DatabaseTables.SiteTheme} WHERE SiteId = {siteId}");

        public async Task CreateSiteThemes(int siteId, List<int> themeIds)
        {
            foreach (var themeId in themeIds)
                await ExecuteQuery($"INSERT INTO {DatabaseTables.SiteTheme} (SiteId, ThemeId) VALUES ({siteId}, {themeId})");
        }
        
        public async Task<IEnumerable<PresetDTO>> GetSitePresetList(int? siteId)
        {
            var presets =
                await
                ExecuteQuery<PresetDTO>($"SELECT * FROM {DatabaseTables.Preset} WHERE IsDeleted = 0 AND SiteId = @siteId",
                new { @siteId = siteId}) ??
                new List<PresetDTO>();

            // Attach Categories to Preset
            foreach (var preset in presets)
                preset.Categories =
                    (await
                    GetPresetCategories(preset.Id))
                    .ToList();

            return
                presets;
        }

        public async Task<IEnumerable<PresetDTO>> GetPresetList(int brandId)
        {
            var presets = 
                await 
                ExecuteQuery<PresetDTO>(
                    $"SELECT * FROM {DatabaseTables.Preset} WHERE IsDeleted = 0 AND BrandId = {brandId} AND SiteId IS NULL AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())");

            // Attach Categories to Preset
            foreach (var preset in presets)
                preset.Categories =
                    (await
                    GetPresetCategories(preset.Id))
                    .ToList();

            return presets.ToList();
        }

        public async Task<IEnumerable<PresetDTO>> GetPresetList(int brandId, string type) 
        {
            var presets = 
                await 
                ExecuteQuery<PresetDTO>(
                    $"SELECT * FROM {DatabaseTables.Preset} WHERE IsDeleted = 0 AND BrandId = @brandId AND SiteId IS NULL AND Type = @type AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())",
                    new 
                    { 
                        @brandId = brandId,
                        @type = type.ScrubForDB()
                    }) ??
                    throw new DataNotFoundException($"No Presets found matching Type {type}");

            // Attach Categories to Preset
            foreach (var preset in presets)
                preset.Categories =
                    (await
                    GetPresetCategories(preset.Id))
                    .ToList();

            return presets.ToList();
        }

        public async Task<IEnumerable<string>> GetPresetTypes(int brandId) =>
            (await ExecuteQuery<string>($"SELECT DISTINCT Type FROM {DatabaseTables.Preset} WHERE IsDeleted = 0 AND BrandId = {brandId} AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())"))
            .ToList();

        public async Task<PresetDTO> GetPreset(int presetId, bool includeDeleted = false)
        {
            var preset = 
                (await
                ExecuteQuery<PresetDTO>(
                    $"SELECT * FROM {DatabaseTables.Preset} WHERE {(!includeDeleted ? " IsDeleted = 0 AND " : string.Empty)} Id = {presetId} AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())"))
                .FirstOrDefault() ??
                throw new Exception($"Unable to retrieve Preset for Id {presetId}");

            // Attach Categories to Preset
            preset.Categories =
                (await
                GetPresetCategories(preset.Id))
                .ToList();

            return preset;
        }

        public async Task<PresetDTO> CreatePreset(int brandId, PresetDTO preset)
        {
            // !!! When creating a Preset for site (siteId != null), duplicate record and assign SiteId.  DO NOT UPDATE where SiteId is null with SiteId
            var query =
                @$"INSERT INTO {DatabaseTables.Preset} 
                    (Handle, BrandId, Type, Title, Description, Settings, IsPrivate, IsDeleted, SiteId, ParentPresetId, ClientCode, CreatedDt, ExpirationDt, AllowAgent, AllowDBA, AllowEagle, AllowNautilus, AllowRegisteredRep, AllowGO, AllowHO, AllowLongTermCare, AllowBusinessSolutions, AllowAARP, SMRUCode)
                    OUTPUT INSERTED.Id
                    VALUES 
                    (@handle,
                    @brandId, 
                    @type, 
                    @title, 
                    @description, 
                    @settings, 
                    @isPrivate, 
                    @isDeleted, 
                    @siteId, 
                    @parentPresetId,
                    @clientCode,
                    GETDATE(),
                    @expirationDt,
                    @allowAgent,
                    @allowDBA,
                    @allowEagle,
                    @allowNautilus,
                    @allowRegisteredRep,
                    @allowGO,
                    @allowHO,
                    @allowLongTermCare,
                    @allowBusinessSolutions,
                    @allowAARP,
                    @smruCode)";

            var presetId = 
                (await ExecuteQuery<int>(
                    query,
                    new
                    {
                        @handle = Guid.NewGuid().ToString(),
                        @brandId = brandId,
                        @type = preset.Type.ScrubForDB(),
                        @title = preset.Title.ScrubForDB(),
                        @description = preset.Description.ScrubForDB(),
                        @settings = preset.Settings.ScrubForDB(),
                        @isPrivate = preset.IsPrivate ? 1 : 0,
                        @isDeleted = 0,
                        @siteId = preset.SiteId,
                        @parentPresetId = preset.ParentPresetId,
                        @clientCode = preset.ClientCode.ScrubForDB(),
                        //@createdDt = ,
                        @expirationDt = preset.ExpirationDt,
                        @allowAgent = preset.AllowAgent ? 1 : 0,
                        @allowDBA = preset.AllowDBA ? 1 : 0,
                        @allowEagle = preset.AllowEagle ? 1 : 0,
                        @allowNautilus = preset.AllowNautilus ? 1 : 0,
                        @allowRegisteredRep = preset.AllowRegisteredRep ? 1 : 0,
                        @allowGO = preset.AllowGO ? 1 : 0,
                        @allowHO = preset.AllowHO ? 1 : 0,
                        @allowLongTermCare = preset.AllowLongTermCare ? 1 : 0,
                        @allowBusinessSolutions = preset.AllowBusinessSolutions ? 1 : 0,
                        @allowAARP = preset.AllowAARP ? 1 : 0,
                        @smruCode = preset.SMRUCode.ScrubForDB()
                    }))
                    .FirstOrDefault();

            if (presetId == 0)
                throw new Exception($"Create Category failed for category: {preset.Title}");

            if (preset.CategoryIds != null && preset.CategoryIds.Count > 0)
                await CreatePresetCategory(presetId, preset.CategoryIds);

            return await GetPreset(presetId);
        }

        //public async Task<PresetDTO> UpdatePreset(string data)
        //{
        //    await
        //    ExecuteQuery<int>(
        //    @$"UPDATE {DatabaseTables.Preset} SET
        //        Title = @Title
        //    WHERE Id = 1176", 
        //    new { @Title = data}
        //    );
        //    // I think we can't chagne siteId on Update, CL has no siteId, Agent can't span sites.  You must create within the context of a site to begin with
        //    //SiteId = {(preset.SiteId.HasValue ? preset.SiteId.Value : "NULL")},

        //    //await CreatePresetCategory(preset.Id, preset.Categories);

        //    return await GetPreset(1176);
        //}

        private async Task CreatePresetCategory(int presetId, List<int> categorieIds)
        {
            // DELETE join table entries for this preset
            await
            ExecuteQuery($"DELETE FROM {DatabaseTables.PresetCategory} WHERE PresetId = {presetId}");

            // TODO: Move to bulk operation
            foreach (var id in categorieIds)
                // Create PrestCategory join table entry
                await ExecuteQuery($"INSERT INTO {DatabaseTables.PresetCategory} (PresetId, CategoryId) VALUES ({presetId}, {id})");
        }

        public async Task DeletePreset(int presetId) =>
            await ExecuteQuery($"UPDATE {DatabaseTables.Preset} SET IsDeleted = 1 WHERE Id = {presetId} OR ParentPresetId = {presetId}");

        private async Task<IEnumerable<CategoryDTO>> GetPresetCategories(int presetId) =>
            await ExecuteQuery<CategoryDTO>(
                $"SELECT [Category].* FROM {DatabaseTables.Category} WHERE Id IN (SELECT CategoryId FROM {DatabaseTables.PresetCategory} WHERE PresetId = {presetId})") ??
                new List<CategoryDTO>();

        public async Task<IEnumerable<CategoryDTO>> GetCategoryListByType(int brandId, string type) => 
            (await ExecuteQuery<CategoryDTO>(
                $"SELECT * FROM {DatabaseTables.Category} WHERE IsDeleted = 0 AND BrandId = @brandId AND Type = @type AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())",
                new
                { 
                    @brandId = brandId,
                    @type = type.ScrubForDB()
                }))
            .ToList();
            
        public async Task<IEnumerable<CategoryDTO>> GetCategoryList(int brandId) =>
            (await 
            ExecuteQuery<CategoryDTO>($"SELECT * FROM {DatabaseTables.Category} WHERE IsDeleted = 0 AND BrandId = {brandId} AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())"))
            .ToList();

        public async Task<IEnumerable<string>> GetCategoryTypes(int brandId) =>
            (await ExecuteQuery<string>($"SELECT DISTINCT Type FROM {DatabaseTables.Category} WHERE IsDeleted = 0 AND BrandId = {brandId} AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())"))
            .ToList();

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesByType(string type, int brandId) =>
            (await ExecuteQuery<CategoryDTO>($"SELECT * FROM {DatabaseTables.Category} WHERE IsDeleted = 0 AND BrandId = @brandId AND Type = @type AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())",
                new
                { 
                    @brandId = brandId,
                    @type = type.ScrubForDB()
                }))
            .ToList();

        public async Task<CategoryDTO> GetCategory(int categoryId) =>
            (await ExecuteQuery<CategoryDTO>(
                $"SELECT * FROM {DatabaseTables.Category} WHERE Id = {categoryId} AND IsDeleted = 0 AND (ExpirationDt IS NULL OR ExpirationDt > GETDATE())"))
            .FirstOrDefault() ??
            throw new DataNotFoundException($"Unable to retrieve Category for Id {categoryId}");

        public async Task<CategoryDTO> CreateCategory(int brandId, CategoryDTO category) 
        {
            var categoryId =
                (await 
                ExecuteQuery<int>(
                    $@"INSERT INTO {DatabaseTables.Category} 
                    (BrandId, Type, Title, Description, Keywords, ParentCategoryId, IsDeleted, ExpirationDt)
                    OUTPUT INSERTED.Id
                    VALUES (
                        @brandId, 
                        @type,
                        @title,
                        @description,
                        @keywords,
                        @parentCategoryId, 
                        @isDeleted, 
                        @expirationDt)",
                        new
                        {
                            @brandId = brandId,
                            @type = category.Type.ScrubForDB(),
                            @title = category.Title.ScrubForDB(),
                            @description = category.Description.ScrubForDB(),
                            @keywords = category.Keywords.ScrubForDB(),
                            @parentCategoryId = category.ParentCategoryId,
                            @isDeleted = 0,
                            @expirationDt = category.ExpirationDt
                        }))
                .FirstOrDefault();

            if (categoryId == 0)
                throw new Exception($"Create Category failed for category: {category?.Title ?? "No title provided"}");

            return await GetCategory(categoryId);
        }

        public async Task<CategoryDTO> UpdateCategory(int brandId, CategoryDTO category) 
        {
            await ExecuteQuery(
                $@"UPDATE {DatabaseTables.Category} 
                SET
                    Title = @title,
                    Description = @description,
                    ParentCategoryId = @parentCategoryId,
                    Keywords = @keywords,
                    ExpirationDt = @expirationDt
                WHERE Id = @categoryId",
                new
                { 
                    @title = category.Title.ScrubForDB(),
                    @description = category.Description.ScrubForDB(),
                    @parentCategoryId = category.ParentCategoryId,
                    @keywords = category.Keywords.ScrubForDB(),
                    @expirationDt = category.ExpirationDt,
                    @categoryId = category.Id
                });

            return await GetCategory(category.Id);
        }

        public async Task DeleteCategory(int categoryId) =>
            await ExecuteQuery($"UPDATE {DatabaseTables.Category} SET IsDeleted = 1 WHERE Id = {categoryId}");
    }
}