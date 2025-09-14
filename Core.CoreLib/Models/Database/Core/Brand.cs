namespace Core.CoreLib.Models.Database.Core
{
    public class Brand
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Host { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Settings { get; set; }
        public bool IsDeleted { get; set; }
    }
}
