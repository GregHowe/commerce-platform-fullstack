
namespace Core.CoreLib.Models.Database.Core
{
	public class Dba
	{
        public string DbaLogoGuid { get; set; }
        public string UrlGuid { get; set; }
        public string ImageGuid { get; set; }
        public string ImageBinary { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}