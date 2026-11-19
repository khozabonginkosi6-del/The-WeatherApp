using System.Net.Http;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherAPIHttpService
    {
        private readonly HttpClient _httpClient;

        public WeatherAPIHttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherAsync(
            double latitude,
            double longitude)
        {
            string url = ConfigurationService.GetWeatherUrl(
                latitude,
                longitude);

            HttpResponseMessage response =
                await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content.ReadAsStringAsync();

            WeatherData? weather =
                JsonSerializer.Deserialize<WeatherData>(json);

            if (weather == null)
            {
                throw new Exception(
                    "Unable to retrieve weather information.");
            }

            return weather;
        }
    }
}