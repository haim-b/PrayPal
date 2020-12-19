using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Prayers;
using PrayPal.Resources;
using PrayPal.TextPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Yahadut.DayTimes;
using Zmanim.HebrewCalendar;

namespace PrayPal.SummaryView
{
    public class SummaryPageViewModel : ScreenPageViewModelBase, IContentPage

    {
        private readonly ILocationService _locationService;
        private readonly ITimeService _timeService;
        private readonly INavigationService _navigationService;
        protected readonly ObservableCollection<SummaryViewItem> _times = new ObservableCollection<SummaryViewItem>();
        private string _errorMessage;

        private string _dateTitle;

        public SummaryPageViewModel(ILocationService locationService, ITimeService timeService, INavigationService navigationService, ILogger<SummaryPageViewModel> logger)
            : base(AppResources.SummaryViewTitle, logger)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            ItemTappedCommand = new Command<PrayerItemViewModel>(OnItemTappedExecuted);
        }

        public ObservableCollection<SummaryViewItem> Times
        {
            get { return _times; }
        }

        public Command<PrayerItemViewModel> ItemTappedCommand { get; }

        public string DateTitle
        {
            get { return _dateTitle; }
            set
            {
                if (_dateTitle != value)
                {
                    SetProperty(ref _dateTitle, value);
                }
            }
        }

        public async Task GenerateZmanimAsync()
        {
            if (!Settings.UseLocation)
            {
                ShowError(AppResources.NeedToEnableAppLocationMessage);
                return;
            }

            Geoposition position = await _locationService.GetCurrentPositionAsync();

            if (position == null)
            {
                ShowError(AppResources.AqcuiringCurrentLocationFailed);
                return;
            }

            ShowError(null);

            try
            {
                JewishCalendar jc = (await _timeService.GetDayInfoAsync()).JewishCalendar;

                CultureInfo ci = new CultureInfo("he-IL");
                //ci.DateTimeFormat.Calendar = new HebrewCalendar();
                //ci.DateTimeFormat.LongDatePattern = "dddd, dd MMMM yyyy";

                Calendar hebrewCalendar = ci.OptionalCalendars.ElementAtOrDefault(1);

                if (hebrewCalendar != null)
                {
                    ci.DateTimeFormat.Calendar = hebrewCalendar;
                }

                //DateTime now = HebDateHelper.GetCurrentDateTime(position);
                //ITimeZone timeZone = new WindowsTimeZone(System.TimeZoneInfo.Local);
                //GeoLocation location = new GeoLocation(string.Empty, position.Latitude, position.Longitude, position.Altitude, timeZone);
                //DateTime time = HebDateHelper.GetCurrentDateTime();
                //ComplexZmanimCalendar zc = new ComplexZmanimCalendar(time, location);

                _times.Clear();

                if (jc == null)
                {
                    DateTitle = null;
                    return;
                }

                HebrewDateFormatter formatter = new HebrewDateFormatter
                {
                    HebrewFormat = true
                };

                List<ItemViewModel> nowItems = await GetNowItemsAsync(position, jc);
                List<string> nowItemNames = nowItems.Select(p => p.Title).ToList();

                SummaryViewItem now = new SummaryViewItem(AppResources.SummaryNowTitle);
                nowItems.ForEach(i => now.Add(i));

                List<ItemViewModel> todayItems = await GetTodayItemsAsync(position, jc, formatter);

                SummaryViewItem today = new SummaryViewItem(AppResources.SummaryTodayTitle);

                // Make sure to add only items that don't already appear on Now:
                foreach (var item in todayItems)
                {
                    if (nowItemNames.Contains(item.Title))
                    {
                        continue;
                    }

                    today.Add(item);
                }

                List<ItemViewModel> weekItems = await GetWeekItemsAsync(position, jc);

                SummaryViewItem week = new SummaryViewItem(AppResources.SummaryThisWeekTitle);
                weekItems.ForEach(i => week.Add(i));

                _times.Add(now);
                _times.Add(today);
                _times.Add(week);

                DateTitle = jc.Time.ToString("dddd", ci) + ", " + formatter.format(jc);//now.ToString("D", ci.DateTimeFormat);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to generate times.");
            }
        }

        private async Task<List<ItemViewModel>> GetNowItemsAsync(Geoposition position, JewishCalendar jc)
        {
            List<ItemViewModel> items = new List<ItemViewModel>();

            var nextPrayer = await _timeService.GetCurrentPrayerAsync(position);

            if (nextPrayer != null && nextPrayer.Prayer != Prayer.Mussaf)
            {
                AddPrayerInfo(nextPrayer, items);

                if (nextPrayer.Prayer == Prayer.Arvit)
                {
                    if (jc.Chanukah)
                    {
                        items.Add(CreateHannukahCandleLighting(jc, nextPrayer));
                    }

                    items.Add(new PrayerItemViewModel(AppResources.BedtimeShmaTitle, AppResources.BedtimeShmaTitle));
                }
            }

            items.Add(new PrayerItemViewModel(PrayerNames.BirkatHamazon, AppResources.BirkatHamazonTitle, AppResources.BirkatHaMazonEndTimeInstruction));

            return items;
        }

