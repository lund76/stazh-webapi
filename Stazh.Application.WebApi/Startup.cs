using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stazh.Core.Data;
using Stazh.Core.Data.Models;
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
            StorageConfig = new StorageConfig
            {
                ConnectionString = Configuration["FileConnectionString"], FileContainer = "images"
                ,ThumbnailContainer = "thumbnails", AccountName = "stazhdevstorage", AccountKey = @"YWKwLUQi21OKkMDOWby4LwymTfbUyjDeM098fDAWFQ4cGeGedWo2Q87yjRaD6fHo3Tx3gV3K0/lNCXHVcvSbag=="
            };
        }

        public IConfiguration Configuration { get; }
        public StorageConfig StorageConfig { get; }

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
                        builder.AllowAnyMethod();
                        builder.WithOrigins("http://localhost:3000","https://stazh-web-app.azurewebsites.net");
                    });
            });

            var context = new StazhDataContext(Configuration.GetConnectionString("StazhDataContextString"));

            //services.AddScoped<IFileService, FileService>();
            //services.AddScoped<IFileStorage, BlobStorage>();

            services.AddScoped<IFileService>(
                isp => new FileService(isp.GetRequiredService<IFileStorage>()));
            services.AddScoped<IFileStorage>(
                isp => new BlobStorage(StorageConfig));

            services.AddScoped<IItemService>(
                isp => new ItemService(isp.GetRequiredService<IUnitOfWork>(), isp.GetRequiredService<IFileService>()));
            services.AddScoped<IUserService>(
                isp => new UserService(isp.GetRequiredService<IUnitOfWork>()));

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
