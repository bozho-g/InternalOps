namespace API.DTOs.Attachments
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required string FileUrl { get; set; }
        public long FileSize { get; set; }
        public string? ContentType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
