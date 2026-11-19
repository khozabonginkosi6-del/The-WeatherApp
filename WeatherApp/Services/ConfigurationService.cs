namespace WeatherApp.Services
{
    public static class ConfigurationService
    {
        public const string BaseUrl =
            "https://api.openweathermap.org/data/2.5/weather";

        public const string ApiKey = "40a8b192f3362c114ca305d64e3f1f0a";

        public static string GetWeatherUrl(
            double latitude,
            double longitude)
        {
            return $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric";
        }
    }
}