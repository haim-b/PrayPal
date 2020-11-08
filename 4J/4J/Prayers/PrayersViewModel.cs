using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Zmanim.HebrewCalendar;

namespace PrayPal.Prayers
{
    public class PrayersViewModel : BindableBase, IContentPage
    {
        private readonly ITimeService _timeService;

        public PrayersViewModel(ITimeService timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            Items = new ObservableCollection<ItemViewModel>();
            Title = AppResources.PrayersAndGracesTitle;
        }

        public string Title { get; }

        public ObservableCollection<ItemViewModel> Items { get; }

        public async Task GenerateContentAsync()
        {

            Items.Add(new ItemViewModel("Shacharit", CommonResources.ShacharitTitle));
            Items.Add(new ItemViewModel("BirkatHamazon", AppResources.BirkatHamazonTitle));
            Items.Add(new ItemViewModel("MeeinShalosh", AppResources.MeeinShaloshTitle));
            Items.Add(new ItemViewModel("Mincha", CommonResources.MinchaTitle));
            Items.Add(new ItemViewModel("Arvit", GetArvitTitle()));
            Items.Add(new ItemViewModel("TfilatHaderech", AppResources.TfilatHaderechTitle));
            Items.Add(new ItemViewModel("BedtimeShma", AppResources.BedtimeShmaTitle));

            await HandleHannukahAsync();
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
                ItemViewModel item = new ItemViewModel("HannukahCandles", AppResources.HadlakatNerotHannukahTitle); ;

                Items.Add(item);
            }
        }

    }
}
