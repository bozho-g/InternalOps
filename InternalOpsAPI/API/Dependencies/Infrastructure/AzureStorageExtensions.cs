namespace API.Dependencies.Infrastructure
{
    using API.Services;
    using API.Services.Interfaces;

    using Azure.Identity;
    using Azure.Storage.Blobs;

    public static class AzureStorageExtensions
    {
        public static IServiceCollection AddAzureStorage(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                var config = x.GetRequiredService<IConfiguration>();
                var accountUrl = config["AzureStorage:AccountUrl"];

                return new BlobServiceClient(
                    new Uri(accountUrl!),
                    new DefaultAzureCredential());
            });

            services.AddScoped<IFileStorageService, AzureBlobStorageService>();

            return services;
        }
    }
}
