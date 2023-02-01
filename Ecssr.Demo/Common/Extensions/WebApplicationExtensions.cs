using Microsoft.AspNetCore.Antiforgery;

namespace Ecssr.Demo.Common.Extensions
{
    /// <summary>
    /// Extension class to configure the piplines using DI
    /// </summary>
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureApp(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            #region Security and antiforgery
            _ = app.UseHsts();

            var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
            _ = app.Use((context, next) =>
            {
                var tokenSet = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
                    new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None
                    });

                return next(context);
            });
            #endregion

            #region Exceptions

            _ = app.UseGlobalExceptionHandler();

            #endregion Exceptions

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html"); ;

            return app;
        }
    }
}
