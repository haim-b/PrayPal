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
        private readonly ObservableCollection<ItemViewModel> _items;
        private readonly ITimeService _timeService;

        public PrayersViewModel(ITimeService timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get { return _items; }
        }

        public async Task GenerateContentAsync()
        {

            _items.Add(new ItemViewModel("Shacharit", CommonResources.ShacharitTitle));
            _items.Add(new ItemViewModel("BirkatHamazon", AppResources.BirkatHamazonTitle));
            _items.Add(new ItemViewModel("MeeinShalosh", AppResources.MeeinShaloshTitle));
            _items.Add(new ItemViewModel("Mincha", CommonResources.MinchaTitle));
            _items.Add(new ItemViewModel("Arvit", GetArvitTitle()));
            _items.Add(new ItemViewModel("TfilatHaderech", AppResources.TfilatHaderechTitle));
            _items.Add(new ItemViewModel("BedtimeShma", AppResources.BedtimeShmaTitle));

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
            JewishCalendar jc = _timeService.GetDayInfoAsync(null, null, true);

            if (jc.Chanukah)
            {
                ItemViewModel item = new ItemViewModel("HannukahCandles", AppResources.HadlakatNerotHannukahTitle); ;

                _items.Add(item);
            }
        }

    }
}
