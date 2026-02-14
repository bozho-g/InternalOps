namespace API.Models
{
    public class FileUploadOptions
    {
        public int MaxFileSizeMB { get; set; }
        public string[] AllowedMimeTypes { get; set; } = [];
    }
}
