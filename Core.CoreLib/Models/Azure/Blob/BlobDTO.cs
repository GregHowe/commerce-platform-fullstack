
using System.Text.Json.Serialization;

namespace Core.CoreLib.Models.Azure.Blob
{
    public class BlobDTO
    {
        //CDN address
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }

        // Blob address
        public string? BlobUri { get; set; }
    }
}