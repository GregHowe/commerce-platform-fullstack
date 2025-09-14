
namespace Core.CoreLib.Models.Azure.Blob
{
    public class BlobResponseDTO
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDTO Blob { get; set; } = new BlobDTO();
    }
}