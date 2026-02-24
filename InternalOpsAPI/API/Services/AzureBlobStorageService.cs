namespace API.Services
{
    using API.Services.Interfaces;

    using Azure.Storage.Blobs;

    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            var containerName = configuration["AzureStorage:ContainerName"];

            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            _containerClient.CreateIfNotExists();
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var blobName = uri.Segments[^1];

            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> FileExistsAsync(string fileUrl)
        {
            var url = new Uri(fileUrl);
            var blobName = url.Segments[^1];

            var blobClient = _containerClient.GetBlobClient(blobName);
            return await blobClient.ExistsAsync();
        }

        public async Task<string> UploadFileAsync(IFormFile file, string requestId)
        {
            var blobName = $"{requestId}/{Guid.NewGuid()}_{file.FileName}";

            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
