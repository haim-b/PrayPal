using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Prayers.MeeinShalosh;
using PrayPal.Resources;
using PrayPal.TextPresenter;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zmanim.HebrewCalendar;

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

        public async Task GenerateContentAsync()
        {
            Items.Clear();
            Items.Add(new PrayerItemViewModel(PrayerNames.Shacharit, CommonResources.ShacharitTitle));
            Items.Add(new PrayerItemViewModel(PrayerNames.BirkatHamazon, AppResources.BirkatHamazonTitle));
            Items.Add(new PrayerItemViewModel(PrayerNames.MeeinShalosh, AppResources.MeeinShaloshTitle, typeof(MeeinShaloshPageViewModel)));
            Items.Add(new PrayerItemViewModel(PrayerNames.Mincha, CommonResources.MinchaTitle));
            Items.Add(new PrayerItemViewModel(PrayerNames.Arvit, GetArvitTitle()));
            Items.Add(new PrayerItemViewModel(PrayerNames.TfilatHaDerech, AppResources.TfilatHaderechTitle));
            Items.Add(new PrayerItemViewModel(PrayerNames.BedtimeShma, AppResources.BedtimeShmaTitle));

            await HandleHannukahAsync();
        }


        private async void OnItemTappedExecuted(PrayerItemViewModel item)
        {
            if (item.ViewModelType != null)
            {
                await _navigationService.NavigateToAsync(item.ViewModelType.Name);
                return;
            }

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel), "textName", item.PageName);
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
                PrayerItemViewModel item = new PrayerItemViewModel(PrayerNames.HannukahCandles, AppResources.HadlakatNerotHannukahTitle); ;

                Items.Add(item);
            }
        }

        protected override async Task OnSettingsChangedAsync(string settingsName)
        {
            if (settingsName != nameof(Settings.UseLocation))
            {
                return;
            }

            await GenerateContentAsync();
        }

    }
}
