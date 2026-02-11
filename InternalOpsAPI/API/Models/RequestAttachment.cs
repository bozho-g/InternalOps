namespace API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RequestAttachment
    {
        public int Id { get; set; }

        [Required]
        public required string FileName { get; set; }

        [Required]
        public required string FileUrl { get; set; }

        public long FileSize { get; set; }

        public string? ContentType { get; set; }

        public int RequestId { get; set; }
        public Request? Request { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
