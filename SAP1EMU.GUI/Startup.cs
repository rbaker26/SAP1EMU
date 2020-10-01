using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using SAP1EMU.Lib;
using SAP1EMU.GUI.Contexts;
using Microsoft.EntityFrameworkCore;

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
                options.MinimumSameSitePolicy = SameSiteMode.None;
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

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Xss-Protection", "1");
            //    await next();
            //});


            //ElectronBootstrap();
            //Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(browserWindowOptions)).Result.WebContents.Session.ClearCacheAsync();
            //Electron.WindowManager.CreateWindowAsync().Result.BlurWebView();
        }

        private async void ElectronBootstrap()
        {
            var display = Electron.Screen.GetPrimaryDisplayAsync().Result;


            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions()
            {
                Width = display.WorkAreaSize.Width,
                Height = display.WorkAreaSize.Height,
                Show = false,

                WebPreferences = new WebPreferences
                {
                    WebSecurity = false
                }
            });

            await browserWindow.WebContents.Session.ClearCacheAsync();
            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("SAP1Emu");
        }
    }
}
