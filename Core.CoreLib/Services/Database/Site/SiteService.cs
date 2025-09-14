
using Core.CoreLib.Models.DTO.Library;
using Core.CoreLib.Models.DTO.Site;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using Core.CoreLib.Services.Azure.ServiceBus;
using Core.CoreLib.Models.Azure.ServiceBus;
using System.Text.Json;
using Core.CoreLib.Models.Constants;
using static Dapper.SqlMapper;
using Core.CoreLib.Services.Database.Library;
using Core.CoreLib.Services.Database.Base;
using Microsoft.Graph;
using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Extensions;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Reflection.Metadata.BlobBuilder;
using Core.CoreLib.Models.Exceptions;

namespace Core.CoreLib.Services.Database.Site
{
    public partial class SiteService : DBBase, ISiteService
    {
        protected IConfiguration _configuration;
        protected ILibraryService _libraryService;
        private DapperContext _context;

        private const string DefaultSiteThemeTitle = "nyl-01";
        private const string DefaultSiteThemeType = "theme";
        private const string DefaultPageTitle = "Untitled Page";
        private const string DefaultSiteTitle = "Untitled Site";

        public SiteService(
            IConfiguration configuration,
            DapperContext dapperContext,
            ILibraryService libraryService) : base(dapperContext)
        {
            _configuration = configuration;
            _libraryService = libraryService;
            _context = dapperContext;
        }

        public async Task<IEnumerable<GetSiteListDTO>> GetSiteListByBrand(int brandId) =>
            (await
            ExecuteQuery<GetSiteListDTO>($"SELECT UserId, IsPrivate, Id, Handle, Title, Description FROM {DatabaseTables.Site} WHERE IsDeleted = 0 AND BrandId = {brandId}"))
            .ToList();

        public async Task<IEnumerable<GetSiteListDTO>> GetSiteListByUser(string userId) =>        
            (await 
            ExecuteQuery<GetSiteListDTO>($"SELECT UserId, IsPrivate, Id, Handle, Title, Description FROM {DatabaseTables.Site} WHERE IsDeleted = 0 AND UserId = '{userId}'"))
            .ToList();

        public async Task<SiteDTO?> CreateSite(int brandId, string userId, string userObjectId, int environmentId)
        {
            var handle = Guid.NewGuid().ToString();
            
            var coreSiteId = 
                (await ExecuteQuery<int>(
                    $"INSERT INTO {DatabaseTables.Site} " +
                    $"(Handle, Title, UserId, SeoIsPrivate, UserObjectId, IsPrivate, IsDeleted, BrandId, EnvironmentId, Created) " +
                    $"OUTPUT INSERTED.Id " +
                    $"VALUES " +
                    $"(@Handle, @Title, @UserId, @SeoIsPrivate, @UserObjectId, @IsPrivate, @IsDeleted, @BrandId, @EnvironmentId, GETDATE())",
                    new
                    {
                        @Handle = handle,
                        @Title = DefaultSiteTitle,
                        @UserId = userId.ScrubForDB(),
                        @SeoIsPrivate = 0,
                        @UserObjectId = userObjectId,
                        @IsPrivate = 0,
                        @IsDeleted = 0,
                        @BrandId = brandId,
                        @EnvironmentId = environmentId,
                    }))
                    .FirstOrDefault();

            var corePage = await CreateSitePage(handle, coreSiteId);

            await ExecuteQuery($"UPDATE {DatabaseTables.Site} SET HomepageId = {corePage.Id} WHERE Id = {coreSiteId}");
                
            return await GetSite(coreSiteId);
        }

