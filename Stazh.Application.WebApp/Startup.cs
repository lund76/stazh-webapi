using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stazh.Core.Data;
using Stazh.Core.Data.Repositories;
using Stazh.Core.Services;
using Stazh.Services.Implementations;
using Stazh.Data.AzureBlob;
using Stazh.Data.EFCore;
using Stazh.Data.EFCore.Repositories;



namespace Stazh.Application.WebApp
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

            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);


            var context = new StazhDataContext(Configuration.GetConnectionString("StazhDataContextString"));

            //services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileStorage, BlobStorage>();

            services.AddScoped<IFileService>(
                 isp => new FileService(isp.GetRequiredService<IFileStorage>()));

            services.AddScoped<IItemService>(
                isp => new ItemService(isp.GetRequiredService<IUnitOfWork>(), isp.GetRequiredService<IFileService>()));

            services.AddSingleton<IUnitOfWork>(isp => new UnitOfWork(context));


            services.AddControllersWithViews();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000");
                    });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("_myAllowSpecificOrigins");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
