using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weather.Services;
using Weather.Workers;

namespace Weather
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            
            services.AddHttpClient("AccuWeather", (client) =>
            {
                client.BaseAddress = new Uri(_configuration["weather:uri"]);
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            
            services.AddHttpClient("ClimateControl", (client) =>
            {
                // options.Address = new Uri("http://localhost:5040");
                client.BaseAddress = _configuration.GetServiceUri("climatecontrol");
            });

            services.AddMemoryCache();
            services.AddHostedService<WeatherWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<WeatherService>();

                endpoints.MapGet("/proto", async req =>
                {
                    await req.Response.SendFileAsync("Protos/weather.proto", req.RequestAborted);
                });

                endpoints.MapGet("/", async req => await req.Response.WriteAsync("Healthy"));
            });
        }
    }
}