        public async Task<SiteDTO> DuplicateSiteUseWithCaution(SiteDTO site)
        {
            if (site == null)
                throw new Exception("Can't update null site, nice try");

            var newSiteId = 0;

            try
            {
                // !!!!!!!!!!This is extremely brittle and linear and designed with very specific intent, CHANGE IT AND YOU OWN IT!!!!!!!!!!
                // This is not good code, will all be better after Content Library exists.
                using (var connection = _context.CreateConnection())
                {
                    var newHandle = 
                        Guid.NewGuid().ToString();

                    // Create new site
                    newSiteId =
                        await connection.ExecuteScalarAsync<int>(
                            $"INSERT INTO {DatabaseTables.Site} (Handle, Title, Description, SeoTitle, SeoDescription, SeoIsPrivate, IsPrivate, IsDeleted, BrandId, EnvironmentId, UserId, UserObjectId, HomePageId, Settings, Footer, Navigation, Created, Retired, Style) " +
                            "OUTPUT INSERTED.Id VALUES" +
                            " (@Handle, @Title, @Description, @SeoTitle, @SeoDescription, @SeoIsPrivate, @IsPrivate, @IsDeleted, @BrandId, @EnvironmentId, @UserId, @UserObjectId, NULL, @Settings, @Footer, @Navigation, GETDATE(), NULL, @Style)",
                            new
                            {
                                @Handle = newHandle,
                                @Title = site.Title.ScrubForDB(),
                                @Description = site.Description.ScrubForDB(),
                                @SeoTitle = site.SeoTitle.ScrubForDB(),
                                @SeoDescription = site.SeoDescription.ScrubForDB(),
                                @SeoIsPrivate = site.SeoIsPrivate,
                                @IsPrivate = site.IsPrivate,
                                @IsDeleted = 0,
                                @BrandId = site.BrandId,
                                @EnvironmentId = site.BrandId,
                                @UserId = site.UserId,
                                @UserObjectId = site?.UserObjectId ?? "Missing Object Id",
                                //HomePageId = null,
                                @Settings = site.Settings.ScrubForDB(),
                                @Footer = site.Footer.ScrubForDB(),
                                @Navigation = site.Navigation.ScrubForDB(),
                                @Style = site.Style.ScrubForDB()
                            });

                    var pageMap = new List<PageMap>();

                    // Duplicate pages
                    foreach (var page in site.Pages ?? new List<PageDTO>())
                    {
                        var newPageId =
                            await connection.ExecuteScalarAsync<int>(
                                $"INSERT INTO {DatabaseTables.Page} (Handle, Title, Description, NavigationTitle, SeoTitle, SeoDescription, Blocks, RedirectUrl, SiteId, ParentPageId, IsDeleted, IsPrivate, SeoIsPrivate, Settings, Created, Retired) " +
                                "OUTPUT INSERTED.Id VALUES " +
                                " (@Handle, @Title, @Description, @NavigationTitle, @SeoTitle, @SeoDescription, @Blocks, @RedirectUrl, @SiteId, @ParentPageId, @IsDeleted, @IsPrivate, @SeoIsPrivate, @Settings, GETDATE(), NULL)",
                                new
                                {
                                    @Handle = page.Handle,
                                    @Title = page.Title,
                                    @Description = page.Description.ScrubForDB(),
                                    @NavigationTitle = page.NavigationTitle.ScrubForDB(),
                                    @SeoTitle = page.SeoTitle.ScrubForDB(),
                                    @SeoDescription = page.SeoDescription.ScrubForDB(),
                                    @Blocks = page.Blocks.ScrubForDB(),
                                    @RedirectUrl = page.RedirectUrl.ScrubForDB(),
                                    @SiteId = newSiteId,
                                    @ParentPageId = page.ParentPageId, // Set the old ParentPageId, gets updated below via PageMap
                                    @IsDeleted = 0,
                                    @IsPrivate = page.IsPrivate,
                                    @SeoIsPrivate = page.SeoIsPrivate,
                                    @Settings = page.Settings.ScrubForDB()
                                });

                        pageMap.Add(new PageMap() { PageId = page.Id, NewPageId = newPageId });

                        // Update the new site homepageId
                        if (site.HomepageId != null &&
                            page.Id == site.HomepageId)
                            await connection.ExecuteScalarAsync<int>(
                               $"UPDATE {DatabaseTables.Site} SET HomePageId = {newPageId} WHERE Id = {newSiteId} AND Handle = '{newHandle}'");
                    }

                    // Update ParentPageIds
                    foreach (var page in pageMap)
                        await connection.ExecuteScalarAsync<int>(
                            $"UPDATE {DatabaseTables.Page} SET ParentPageId = {page.NewPageId} WHERE SiteId = {newSiteId} AND ParentPageId = {page.PageId}");

                    // Can't trust the Presets attached to original site object, they don't expose all the props.
                    // Requery Presets for site that we are duplicating
                    var presets =
                        await connection.QueryAsync<PresetDTO>(
                            $"SELECT * FROM {DatabaseTables.Preset} WHERE SiteId = {site.Id}");

                    // Duplicate Presets for new site
                    foreach (var preset in presets)
                    {
                        await connection.ExecuteScalarAsync<int>(
                           @$"INSERT INTO {DatabaseTables.Preset} (Handle, Type, Title, Description, Settings, IsPrivate, IsDeleted, BrandId, SiteId, ParentPresetId, ClientCode, CreatedDt, ExpirationDt, AllowAgent, AllowDBA, AllowEagle, AllowNautilus, AllowRegisteredRep, AllowGO, AllowHO, AllowLongTermCare, AllowBusinessSolutions, AllowAARP, SMRUCode) 
                            OUTPUT INSERTED.Id
                            VALUES (@Handle, @Type, @Title, @Description, @Settings, @IsPrivate, @IsDeleted, NULL, @SiteId, @ParentPresetId, @ClientCode, GETDATE(), @ExpirationDt, @AllowAgent, @AllowDBA, @AllowEagle, @AllowNautilus, @AllowRegisteredRep, @AllowGO, @AllowHO, @AllowLongTermCare, @AllowBusinessSolutions, @AllowAARP, @SMRUCode)",
                            new
                            {
                                @Handle = Guid.NewGuid().ToString(),
                                @Type = preset.Type.ScrubForDB(),
                                @Title = preset.Title.ScrubForDB(),
                                @Description = preset.Description.ScrubForDB(),
                                @Settings = preset.Settings.ScrubForDB(),
                                @IsPrivate = preset.IsPrivate,
                                @IsDeleted = 0,
                                //BrandId = null
                                @SiteId = newSiteId,
                                @ClientCode = preset.ClientCode.ScrubForDB(),
                                @ParentPresetId = preset.ParentPresetId,
                                //@CreatedDt = preset.CreatedDt,
                                @ExpirationDt = preset.ExpirationDt,
                                @AllowAgent = preset.AllowAgent,
                                @AllowDBA = preset.AllowDBA,
                                @AllowEagle = preset.AllowEagle,
                                @AllowNautilus = preset.AllowNautilus,
                                @AllowRegisteredRep = preset.AllowRegisteredRep,
                                @AllowGO = preset.AllowGO,
                                @AllowHO = preset.AllowHO,
                                @AllowLongTermCare = preset.AllowLongTermCare,
                                @AllowBusinessSolutions = preset.AllowBusinessSolutions,
                                @AllowAARP = preset.AllowAARP,
                                @SMRUCode = preset.SMRUCode.ScrubForDB()
                            });
                    }

                    // SiteThemes
                    var themeIds =
                        await connection.QueryAsync<int>(
                            $"SELECT ThemeId FROM {DatabaseTables.SiteTheme} WHERE SiteId = {site.Id}");

                    foreach (var themeId in themeIds ?? new List<int>())
                    {
                        await connection.ExecuteScalarAsync<int>(
                          @$"INSERT INTO {DatabaseTables.SiteTheme} (SiteId, ThemeId) 
                            OUTPUT INSERTED.Id
                                VALUES (@SiteId, @ThemeId)",
                           new
                           {
                               SiteId = newSiteId,
                               ThemeId = themeId
                           });
                    }

                    // Validate
                    var newSite = await GetSite(newSiteId);

                    if (newSite == null)
                        throw new Exception($"Site duplication failed, new site not found, new site id {newSiteId}");

                    if (newSite.Pages.Count != site.Pages.Count ||
                        newSite.Presets.Count != site.Presets.Count)
                        throw new Exception($"Site duplication failed, supporting object counts don't match");

                    return
                        newSite;
                }
            }
            catch (Exception ex)
            {
                await HardDeleteSiteUseWithCaution(newSiteId, site.Handle);

                throw ex;
            }
        }

