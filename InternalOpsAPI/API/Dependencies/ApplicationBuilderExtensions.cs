namespace API.Dependencies
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseCustomMiddleware(this WebApplication app)
        {
            app.UseExceptionHandler();

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
