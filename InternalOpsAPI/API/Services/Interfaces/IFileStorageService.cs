namespace API.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string requestId);
        Task DeleteFileAsync(string fileUrl);
    }
}