        public async Task<SiteDTO?> AssignSiteToNewUser(int siteId, string userEmail, string userObjectId)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteScalarAsync<int>(
                    $"UPDATE {DatabaseTables.Site} SET UserId = '{userEmail}', UserObjectId = '{userObjectId}' WHERE Id = {siteId}");
            }

            return
                await GetSite(siteId);
        }

        public async Task HardDeleteSiteUseWithCaution(int siteId, string handle)
        {
            using (var connection = _context.CreateConnection())
            {
                // Any exceptions and we remove all DB objects associated with new site we attempted to create
                await connection.ExecuteScalarAsync<int>($"DELETE FROM Core.Site WHERE Id = {siteId} AND Handle = '{handle}'");
                await connection.ExecuteScalarAsync<int>($"DELETE FROM Core.Page WHERE SiteId = {siteId}");
                await connection.ExecuteScalarAsync<int>($"DELETE FROM Core.Preset WHERE SiteId = {siteId}");
                await connection.ExecuteScalarAsync<int>($"DELETE FROM Core.SiteTheme WHERE SiteId = {siteId}");
            }
        }

        private class PageMap
        {
            public int PageId { get; set; }
            public int NewPageId { get; set; }
        }

        public async Task<SiteDTO?> GetSite(int siteId)
        {
            var site =
                (await ExecuteQuery<SiteDTO>($"SELECT * FROM {DatabaseTables.Site} WHERE Id = {siteId} AND IsDeleted = 0"))
                .FirstOrDefault();

            if (site == null)
                return site;

            // TODO: These should be broken out to functions (Most probably already exist)
            var pages = 
                await ExecuteQuery<PageDTO>($"SELECT * FROM {DatabaseTables.Page} WHERE IsDeleted = 0 AND SiteId = {siteId}") ??
                new List<PageDTO>();

            site.Pages = new List<PageDTO>();
            foreach (var page in pages) 
                site.Pages.Add(page);

            var siteThemes =
                await
                _libraryService.GetSiteThemes(siteId);
                
            site.Themes =
                new SiteThemesDTO()
                {
                    // siteThemes will become a list soon, this will go from .FirstOrDefault() to a for loop
                    Id = siteThemes?.FirstOrDefault()?.Id ?? 0,
                    Base = siteThemes?.FirstOrDefault()?.Title ?? DefaultSiteThemeTitle,
                    Type = siteThemes?.FirstOrDefault()?.Type ?? DefaultSiteThemeType
                };

            site.Presets =
                (await 
                _libraryService.GetSitePresetList(siteId))
                .ToList();
            
            return site;
        }

        public async Task<SiteDTO?> UpdateSite(int siteId, SiteDTO site)
        {
            if (site == null)
                throw new BadRequestException($"Update failed, invalid site. Id: {siteId}");

            // Site
            var coreSiteQuery =
                @$"UPDATE {DatabaseTables.Site} 
                SET
                    Handle = @Handle,
                    Title = @Title,
                    Description = @Description,
                    Settings = @Settings,
                    Navigation = @Navigation,
                    Footer = @Footer,
                    SeoTitle = @SeoTitle,
                    SeoDescription = @SeoDescription,
                    SeoIsPrivate = @SeoIsPrivate,
                    IsPrivate = @IsPrivate,
                    HomepageId = @HomepageId
                WHERE Id = {siteId}";

            await ExecuteQuery(
                coreSiteQuery,
                new
                {
                    @Handle = site.Handle,
                    @Title = site.Title.ScrubForDB(),
                    @Description = site.Description.ScrubForDB(),
                    @Settings = site.Settings.ScrubForDB(),
                    @Navigation = site.Navigation.ScrubForDB(),
                    @Footer = site.Footer.ScrubForDB(),
                    @SeoTitle = site.SeoTitle.ScrubForDB(),
                    @SeoDescription = site.Description.ScrubForDB(),
                    @SeoIsPrivate = site.SeoIsPrivate ? 1 : 0,
                    @IsPrivate = site.IsPrivate ? 1 : 0,
                    @HomepageId = site.HomepageId
                });

            // Pages
            foreach (var page in site.Pages ?? new List<PageDTO>())
            {
                var corePageQuery =
                    @$"UPDATE {DatabaseTables.Page} SET
                        Handle = @Handle,
                        Title = @Title,
                        Description = @Description,
                        Blocks = @Blocks,
                        Settings = @Settings,
                        NavigationTitle = @NavigationTitle,
                        SeoTitle = @SeoTitle,
                        SeoDescription = @SeoDescription,
                        SeoIsPrivate = @SeoIsPrivate,
                        IsPrivate = @IsPrivate,
                        IsDeleted = @IsDeleted,
                        ParentPageId = @ParentPageId
                    WHERE SiteId = {siteId} AND Id = {page.Id}";

                await ExecuteQuery(
                    corePageQuery,
                    new
                    {
                        @Handle = page.Handle,
                        @Title = page.Title.ScrubForDB(),
                        @Description = page.Description.ScrubForDB(),
                        @Blocks = page.Blocks.ScrubForDB(),
                        @Settings = page.Settings.ScrubForDB(),
                        @NavigationTitle = page.NavigationTitle.ScrubForDB(),
                        @SeoTitle = page.SeoTitle.ScrubForDB(),
                        @SeoDescription =page.SeoDescription.ScrubForDB(),
                        @SeoIsPrivate = page.SeoIsPrivate ? 1 : 0,
                        @IsPrivate = page.IsPrivate ? 1 : 0,
                        @IsDeleted = page.IsDeleted ? 1 : 0,
                        @ParentPageId = page.ParentPageId
                });
            }

            // Themes/SiteThemes
            //
            // Get the currently available Themes (cache this at some point TODO)
            var availableThemes =
                await _libraryService.GetThemeList(site.BrandId ?? 0);

            // DELETE from SiteThemes, update SiteThemes with new id
            await _libraryService.DeleteSiteThemes(siteId);

            // Assign default theme in absence of any theme
            if (site.Themes == null)
                site.Themes =
                    new SiteThemesDTO()
                    {
                        Base = DefaultSiteThemeTitle,
                        Type = DefaultSiteThemeType,
                        Id = availableThemes.Where(w => w.Title == DefaultSiteThemeTitle).Select(s => s.Id).FirstOrDefault()
                    };

            // Always lookup Id, front end is passing string (Title)
            // If a Title is passed that does not exist, do not save
            if (string.IsNullOrWhiteSpace(site.Themes.Base) ||
                !availableThemes.Where(w => w.Title == site.Themes.Base).Select(s => s.Id).Any())
                throw new BadRequestException($"Invalid Theme title: {site.Themes.Base}.  Theme not updated.");
            else
                // Get the current theme Id for the Title passed
                site.Themes.Id =
                    availableThemes.Where(w => w.Title == site.Themes.Base).Select(s => s.Id).FirstOrDefault();

            await _libraryService.CreateSiteThemes(siteId, new List<int>() { site.Themes.Id });

            return
                await GetSite(siteId);
        }

        public async Task<bool> PublishSite(int siteId, SiteDTO site)
        {
            var result = 
                await UpdateSite(siteId, site);

            if (result == null)
                throw new DataNotFoundException($"Unable to retrieve site with Id: {siteId}");

            await
            new MessageSender()
           .SendMessageAsync(
                new PublishSiteMessage()
                {
                    BrandId = result.BrandId ?? 3,
                    SiteId = siteId,
                    Action = PublishSiteMessageActions.Publish
                },
                _configuration["ConnectionStrings:SBUserIngest"],
                PublishSiteMessageTopic.Publishing,
                JsonNamingPolicy.CamelCase);

            return true;
        }

        public async Task DeleteSite(int siteId) =>
            await ExecuteQuery($"UPDATE {DatabaseTables.Site} SET IsDeleted = 1 WHERE Id = {siteId}");
        
        public async Task<PageDTO> CreateSitePage(string handle, int siteId)
        {
            var pageId =
                (await ExecuteQuery<int>(
                    $"INSERT INTO {DatabaseTables.Page} (SiteId, Handle, Title, SeoIsPrivate, IsPrivate, IsDeleted, Created) OUTPUT INSERTED.Id VALUES " +
                    $"(@SiteId, @Handle, @Title, @SeoIsPrivate, @IsPrivate, @IsDeleted, GETDATE())",
                    new
                    { 
                        @SiteId = siteId,
                        @Handle = handle,
                        @Title = DefaultPageTitle,
                        @SeoIsPrivate = 0,
                        @IsPrivate = 0,
                        @IsDeleted = 0
                    }))
                    .FirstOrDefault();

            return await GetSitePage(siteId, pageId);
        }

        public async Task<PageDTO> GetSitePage(int siteId, int pageId) =>
            (await ExecuteQuery<PageDTO>($"SELECT * FROM {DatabaseTables.Page} WHERE Id = {pageId} AND SiteId = {siteId}"))
            .FirstOrDefault() ??
            throw new DataNotFoundException($"No page found with page Id {pageId} and site Id {siteId}");

        public async Task DeleteSitePage(int siteId, int pageId) =>
            await ExecuteQuery($"UPDATE {DatabaseTables.Page} SET IsDeleted = 1 WHERE Id = {pageId} AND SiteId = {siteId}");
    }
}