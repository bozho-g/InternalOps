

namespace API
{
    using System.Threading.Tasks;

    using API.Dependencies;
    using API.Dependencies.Infrastructure;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApiServices(builder.Configuration);

            var app = builder.Build();

            app.UseCustomMiddleware();

            app.Run();
        }
    }
}
