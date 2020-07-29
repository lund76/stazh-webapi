using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stazh.Core.Data;
using Stazh.Core.Data.Repositories;
using Stazh.Core.Services;
using Stazh.Data.AzureBlob;
using Stazh.Data.EFCore;
using Stazh.Services.Implementations;

namespace Stazh.Application.WebApi
{
    public class Startup
    {
        private readonly string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
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
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: myAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyHeader();
                        builder.WithOrigins("http://localhost:3000");
                    });
            });

            var context = new StazhDataContext(Configuration.GetConnectionString("StazhDataContextString"));

            //services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileStorage, BlobStorage>();

            services.AddScoped<IFileService>(
                isp => new FileService(isp.GetRequiredService<IFileStorage>()));

            services.AddScoped<IItemService>(
                isp => new ItemService(isp.GetRequiredService<IUnitOfWork>(), isp.GetRequiredService<IFileService>()));

            services.AddSingleton<IUnitOfWork>(isp => new UnitOfWork(context));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(myAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
