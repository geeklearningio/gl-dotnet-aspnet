namespace GeekLearning.AspNetCore.Semver
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class SemverExtensions
    {
        public static IServiceCollection AddSemver(this IServiceCollection services, IConfigurationSection semverConfigurationSection)
        {
            services.Configure<SemverOptions>(semverConfigurationSection);
            return services;
        }

        public static IApplicationBuilder UseSemverHttpHeader(this IApplicationBuilder app)
        {
            app.UseMiddleware<Internal.SemverMiddleware>();
            return app;
        }
    }
}
