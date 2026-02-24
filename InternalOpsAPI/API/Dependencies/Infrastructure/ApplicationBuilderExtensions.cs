namespace API.Dependencies.Infrastructure
{
    using API.Hubs;

    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseCustomMiddleware(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<NotificationHub>("/notifications");

            return app;
        }
    }
}
