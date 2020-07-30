using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Weather;
using static Weather.Weather;

namespace WeatherClientLib
{
    public class GrpcWeatherForecastService : IWeatherForecastService
    {
        private readonly WeatherClient _weatherClient;

        public GrpcWeatherForecastService(WeatherClient weatherClient)
        {
            _weatherClient = weatherClient;
        }

        public async Task<WeatherResponse> GetWeather()
        {
            return await _weatherClient.GetWeatherAsync(new Empty());
        }

        public IAsyncEnumerable<WeatherResponse> GetStreamingWeather(CancellationToken token)
        {
            return _weatherClient.GetWeatherStream(new Empty(), cancellationToken: token)
                .ResponseStream.ReadAllAsync();
        }
    }
}