        private ItemViewModel CreateHannukahCandleLighting(JewishCalendar jc, PrayerInfo arvitInfo)
        {
            string candleTitle = CommonResources.ResourceManager.GetString("Chanuka" + jc.DayOfChanukah);

            return new PrayerItemViewModel(PrayerNames.HannukahCandles, candleTitle, string.Format("{0:t}-{1:t}", arvitInfo.Start, arvitInfo.End));
        }

        private async Task<List<ItemViewModel>> GetTodayItemsAsync(Geoposition position, JewishCalendar jc, HebrewDateFormatter formatter)
        {
            List<ItemViewModel> items = new List<ItemViewModel>();

            string yomTov = HebDateHelper.GetMoedTitle(jc, true);

            if (!string.IsNullOrEmpty(yomTov))
            {
                items.Add(new ItemViewModel(yomTov));
            }

            items.Add(new ItemViewModel(GetDafYomiText(jc, formatter), CommonResources.DafYomiTitle));

            AddPrayerInfo(await _timeService.GetShacharitInfoAsync(position, jc.Time), items);

            AddPrayerInfo(await _timeService.GetMinchaInfoAsync(position, jc.Time), items);

            var arvitInfo = await _timeService.GetArvitInfoAsync(position, jc.Time);
            AddPrayerInfo(arvitInfo, items);

            DateTime? sunrise = await _timeService.GetSunriseAsync(position);

            if (sunrise != null)
            {
                items.Add(new ItemViewModel(AppResources.Sunrise, sunrise.Value.ToString("t")));
            }

            DateTime? sunset = await _timeService.GetSunsetAsync(position);

            if (sunset != null)
            {
                items.Add(new ItemViewModel(AppResources.Sunset, sunset.Value.ToString("t")));
            }

            int omer = jc.DayOfOmer;

            if (omer == 1)
            {
                items.Add(new ItemViewModel(CommonResources.Omer1));
            }
            else if (omer > 1)
            {
                items.Add(new ItemViewModel(string.Format(CommonResources.OmerShort, omer)));
            }

            return items;
        }

        private async Task<List<ItemViewModel>> GetWeekItemsAsync(Geoposition position, JewishCalendar jc)
        {
            List<ItemViewModel> items = new List<ItemViewModel>();

            try
            {
                string parasha = HebDateHelper.GetParasha(jc);

                if (!string.IsNullOrEmpty(parasha))
                {
                    items.Add(new ItemViewModel(string.Format(CommonResources.ParashaNameTitleFormat, parasha)));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to calculate the Parasha.");
            }

            DateTime? knissatShabbat = await _timeService.GetKnissatShabbatAsync(position, jc.Time);

            if (knissatShabbat != null)
            {
                items.Add(new ItemViewModel(CommonResources.KnissatShabbat, string.Format("‭{0:t}", knissatShabbat.Value)));
            }

            return items;
        }

        private void AddPrayerInfo(PrayerInfo prayer, List<ItemViewModel> items)
        {
            if (prayer == null)
            {
                return;
            }

            items.Add(new PrayerItemViewModel(GetPrayerPageName(prayer.Prayer), prayer.PrayerName, string.Format("‭{0:t}-{1:t}", prayer.Start, prayer.End)));

            if (!string.IsNullOrEmpty(prayer.ExtraInfo))
            {
                string[] parts = prayer.ExtraInfo.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                {
                    items.Add(new ItemViewModel(prayer.ExtraInfo));
                }
                else
                {
                    items.Add(new ItemViewModel(parts[0], parts[1]));
                }
            }
        }

        private string GetPrayerPageName(Prayer prayer)
        {
            switch (prayer)
            {
                case Prayer.Shacharit:
                    return PrayerNames.Shacharit;
                case Prayer.Mincha:
                    return PrayerNames.Mincha;
                case Prayer.Arvit:
                    return PrayerNames.Arvit;
                default:
                    return null;
            }
        }


        private string GetDafYomiText(JewishCalendar jc, HebrewDateFormatter formatter)
        {
            JewishCalendar nowBasedOnGregorian = new JewishCalendar(DateTime.Now, Settings.IsInIsrael);

            if (jc.Time == nowBasedOnGregorian.Time)
            {
                return HebDateHelper.GetDafYomi(nowBasedOnGregorian);
            }

            return HebDateHelper.GetDafYomi(nowBasedOnGregorian) + " (" + formatter.format(nowBasedOnGregorian) + ")";
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

        public async Task GenerateContentAsync()
        {
            await GenerateZmanimAsync();
        }

        protected override async Task OnSettingsChangedAsync(string settingsName)
        {
            if (!Settings.TimeAffecingSettings.Contains(settingsName))
            {
                return;
            }

            await GenerateContentAsync();
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
            RaisePropertyChanged(nameof(HasErrors));
        }

        public bool HasErrors
        {
            get { return !string.IsNullOrEmpty(_errorMessage); }
        }
    }

    public class SummaryViewItem : ObservableCollection<ItemViewModel>
    {
        public SummaryViewItem(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
