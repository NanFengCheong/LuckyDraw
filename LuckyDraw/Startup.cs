using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LuckyDraw.Data;
using EmbeddedBlazorContent;
using LuckyDraw.Context;
using Microsoft.EntityFrameworkCore;
using LuckyDraw.Pages;
using System.Reflection;
using BlazorState;
using LuckyDraw.Features;
using MatBlazor;

namespace LuckyDraw
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContext<LuckyDrawContext>(options => options.UseSqlite("Data Source=luckyDraw.db"));
            services.AddTransient<LuckyDrawService>();
            services.AddBlazorState
              (

                (aOptions) =>
                {
                    aOptions.Assemblies =
                       new Assembly[]
                       {
                                    typeof(Startup).GetTypeInfo().Assembly,
                       };
                }
              );
            services.AddScoped<LuckyDrawState>();
            services.AddApplicationInsightsTelemetry();
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.TopFullWidth;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = false;
                config.MaximumOpacity = 100;
                config.VisibleStateDuration = 5000;
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LuckyDrawHub>("/LuckyDrawHub");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            app.UseEmbeddedBlazorContent(typeof(MatBlazor.BaseMatComponent).Assembly);
        }
    }
}