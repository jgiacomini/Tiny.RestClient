using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tiny.RestClient.ForTest.Api.CompressionProvider;
using Tiny.RestClient.ForTest.Api.Middleware;

public class Startup
{
    public IConfiguration Configuration
    {
        get;
    }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers(options =>
        {
            // Enable support of XML serialization
            options.RespectBrowserAcceptHeader = true;
            options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
            options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
        });
        services.AddEndpointsApiExplorer();

        services.Configure<GzipCompressionProviderOptions>(o => o.Level = System.IO.Compression.CompressionLevel.Optimal);
        services.AddResponseCompression(o =>
        {
            o.Providers.Add(new Tiny.RestClient.ForTest.Api.CompressionProvider.BrotliCompressionProvider());
            o.Providers.Add(new DeflateCompressionProvider());
            o.EnableForHttps = true;
        });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<CompressionMiddleware>();
        app.UseMiddleware<ETagMiddleware>();
        app.UseResponseCompression();
        app.MapControllers();
        app.Run();
    }
}