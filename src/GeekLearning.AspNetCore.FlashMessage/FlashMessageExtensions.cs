namespace GeekLearning.AspNetCore.FlashMessage
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class FlashMessageExtensions
    {
        public static IServiceCollection AddFlashMessage(this IServiceCollection services)
        {
            services.AddScoped<IFlashMessageManager, Internal.FlashMessageManager>();
            return services;
        }

        public static IApplicationBuilder UseFlashMessage(this IApplicationBuilder app)
        {
            app.UseMiddleware<Internal.FlashMessageCookieMiddleware>();
            return app;
        }
    }
}
