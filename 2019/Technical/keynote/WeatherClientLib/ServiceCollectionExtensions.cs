using System;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using static Weather.Weather;

namespace WeatherClientLib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcWeatherForecastService(
            this IServiceCollection services, Action<GrpcClientFactoryOptions> configure)
        {
            services.AddGrpcClient<WeatherClient>(configure);
            services.AddScoped<IWeatherForecastService, GrpcWeatherForecastService>();
            return services;
        }
    }
}
