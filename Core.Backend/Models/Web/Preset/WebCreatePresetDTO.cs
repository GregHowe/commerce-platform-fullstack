
namespace Core.Backend.Models.Web.Preset
{
    public class WebCreatePresetDTO : WebUpdatePresetDTO
    {
        public string Type { get; set; }
        public bool AllowAgent { get; set; }
        public bool AllowDBA { get; set; }
        public bool AllowEagle { get; set; }
        public bool AllowNautilus { get; set; }
        public bool AllowRegisteredRep { get; set; }
        public bool AllowGO { get; set; }
        public bool AllowHO { get; set; }
        public bool AllowLongTermCare { get; set; }
        public bool AllowBusinessSolutions { get; set; }
        public bool AllowAARP { get; set; }
    }
}
