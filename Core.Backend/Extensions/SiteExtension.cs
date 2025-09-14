
using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Services.User;

namespace Core.Backend.Extensions
{
    public static class SiteExtension
    {
        public static async Task<Models.Web.SiteDTO> ToSiteDTO(this CoreLib.Models.DTO.Site.SiteDTO getSiteDTO, IUserService userService) =>
            await ConvertToSiteDTO(getSiteDTO, userService);
        
        public static async Task<Models.Web.SiteDTO> ToSiteDTO(this CoreLib.Models.DTO.Site.SiteDTO getSiteDTO, IUserService userService, UserAccessInfoDetailDDC ddcUserData) =>
            await ConvertToSiteDTO(getSiteDTO, userService, ddcUserData);
       
        private static async Task<Models.Web.SiteDTO> ConvertToSiteDTO(this CoreLib.Models.DTO.Site.SiteDTO getSiteDTO, IUserService userService, UserAccessInfoDetailDDC? ddcUserData = null) =>
            new Models.Web.SiteDTO
            {
                Id = getSiteDTO.Id,
                BrandId = getSiteDTO.BrandId,
                Handle = getSiteDTO.Handle,
                Title = getSiteDTO.Title,
                Description = getSiteDTO.Description,
                Settings = getSiteDTO.Settings,
                Navigation = getSiteDTO.Navigation,
                Footer = getSiteDTO.Footer,
                SeoDescription = getSiteDTO.SeoDescription,
                SeoIsPrivate = getSiteDTO.SeoIsPrivate,
                SeoTitle = getSiteDTO.SeoTitle,
                HomepageId = getSiteDTO.HomepageId,
                Pages = getSiteDTO.Pages,
                Presets = getSiteDTO.Presets,
                Seats = getSiteDTO.Seats,
                Style = getSiteDTO.Style,
                Themes = getSiteDTO.Themes,
                UserId = getSiteDTO.UserId,
                User =
                    ddcUserData == null ?
                        (await userService.GetADUserAsync(getSiteDTO.UserId)).ToWebNYLUser() :
                        (await userService.GetADUserAsync(getSiteDTO.UserId)).ToWebNYLUser(ddcUserData)
            };
    }
}