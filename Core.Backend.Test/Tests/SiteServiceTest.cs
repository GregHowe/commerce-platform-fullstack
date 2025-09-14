
using Core.CoreLib.Services.Database.Library;
using Core.CoreLib.Services.Database;
using Core.CoreLib.Services.Database.Site;

namespace Core.Backend.Test.Tests
{
    public class SiteServiceTest : TestBase
    {
        private SiteService _siteService;
        private LibraryService _libraryService;

        public SiteServiceTest()
        {
            // Scaffold
            var _config = CreateConfig();

            CreateServices(_config);

            var dapperContext =
                new DapperContext(_config);

            _siteService =
                new SiteService(_config, dapperContext, new LibraryService(dapperContext));
        }

        [Fact]
        public async Task SiteAndPageTests()
        {
            var brandId = 3;
            const string UpdatedTitle = "Updated Title";

            var site =
                await
                _siteService
                .CreateSite(
                    brandId,
                    "c.courtois@fusion92.com",
                    "9689e030-9c4a-4332-9501-fe113ea7ea61",
                    3);

            Assert.NotNull(site);
            Assert.True(site.Id > 0);

            site.Title = UpdatedTitle;

            var updatedSite =
                await
                _siteService
                .UpdateSite(site.Id, site);

            Assert.NotNull(updatedSite);
            Assert.True(updatedSite.Title == UpdatedTitle);

            var getSite =
                await
                _siteService
                .GetSite(updatedSite.Id);

            Assert.NotNull(getSite);
            Assert.True(getSite.Id == getSite.Id);

            // Page tests
            var page =
                await
                _siteService
                .CreateSitePage(Guid.NewGuid().ToString(), site.Id);

            Assert.NotNull(page);
            Assert.True(page.Id > 0);

            await
            _siteService
            .DeleteSitePage(site.Id, page.Id);

            var deletedPage =
                await
                _siteService
                .GetSitePage(site.Id, page.Id);

            Assert.NotNull(deletedPage);
            Assert.True(deletedPage.IsDeleted);

            var siteListByUser =
                await
                _siteService
                .GetSiteListByUser("c.courtois@fusion92.com");

            Assert.NotNull(siteListByUser);
            Assert.True(siteListByUser.Count() > 0);

            var siteListByBrand =
                await
                _siteService
                .GetSiteListByBrand(brandId);

            Assert.NotNull(siteListByBrand);
            Assert.True(siteListByBrand.Count() > 0);

            await
            _siteService
            .DeleteSite(site.Id);

            // Not sure if this should return deleted site or null, use will determine over time
            var deletedSite =
                await
                _siteService
                .GetSite(site.Id);

            Assert.Null(deletedSite);
            //Assert.True(deletedSite.IsDeleted);
        }
    }
}