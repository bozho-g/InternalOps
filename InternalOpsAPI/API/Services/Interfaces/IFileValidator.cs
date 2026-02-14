namespace API.Services.Interfaces
{
    public interface IFileValidator
    {
        Task ValidateAsync(IFormFile file);
    }
}
