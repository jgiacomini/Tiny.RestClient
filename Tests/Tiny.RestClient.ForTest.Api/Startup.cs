using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tiny.RestClient.ForTest.Api.CompressionProvider;
using Tiny.RestClient.ForTest.Api.Middleware;

namespace Tiny.RestClient.ForTest.Api
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
            services.Configure<GzipCompressionProviderOptions>(o => o.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddResponseCompression(o =>
            {
               o.Providers.Add(new BrotliCompressionProvider());
               o.Providers.Add(new DeflateCompressionProvider());
               o.EnableForHttps = true;
            });

            services.AddMvc(options =>
            {
                // Enable support of XML serialization
                options.RespectBrowserAcceptHeader = true;
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CompressionMiddleware>();
            app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
