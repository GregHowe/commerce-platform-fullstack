using Core.CoreLib.Models.Database.Core;
using Microsoft.EntityFrameworkCore;

namespace Core.CoreLib.Services.Database
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Database.Core.Brand> Brands { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Preset> Presets { get; set; }
        public DbSet<PresetCategory> PresetCategories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Models.Database.Core.Site> Sites { get; set; }
        public DbSet<SiteTheme> SiteThemes { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Models.Database.Core.User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSeat> UserSeats { get; set; }
    }
}