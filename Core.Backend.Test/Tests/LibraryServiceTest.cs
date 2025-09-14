
using Core.CoreLib.Models.DTO.Library;
using Core.CoreLib.Models.Exceptions;
using Core.CoreLib.Services.Database;
using Core.CoreLib.Services.Database.Library;

namespace Core.Backend.Test.Tests
{
    public class LibraryServiceTest : TestBase
    {
        private LibraryService _libraryService;

        public LibraryServiceTest()
        {
            // Scaffold
            var _config = CreateConfig();

            CreateServices(_config);

            _libraryService = 
                new LibraryService(new DapperContext(_config));
        }

        [Fact]
        public async Task CategoryTests()
        {
            var brandId = 3;
            const string UpdatedTitle = "Updated Title";
            const string TypeThatWillFail = "FAIL";

            var types =
                await
                _libraryService
                .GetCategoryTypes(brandId);

            Assert.NotNull(types);
            Assert.True(types.Count() > 0);

            var categoriesByType =
                await
                _libraryService
                .GetCategoriesByType(types.FirstOrDefault() ?? TypeThatWillFail, brandId);

            Assert.NotNull(categoriesByType);
            Assert.True(categoriesByType.Count() > 0);

            var category =
                await
                _libraryService
                .CreateCategory(
                    brandId,
                    new CategoryDTO()
                    { 
                        Title = "OK To Delete",
                        Type = types.FirstOrDefault() ?? TypeThatWillFail,
                        Description = "Unit Test",
                        Keywords = "Keyword1,Keyword2,Keyword6",
                        BrandId = brandId,
                        ParentCategoryId = null,
                        IsDeleted = false,
                        ExpirationDt = DateTime.Now.AddYears(1)
                    });

            Assert.NotNull(category);
            Assert.True(category.Id > 0);

            category.Title = UpdatedTitle;

            var updatedCategory =
                await
                _libraryService
                .UpdateCategory(brandId, category);

            Assert.NotNull(updatedCategory);
            Assert.True(updatedCategory.Title == UpdatedTitle);

            var getCategory =
                await
                _libraryService
                .GetCategory(updatedCategory.Id);

            Assert.NotNull(getCategory);
            Assert.True(updatedCategory.Id == getCategory.Id);

            var categoryList =
                await
                _libraryService
                .GetCategoryList(brandId);

            Assert.NotNull(categoryList);
            Assert.True(categoryList.Count() > 0);

            await
            _libraryService
            .DeleteCategory(
                updatedCategory.Id);

            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => 
                    await
                    _libraryService
                    .GetCategory(updatedCategory.Id));

            Assert.True(ex.Message == $"Unable to retrieve Category for Id {updatedCategory.Id}");
        }

        [Fact]
        public async Task ThemeTests()
        {
            var brandId = 3;

            var themes =
                await
                _libraryService
                .GetThemeList(brandId);

            Assert.NotNull(themes);
            Assert.True(themes.Count() > 0);

            var theme =
                await
                _libraryService
                .GetTheme(themes.First().Id);

            Assert.NotNull(theme);
            Assert.True(theme.Id == themes.First().Id);

            // TODO: Bad test, hardcoded siteId
            var siteThemes =
                await
                _libraryService
                .GetSiteThemes(178);

            Assert.NotNull(siteThemes);
            Assert.True(siteThemes.Count() > 0);
        }

        [Fact]
        public async Task PresetTests()
        {
            var brandId = 3;
            const string UpdatedTitle = "Updated Title";

            var categoryList =
                (await
                _libraryService
                .GetCategoryList(brandId))
                .Take(2);

            var preset =
                await
                _libraryService
                .CreatePreset(
                    brandId,
                    new PresetDTO()
                    {
                        Type = "image",
                        Title = "OK To Delete",
                        Description = "Unit test",
                        Settings = "{Some JSON Data}",
                        IsDeleted = false,
                        IsPrivate = false,
                        BrandId = brandId,
                        SiteId = null,
                        ClientCode = null,
                        Categories = categoryList.ToList(),
                        ExpirationDt = null
                    });

            Assert.NotNull(preset);
            Assert.True(preset.Id > 0);

            //preset.Title = UpdatedTitle;

            //var updatedPreset =
            //    await
            //    _libraryService
            //    .UpdatePreset(preset);

            //Assert.NotNull(updatedPreset);
            //Assert.True(updatedPreset.Title == UpdatedTitle);

            var getPreset =
                await
                _libraryService
                .GetPreset(preset.Id);

            Assert.NotNull(getPreset);
            Assert.True(getPreset.Id == getPreset.Id);

            var presetList =
                await
                _libraryService
                .GetPresetList(brandId);

            Assert.NotNull(presetList);
            Assert.True(presetList.Count() > 0);

            var presetListByType =
                await
                _libraryService
                .GetPresetList(brandId, "image");

            Assert.NotNull(presetListByType);
            Assert.True(presetListByType.Count() > 0);

            await
            _libraryService
            .DeletePreset(getPreset.Id);

            var ex =
                await Assert.ThrowsAsync<Exception>(async () =>
                    await
                    _libraryService
                    .GetPreset(getPreset.Id));

            Assert.True(ex.Message == $"Unable to retrieve Preset for Id {getPreset.Id}");
        }
    }
}