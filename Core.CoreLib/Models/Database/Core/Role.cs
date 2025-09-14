namespace Core.CoreLib.Models.Database.Core
{
    public class Role
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public int BrandId { get; set; }
    }
}
