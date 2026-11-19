
using Microsoft.Maui.Devices.Sensors;

namespace WeatherApp.Services
{
    public class LocationService
    {
        public async Task<Location?> GetCurrentLocationAsync()
        {
            try
            {
                PermissionStatus status = await Permissions
                    .CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions
                        .RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    throw new Exception(
                        "Location permission was not granted.");
                }

                if (!Geolocation.Default.IsEnabled)
                {
                    throw new Exception(
                        "Please enable location services on your device.");
                }

                GeolocationRequest request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(20));

                Location? location = await Geolocation.Default
                    .GetLocationAsync(request);

                if (location == null)
                {
                    throw new Exception(
                        "Unable to retrieve your current location. Please try again.");
                }

                return location;
            }
            catch (FeatureNotSupportedException)
            {
                throw new Exception(
                    "GPS is not supported on this device.");
            }
            catch (FeatureNotEnabledException)
            {
                throw new Exception(
                    "Please enable location services on your device.");
            }
            catch (PermissionException)
            {
                throw new Exception(
                    "Location permission was denied.");
            }
            catch (TimeoutException)
            {
                throw new Exception(
                    "Location request timed out. Please try again.");
            }
        }
    }
}
