namespace API.Services
{
    using API.Exceptions;
    using API.Models;
    using API.Services.Interfaces;

    using FileSignatures;

    using Microsoft.Extensions.Options;

    public class FileValidator(IOptions<FileUploadOptions> options) : IFileValidator
    {
        private readonly FileUploadOptions _options = options.Value;
        private readonly FileFormatInspector _inspector = new();

        public async Task ValidateAsync(IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new FileValidationException("File is required");

            ValidateFileSize(file);
            ValidateMimeType(file);
            await ValidateSignatureAsync(file);
        }

        private void ValidateFileSize(IFormFile file)
        {
            long maxBytes = _options.MaxFileSizeMB * 1024L * 1024L;

            if (file.Length > maxBytes)
                throw new FileValidationException("File size exceeds the maximum allowed size of 10 MB");
        }

        private void ValidateMimeType(IFormFile file)
        {
            if (!_options.AllowedMimeTypes.Contains(file.ContentType))
                throw new FileValidationException("MIME type not allowed");
        }

        private async Task ValidateSignatureAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var format = _inspector.DetermineFileFormat(stream);

            if (format == null || !_options.AllowedMimeTypes.Contains(format.MediaType))
                throw new FileValidationException("File signature does not match allowed formats");
        }
    }
}
