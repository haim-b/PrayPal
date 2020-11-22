using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Resources;
using PrayPal.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PrayPal.Tools.PrayerDirection
{
    public class PrayerDirectionViewModel : PageViewModelBase
    {
        private double _JerusalemBearing;

        private readonly ILocationService _locationService;
        private readonly INavigationService _navigationService;

        const double Rad = Math.PI / 180;
        const double WesternWallLongitudeInRadians = 0.61495777349455427;
        //const double WesternWallSinLatitude = 0.52661232901178656;
        //const double WesternWallCosLatitude = 0.85010555517111042;
        const double HarHabaytSinLatitude = 0.52663443617369671836901193348503;
        const double HarHabaytCosLatitude = 0.85009186011631269903564888101303;

        private string _errorMessage;
        private bool _hasErrors;
        private double _angle;
        private bool _isCalibrationNeeded;

        private Command _calibrateCompassCommand;

        public PrayerDirectionViewModel(ILocationService locationService, INavigationService navigationService, INotificationService notificationService, IErrorReportingService errorReportingService, ILogger<PrayerDirectionViewModel> logger)
            : base(AppResources.PrayingDirectionTitle, notificationService, errorReportingService, logger)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            _calibrateCompassCommand = new Command(ExecuteCalibrateCompass);

            InitializePrivate();
        }

        private async void InitializePrivate()
        {
            if (!Settings.UseLocation)
            {
                ShowError(AppResources.NeedToEnableAppLocationMessage);
                return;
            }

            if (!_locationService.IsActive)
            {
                ShowError(AppResources.NeedToEnableSysLocationMessage);
                return;
            }
            //else if (geolocator.LocationStatus == PositionStatus.NotAvailable)
            //{
            //    ShowError(AppResources.LocationNotSupported);
            //    return;
            //}

            try
            {
                Geoposition position = await _locationService.GetCurrentPositionAsync();

                if (position == null)
                {
                    ShowError(AppResources.AqcuiringCurrentLocationFailed);
                    return;
                }

                CalculateBearing(position);

                Compass.ReadingChanged += OnCompassValueChanged;
                Compass.Start(SensorSpeed.UI, true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get location.");
                ShowError(ex.Message);
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                SetProperty(ref _errorMessage, value);
            }
        }


        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasErrors = !string.IsNullOrEmpty(message);
        }

        public bool HasErrors
        {
            get { return _hasErrors; }
            private set
            {
                SetProperty(ref _hasErrors, value);
            }
        }

        public Command CalibrateCompassCommand
        {
            get { return _calibrateCompassCommand; }
        }

        private async void ExecuteCalibrateCompass()
        {
            //await _navigationService.NavigateToAsync("CalibrateCompass", null);
        }

        private void CalculateBearing(Geoposition position)
        {
            double currentLat = position.Latitude * Rad;
            double currentLon = position.Longitude * Rad;

            double deltaLon = WesternWallLongitudeInRadians - currentLon;

            double y = Math.Sin(deltaLon) * HarHabaytCosLatitude;
            double x = Math.Cos(currentLat) * HarHabaytSinLatitude - Math.Sin(currentLat) * HarHabaytCosLatitude * Math.Cos(deltaLon);

            _JerusalemBearing = Math.Atan2(y, x) / Rad;

            // fix negative degrees
            if (_JerusalemBearing < 0)
            {
                _JerusalemBearing = 360 - Math.Abs(_JerusalemBearing);
            }
        }

        private void OnCompassValueChanged(object sender, CompassChangedEventArgs e)
        {
            CalculateAngle(e.Reading.HeadingMagneticNorth);//e.Reading.HeadingTrueNorth);
            //IsCalibrationNeeded = e.Reading.HeadingAccuracy == MagnetometerAccuracy.Unreliable;

            //if (e.Reading.HeadingAccuracy == MagnetometerAccuracy.Unknown)
            //{
            //    ShowError(AppResources.CompassCannotBeReadError);
            //}
        }

        private void CalculateAngle(double? trueHeading)
        {
            if (trueHeading == null)
            {
                return;
            }

            Angle = _JerusalemBearing - (double)trueHeading;
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                SetProperty(ref _angle, value);
            }
        }

        public bool IsCalibrationNeeded
        {
            get { return _isCalibrationNeeded; }
            set
            {
                SetProperty(ref _isCalibrationNeeded, value);
            }
        }
    }
}
