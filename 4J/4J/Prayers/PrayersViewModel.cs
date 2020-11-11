using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
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
    public class PrayersViewModel : PageViewModelBase, IContentPage
    {
        private readonly ITimeService _timeService;
        private readonly INavigationService _navigationService;

        public PrayersViewModel(ITimeService timeService, INavigationService navigationService, ILogger<PrayersViewModel> logger)
            : base(AppResources.PrayersAndGracesTitle, logger)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            Items = new ObservableCollection<ItemViewModel>();

            ItemTappedCommand = new Command<ItemViewModel>(OnItemTappedExecuted);
        }

        public ObservableCollection<ItemViewModel> Items { get; }

        public Command<ItemViewModel> ItemTappedCommand { get; }

        public async Task GenerateContentAsync()
        {

            Items.Add(new ItemViewModel(PrayerNames.Shacharit, CommonResources.ShacharitTitle));
            Items.Add(new ItemViewModel(PrayerNames.BirkatHamazon, AppResources.BirkatHamazonTitle));
            Items.Add(new ItemViewModel(PrayerNames.MeeinShalosh, AppResources.MeeinShaloshTitle));
            Items.Add(new ItemViewModel(PrayerNames.Mincha, CommonResources.MinchaTitle));
            Items.Add(new ItemViewModel(PrayerNames.Arvit, GetArvitTitle()));
            Items.Add(new ItemViewModel(PrayerNames.TfilatHaderech, AppResources.TfilatHaderechTitle));
            Items.Add(new ItemViewModel(PrayerNames.BedtimeShma, AppResources.BedtimeShmaTitle));

            await HandleHannukahAsync();
        }


        private async void OnItemTappedExecuted(ItemViewModel item)
        {
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
                ItemViewModel item = new ItemViewModel(PrayerNames.HannukahCandles, AppResources.HadlakatNerotHannukahTitle); ;

                Items.Add(item);
            }
        }

    }
}
