using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SAP1EMU.GUI.Contexts;
using SAP1EMU.Lib;

namespace SAP1EMU.GUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                //options.MinimumSameSitePolicy = SameSiteMode.None;

                //Cookie restrictions if we ever do use cookies to store and retrieve from
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always; 
            });

            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<IDecoder, InstructionDecoder>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<Sap1EmuContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("sap1emu_db_conn_string")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // This adds the appropriate headers to the http responses
            app.Use(async (context, next) =>
            {
                // Protect against XSS (Cross site scripting). The header is designed to enable the filter built into modern web browsers. This is usually enabled
                // by default but using it will enforce it. 
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                // This response indicates whether or not a browser should be allowed to render a page in a <frame>, <iframe>, or <object>. Sites can use this to 
                // avoid clickjacking attacks, by ensuring that their content is not embedded into other sites. 
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                // A header that is used by the server to indicate that the MIME types in Content-Type headers should not be changed and be followed.
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                // This header lets a webs site tell browsers that it should only be accessed using HTTPS, instead of using HTTP.
                // max-age: The time in seconds that the browser should remmeber that a site is only to be accessed using HTTPS
                // context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");

                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}