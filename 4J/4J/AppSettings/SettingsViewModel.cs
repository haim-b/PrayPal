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
using System.Linq;

namespace PrayPal.AppSettings
{
    public class SettingsViewModel : ScreenPageViewModelBase
    {
        //private readonly Command _goToLockScreenSettingsCommand;
        //private readonly Command _goToLocationSettingsCommand;

        private readonly ITimeService _timeService;
        private readonly IPermissionsService _permissionsService;
        private List<PermissionInfo> _permissionsInfo;
        private bool _showVeanenuSetting;
        private bool _arePermissionsRequired;


        public SettingsViewModel(ITimeService timeService, IPermissionsService permissionsService, ILogger<SettingsViewModel> logger)
            : base(AppResources.Settings, logger)
        {
            //_goToLockScreenSettingsCommand = new Command(ExecuteGoToLockScreenSettings);
            //_goToLocationSettingsCommand = new Command(ExecuteGoToLocationSettings);
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _permissionsService = permissionsService ?? throw new ArgumentNullException(nameof(permissionsService));
            ShowUseLightBackgroundSetting = App.Current.RequestedTheme != OSAppTheme.Light;
            RequestPermissionsCommand = new Command(RequestPermissions);

            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                ShowVeanenuSetting = (await _timeService.GetDayInfoAsync()).IsVetenBracha();

                List<PermissionInfo> permissions = new List<PermissionInfo>
                {
                    new PermissionInfo(Permissions.Location, AppResources.LocationPermissionTitle, AppResources.UseLocationPrivacyPolicy, await _permissionsService.IsAllowedAsync(Permissions.Location)),
                    new PermissionInfo(Permissions.Camera, AppResources.CameraPermissionTitle, AppResources.CameraPermissionReason, await _permissionsService.IsAllowedAsync(Permissions.Camera))
                };

                PermissionsInfo = permissions;
                ArePermissionsRequired = permissions.Any(p => !p.IsAllowed);

                Settings.UseLocation = permissions.First(p => p.Name == Permissions.Location).IsAllowed;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to initialize settings view model.");
            }
        }

        //public bool UseLocation
        //{
        //    get { return Settings.UseLocation; }
        //    set
        //    {
        //        SetUseLocation(value);
        //    }
        //}

        public List<PermissionInfo> PermissionsInfo
        {
            get { return _permissionsInfo; }
            set { SetProperty(ref _permissionsInfo, value); }
        }

        public bool ArePermissionsRequired
        {
            get { return _arePermissionsRequired; }
            set { SetProperty(ref _arePermissionsRequired, value); }
        }


        public Command RequestPermissionsCommand { get; }

        public string MailTo
        {
            get { return "mailto:" + AppResources.EmailAddress; }
        }

        //private async void SetUseLocation(bool useLocation)
        //{
        //    try
        //    {
        //        if (useLocation)
        //        {
        //            if (!(await _permissionsService.RequestAsync(Permissions.Location)))
        //            {
        //                return;
        //            }
        //        }


        //        Settings.UseLocation = useLocation;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Set location exception.");
        //        return;
        //    }
        //    finally
        //    {
        //        RaisePropertyChanged("UseLocation");
        //    }
        //}

        public int Theme
        {
            get { return (int)Settings.Theme; }
            set
            {
                Settings.Theme = (Theme)value;
                RaisePropertyChanged();
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

        public List<string> Themes { get; } = new List<string>() { AppResources.ThemeNameFromOS, AppResources.ThemeNameLight, AppResources.ThemeNameDark };

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

        private async void RequestPermissions()
        {
            try
            {
                await _permissionsService.RequestAsync(Permissions.Location);
                await _permissionsService.RequestAsync(Permissions.Camera);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to request permissions.");
            }

            // Refresh the permissions info:
            Initialize();
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

    public class PermissionInfo : BindableBase
    {
        private bool _isAllowed;

        public Permissions Name { get; }

        public string Title { get; }

        public string Reason { get; }

        public PermissionInfo(Permissions name, string title, string reason, bool isAllowed)
        {
            Name = name;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
            _isAllowed = isAllowed;
        }

        public bool IsAllowed
        {
            get { return _isAllowed; }
            set { SetProperty(ref _isAllowed, value); }
        }

    }
}
