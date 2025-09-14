
namespace Core.CoreLib.Models.DTO.Site
{
    // THis is really a "Style", each site can have 3 styles, theme, font and color...1 of each Type
    public class ThemeBase
    {
        public int Id { get; set; }
        public string? Base { get; set; }
        //public string? Description { get; set; } // Do not need to expose until we create Themes in UI
        public string? Type { get; set; } // theme, font, color
        public int? Color { get; set; } // Will be obsolete, to be defined by Type
        public int? Font { get; set; } // Will be obsolete, to be defined by Type
    }
}
