

namespace API
{
    using System.Threading.Tasks;

    using API.Dependencies;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApiServices(builder.Configuration)
                .AddControllers();

            var app = builder.Build();

            app.UseCustomMiddleware();

            app.Run();
        }
    }
}
