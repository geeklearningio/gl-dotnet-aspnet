using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GeekLearning.AspNetCore.Sample.Data;
using GeekLearning.AspNetCore.Sample.Models;
using GeekLearning.AspNetCore.Sample.Services;
using GeekLearning.AspNetCore.FlashMessage;
using GeekLearning.AspNetCore.Semver;
using GeekLearning.AspNetCore.Identity.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace GeekLearning.AspNetCore.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.version.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            services.AddFlashMessage();
            services.AddSemver(Configuration.GetSection("Version"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var frenchCultureInfo = new CultureInfo("fr-FR");
            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = new List<CultureInfo> { frenchCultureInfo },
                SupportedUICultures = new List<CultureInfo> { frenchCultureInfo },
                DefaultRequestCulture = new RequestCulture(frenchCultureInfo),
            };

            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSemverHttpHeader();
            app.UseFlashMessage();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
