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
using Xamarin.Essentials;

namespace PrayPal.AppSettings
{
    public class SettingsViewModel : PageViewModelBase
    {
        private readonly Command _goToLockScreenSettingsCommand;
        private readonly Command _goToLocationSettingsCommand;

        private readonly ITimeService _timeService;
        private bool _showVeanenuSetting;

        public SettingsViewModel(ITimeService timeService, ILogger<SettingsViewModel> logger)
            : base(AppResources.Settings, logger)
        {
            _goToLockScreenSettingsCommand = new Command(ExecuteGoToLockScreenSettings);
            _goToLocationSettingsCommand = new Command(ExecuteGoToLocationSettings);
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

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
                    var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                    if (status != PermissionStatus.Granted)
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
                //App.Current.UserAppTheme = value == true? .SetTheme();
                //IsolatedStorageSettings.ApplicationSettings["UseLightBackground"] = value;
                //RaisePropertyChanged("UseLightBackground");
                //Save();

                ////System.Windows.Application.Current.Resources.Remove("PhoneBackgroundColor");
                ////System.Windows.Application.Current.Resources.Remove("PhoneForegroundColor");

                //if (value)
                //{
                //    ((Microsoft.Phone.Controls.TransitionFrame)System.Windows.Application.Current.RootVisual).Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                //    ((Microsoft.Phone.Controls.TransitionFrame)System.Windows.Application.Current.RootVisual).Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                //    //System.Windows.Application.Current.Resources.Add("PhoneBackgroundColor", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White));
                //    //System.Windows.Application.Current.Resources.Add("PhoneForegroundColor", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black));
                //}
                //else
                //{
                //    ((Microsoft.Phone.Controls.TransitionFrame)System.Windows.Application.Current.RootVisual).ClearValue(System.Windows.Controls.Page.BackgroundProperty);//.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Application.Current.Resources["PhoneBackgroundColor"]);
                //    ((Microsoft.Phone.Controls.TransitionFrame)System.Windows.Application.Current.RootVisual).ClearValue(System.Windows.Controls.Page.ForegroundProperty);//.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Application.Current.Resources["PhoneForegroundColor"]);
                //    //System.Windows.Application.Current.Resources.Add("PhoneBackgroundColor", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black));
                //    //System.Windows.Application.Current.Resources.Add("PhoneForegroundColor", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White));
                //}

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

        public Command GoToLockScreenSettingsCommand
        {
            get { return _goToLockScreenSettingsCommand; }
        }

        public Command GoToLocationSettingsCommand
        {
            get { return _goToLocationSettingsCommand; }
        }

        private async void ExecuteGoToLockScreenSettings()
        {
            //await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
        }

        private async void ExecuteGoToLocationSettings()
        {
            //await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        //private void Save()
        //{
        //    IsolatedStorageSettings.ApplicationSettings.Save();
        //}
    }
}
