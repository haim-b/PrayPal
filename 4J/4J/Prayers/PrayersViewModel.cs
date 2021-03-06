﻿using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Prayers.MeeinShalosh;
using PrayPal.Resources;
using PrayPal.TextPresenter;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zmanim.HebrewCalendar;
using Microsoft.AppCenter.Analytics;
using Xamarin.Essentials;

namespace PrayPal.Prayers
{
    public class PrayersViewModel : ScreenPageViewModelBase, IContentPage
    {
        private readonly ITimeService _timeService;
        private readonly INavigationService _navigationService;

        public PrayersViewModel(ITimeService timeService, INavigationService navigationService, ILogger<PrayersViewModel> logger)
            : base(AppResources.PrayersAndGracesTitle, logger)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            Items = new ObservableCollection<PrayerItemViewModel>();

            ItemTappedCommand = new Command<PrayerItemViewModel>(OnItemTappedExecuted);
        }

        public ObservableCollection<PrayerItemViewModel> Items { get; }

        public Command<PrayerItemViewModel> ItemTappedCommand { get; }

        public bool ShowHeadersInstruction
        {
            get { return Preferences.Get(nameof(ShowHeadersInstruction), true); }
            set
            {
                Preferences.Set(nameof(ShowHeadersInstruction), value);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(ShowHeadersInstruction)));
            }
        }

        public async Task GenerateContentAsync()
        {
            Items.Clear();
            Items.Add(new PrayerItemViewModel(PrayerNames.Shacharit, CommonResources.ShacharitTitle, FormatPrayerTime(await _timeService.GetShacharitInfoAsync())));
            Items.Add(new PrayerItemViewModel(PrayerNames.BirkatHamazon, AppResources.BirkatHamazonTitle));
            Items.Add(new PrayerItemViewModel(PrayerNames.MeeinShalosh, AppResources.MeeinShaloshTitle, typeof(MeeinShaloshPageViewModel)));
            Items.Add(new PrayerItemViewModel(PrayerNames.Mincha, CommonResources.MinchaTitle, FormatPrayerTime(await _timeService.GetMinchaInfoAsync())));
            Items.Add(new PrayerItemViewModel(PrayerNames.Arvit, GetArvitTitle(), FormatPrayerTime(await _timeService.GetMinchaInfoAsync())));
            Items.Add(new PrayerItemViewModel(PrayerNames.TfilatHaDerech, AppResources.TfilatHaderechTitle));
            Items.Add(new PrayerItemViewModel(PrayerNames.BedtimeShma, AppResources.BedtimeShmaTitle));

            await HandleHannukahAsync();
        }

        private string FormatPrayerTime(PrayerInfo prayerInfo)
        {
            return prayerInfo?.Start.ToString();
        }

        private async void OnItemTappedExecuted(PrayerItemViewModel item)
        {
            if (item == null)
            {
                return;
            }

            Logger.LogInformation("Opened " + item.PageName);
            Analytics.TrackEvent("Opened prayer", Utils.AnalyticsProperty("prayer", item.PageName));

            if (item.ViewModelType != null)
            {
                await _navigationService.NavigateToAsync(item.ViewModelType.Name);
                return;
            }

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel), TextPresenterViewModel.TextNameParam, item.PageName);
        }

        private static string GetArvitTitle()
        {
            if (Settings.Nusach == Nusach.Ashkenaz)
            {
                return CommonResources.MaarivTitle;
            }
            else
            {
                return CommonResources.ArvitTitle;
            }
        }

        private async Task HandleHannukahAsync()
        {
            JewishCalendar jc = (await _timeService.GetDayInfoAsync(null, null, true)).JewishCalendar;

            if (jc.Chanukah)
            {
                PrayerItemViewModel item = new PrayerItemViewModel(PrayerNames.HannukahCandles, AppResources.HadlakatNerotHannukahTitle);

                Items.Add(item);
            }
        }

        protected override async Task OnSettingsChangedAsync(string settingsName)
        {
            if (!Settings.TimeAffecingSettings.Contains(settingsName))
            {
                return;
            }

            await GenerateContentAsync();
        }

    }
}
