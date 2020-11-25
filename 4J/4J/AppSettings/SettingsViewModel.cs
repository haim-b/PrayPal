using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using PrayPal.Content;
using PrayPal.Models;
using PrayPal.Resources;
using PrayPal.Common.Services;
using Microsoft.Extensions.Logging;
using PrayPal.Common;

namespace PrayPal.AppSettings
{
    public class SettingsViewModel : ScreenPageViewModelBase
    {
        //private readonly Command _goToLockScreenSettingsCommand;
        //private readonly Command _goToLocationSettingsCommand;

        private readonly ITimeService _timeService;
        private readonly IPermissionsService _permissionsService;
        private bool _showVeanenuSetting;

        public SettingsViewModel(ITimeService timeService, IPermissionsService permissionsService, ILogger<SettingsViewModel> logger)
            : base(AppResources.Settings, logger)
        {
            //_goToLockScreenSettingsCommand = new Command(ExecuteGoToLockScreenSettings);
            //_goToLocationSettingsCommand = new Command(ExecuteGoToLocationSettings);
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _permissionsService = permissionsService ?? throw new ArgumentNullException(nameof(permissionsService));
            ShowUseLightBackgroundSetting = App.Current.RequestedTheme != OSAppTheme.Light;

            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                ShowVeanenuSetting = (await _timeService.GetDayInfoAsync()).IsVetenBracha();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to initialize settings view model.");
            }
        }

        public bool UseLocation
        {
            get { return Settings.UseLocation; }
            set
            {
                SetUseLocation(value);
            }
        }

        public string MailTo
        {
            get { return "mailto:" + AppResources.EmailAddress; }
        }

        private async void SetUseLocation(bool useLocation)
        {
            try
            {
                if (useLocation)
                {
                    if (!(await _permissionsService.RequestAsync(Permissions.Location)))
                    {
                        return;
                    }
                }


                Settings.UseLocation = useLocation;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Set location exception.");
                return;
            }
            finally
            {
                RaisePropertyChanged("UseLocation");
            }
        }

        public bool UseLightBackground
        {
            get { return Settings.UseLightBackground; }
            set
            {
                Settings.UseLightBackground = value;
            }
        }

        public int TimeCalcMethodIndex
        {
            get { return Settings.TimeCalcMethod == TimeCalcMethod.Gra ? 0 : 1; }
            set
            {
                Settings.TimeCalcMethod = value == 0 ? TimeCalcMethod.Gra : TimeCalcMethod.Mga;
                RaisePropertyChanged();
            }
        }

        public int Nusach
        {
            get { return (int)Settings.Nusach; }
            set
            {
                Settings.Nusach = (Nusach)value;
                RaisePropertyChanged();

                PrayersHelper.SetPrayerTextProvider((Nusach)value);
            }
        }

        public int UseLargeFont
        {
            get { return Settings.UseLargeFont ? 1 : 0; }
            set
            {
                Settings.UseLargeFont = value == 1;
                RaisePropertyChanged();

                ParagraphModel.SetFontSize();
            }
        }

        public List<string> TimeCalcMethods { get; } = new List<string>(2) { AppResources.Gra, AppResources.Mga };

        public List<string> FontSizes { get; } = new List<string>(2) { AppResources.FontSizeSettingLabelRegular, AppResources.FontSizeSettingLabelLarge };

        public List<string> Nusachim { get; } = new List<string>() { AppResources.NusachAshkenazTitle, AppResources.NusachSfardTitle, AppResources.NusachEdotMizrachTitle };

        public bool ShowVeanenu
        {
            get
            {
                return Settings.ShowVeanenu;
            }
            set
            {
                Settings.ShowVeanenu = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInChul
        {
            get { return !Settings.IsInIsrael; }
            set
            {
                bool val = !value;

                if (Settings.IsInIsrael != val)
                {
                    Settings.IsInIsrael = val;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShowUseLightBackgroundSetting { get; }

        public bool ShowVeanenuSetting
        {
            get
            {
                return _showVeanenuSetting;
            }
            set
            {
                _showVeanenuSetting = value;
                RaisePropertyChanged();
            }
        }

        //public Command GoToLockScreenSettingsCommand
        //{
        //    get { return _goToLockScreenSettingsCommand; }
        //}

        //public Command GoToLocationSettingsCommand
        //{
        //    get { return _goToLocationSettingsCommand; }
        //}

        //private async void ExecuteGoToLockScreenSettings()
        //{
        //    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
        //}

        //private async void ExecuteGoToLocationSettings()
        //{
        //    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        //}
    }
}
