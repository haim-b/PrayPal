using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrayPal.Common.Services
{
    public class LocationService : ILocationService
    {
        private readonly TimeSpan _locationTimeout;
        private readonly ILogger _logger;

        public LocationService(ILogger<LocationService> logger)
        {
            //#if DEBUG
            //            _locationTimeout = TimeSpan.FromSeconds(20);
            //#else
            _locationTimeout = TimeSpan.FromSeconds(7);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //#endif

        }

        public async Task<Geoposition> GetCurrentPositionAsync(CancellationToken cancellationToken = default)
        {
            if (!Settings.UseLocation)
            {
                return null;
            }

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Low, _locationTimeout);
                var location = await Geolocation.GetLocationAsync(request, cancellationToken);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }

                return new Geoposition(location.Longitude, location.Latitude, location.Altitude);
            }
            catch (Exception ex) when (ex is FeatureNotSupportedException
                    || ex is FeatureNotEnabledException
                    || ex is PermissionException pEx)
            {
                Settings.UseLocation = false;
                _logger.LogError(ex, "Location is unavailable. Turning it off.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get location.");
                return null;
            }
        }
    }
}
