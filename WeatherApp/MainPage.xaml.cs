
using System;
using System.Threading.Tasks;
using WeatherApp.Services;
using Microsoft.Maui.Devices.Sensors;

namespace WeatherApp;

public partial class MainPage : ContentPage
{
    private readonly LocationService _locationService;
    private readonly WeatherAPIHttpService _weatherService;

    public MainPage()
    {
        InitializeComponent();

        DateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");

        _locationService = new LocationService();
        _weatherService = new WeatherAPIHttpService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadWeatherAsync();
    }

    private async void OnRefreshWeatherClicked(
        object sender, EventArgs e)
    {
        await LoadWeatherAsync();
    }

    private async Task LoadWeatherAsync()
    {
        try
        {
            RefreshButton.IsEnabled = false;
            RefreshButton.Text = "Loading...";

            Location? location =
                await _locationService.GetCurrentLocationAsync();

            if (location == null)
            {
                throw new Exception("Unable to detect your current location.");
            }

            var weather = await _weatherService.GetWeatherAsync(
                location.Latitude,
                location.Longitude);

            LocationLabel.Text =
                $"{weather.Name}, {weather.Sys.Country}";

            TemperatureLabel.Text =
                $"{weather.Main.Temperature:F1}°C";

            FeelsLikeLabel.Text =
                $"Feels like: {weather.Main.FeelsLike:F1}°C";

            HumidityLabel.Text =
                $"{weather.Main.Humidity}%";

            WindLabel.Text =
                $"{weather.Wind.Speed:F1} m/s";

            PressureLabel.Text =
                $"{weather.Main.Pressure} hPa";

            CountryLabel.Text =
                weather.Sys.Country;

            if (weather.Weather.Count > 0)
            {
                DescriptionLabel.Text =
                    weather.Weather[0].Description;

                WeatherIconLabel.Text =
                    GetWeatherEmoji(weather.Weather[0].Icon);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Weather Error",
                ex.Message,
                "OK");
        }
        finally
        {
            RefreshButton.IsEnabled = true;
            RefreshButton.Text = "Refresh Weather";
        }
    }

    private static string GetWeatherEmoji(string icon)
    {
        return icon.StartsWith("01") ? "☀️" :
               icon.StartsWith("02") ? "🌤️" :
               icon.StartsWith("03") || icon.StartsWith("04") ? "☁️" :
               icon.StartsWith("09") || icon.StartsWith("10") ? "🌧️" :
               icon.StartsWith("11") ? "⛈️" :
               icon.StartsWith("13") ? "❄️" :
               icon.StartsWith("50") ? "🌫️" : "🌤️";
    }
}
