using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Globomantics.ProductsApi
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
            var allowedOrigins = Configuration
                .GetValue<string>("AllowedOrigins")?
                .Split(",") ?? new string[0];

            services.AddCors(options=>
            {
                options.AddPolicy("GlobomanticsInteral",
                    builder => builder
                        
                        // example with sub domain:
                        // .WithOrigins("https://*.globomanticsshop.com")
                        // .SetIsOriginAllowedToAllowWildcardSubdomains()

                        // example runtime validation
                        // .SetIsOriginAllowed(IsOriginAllowed)

                        .WithOrigins(allowedOrigins)
                        .WithExposedHeaders("PageNo", "PageSize", "PageCount", "PageTotalRecords")
                    );
                options.AddPolicy("PublicApi",
                    builder => builder
                        .AllowAnyOrigin()
                        .WithExposedHeaders("Get")
                        .WithHeaders("Content-Type"));
            });
            services.AddControllers();
        }

        private static bool IsOriginAllowed(string host)
        {
            var corsOriginAllowed = new[] { "globomantics" };

            return corsOriginAllowed.Any(origin => host.Contains(origin));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("GlobomanticsInteral");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
