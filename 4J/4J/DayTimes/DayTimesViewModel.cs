﻿using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yahadut.DayTimes;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.DayTimes
{
    public class DayTimesViewModel : BindableBase
    {
        protected readonly ILocationService _locationService;
        protected readonly ITimeService _timeService;
        protected readonly ObservableCollection<TimeOfDay> _zmanim = new ObservableCollection<TimeOfDay>();

        private string _title;

        public DayTimesViewModel(ILocationService locationService, ITimeService timeService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

            ShowRelativePrayers = true;
            IncludeIsruChag = true;
            Title = AppResources.ZmaneyHayom;
        }

        public ObservableCollection<TimeOfDay> Zmanim
        {
            get { return _zmanim; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    SetProperty(ref _title, value);
                }
            }
        }

        protected virtual async void OnLocationCalculated(Geoposition position)
        {
            await GenerateZmanim(position, _jewishDate, _jewishDate);
        }

        private JewishCalendar _jewishDate;

        public JewishCalendar JewishDate
        {
            get { return _jewishDate; }
            set
            {
                _jewishDate = value;
                OnJewishDateChanged(value);
            }
        }

        private async void OnJewishDateChanged(JewishCalendar value)
        {
            await GenerateZmanim(await _locationService.GetCurrentPositionAsync(), value, value);
        }

        public bool ShowRelativePrayers { get; set; }

        public bool ShowPrayersTime { get; set; }

        public bool IncludeIsruChag { get; set; }

        public bool ShowGregorianDate { get; set; }

        protected async Task GenerateZmanim(Geoposition position, JewishCalendar jc, JewishCalendar dafYomiDate)
        {
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

            _zmanim.Clear();

            if (jc == null)
            {
                Title = null;
                return;
            }

            HebrewDateFormatter formatter = new HebrewDateFormatter();
            formatter.HebrewFormat = true;

            if (ShowGregorianDate)
            {
                _zmanim.Add(new TimeOfDay(jc.Time.ToString("d")));
            }

            string yomTov = HebDateHelper.GetMoedTitle(jc, IncludeIsruChag);

            if (!string.IsNullOrEmpty(yomTov))
            {
                _zmanim.Add(new TimeOfDay(yomTov));
            }

            string parasha = HebDateHelper.GetParasha(jc);

            if (!string.IsNullOrEmpty(parasha))
            {
                _zmanim.Add(new TimeOfDay(string.Format(CommonResources.ParashaNameTitleFormat, parasha)));
            }

            _zmanim.Add(new TimeOfDay(CommonResources.DafYomiTitle + ": " + GetDafYomiText(jc, dafYomiDate, formatter)));

            //_zmanim.Add(new ZmanHayom(string.Format("{0}: ‭{1:t}", AppResources.EndTimeOfShma, GetValue(zc.GetSofZmanShmaGRA()).ToString("t"))));
            //_zmanim.Add(new ZmanHayom(AppResources.EndTimeOfPrayer, GetValue(zc.GetSofZmanTfilaGRA())));

            if (ShowPrayersTime)
            {
                //ComplexZmanimCalendar calendar = await _timeService.GetCurrentZmanimCalendarAsync(ShowRelativePrayers ? (DateTime?)jc.Time : null, position);

                AddPrayerInfo(await _timeService.GetShacharitInfoAsync(position, ShowRelativePrayers ? (DateTime?)jc.Time : null));
                //_zmanim.Add(new ZmanHayom(string.Format("{0}: ‭{1:t}", AppResources.EndTimeOfShma, Settings.Instance.TimeCalcMethod == "MGA" ? calendar?.GetSofZmanShmaMGA() : calendar?.GetSofZmanShmaGRA())));
                AddPrayerInfo(await _timeService.GetMinchaInfoAsync(position, ShowRelativePrayers ? (DateTime?)jc.Time : null));
                AddPrayerInfo(await _timeService.GetArvitInfoAsync(position, ShowRelativePrayers ? (DateTime?)jc.Time : null));
            }

            DateTime? sunset = await _timeService.GetSunsetAsync(position);

            if (sunset != null)
            {
                _zmanim.Add(new TimeOfDay(AppResources.Sunset + ": " + sunset.Value.ToString("t")));
            }

            DateTime? knissatShabbat = await _timeService.GetKnissatShabbatAsync(position, jc.Time);

            if (knissatShabbat != null)
            {
                _zmanim.Add(new TimeOfDay(string.Format("{0}: ‭{1:t}", CommonResources.KnissatShabbat, knissatShabbat.Value)));
            }

            int omer = jc.DayOfOmer;

            if (omer == 1)
            {
                _zmanim.Add(new TimeOfDay(CommonResources.Omer1));
            }
            else if (omer > 1)
            {
                _zmanim.Add(new TimeOfDay(string.Format(CommonResources.OmerShort, omer)));
            }

            Title = jc.Time.ToString("dddd", ci) + ", " + formatter.format(jc);//now.ToString("D", ci.DateTimeFormat);
        }

        private void AddPrayerInfo(PrayerInfo prayer)
        {
            _zmanim.Add(new TimeOfDay(string.Format("{0}: ‭{1:t}-{2:t}", prayer.PrayerName, prayer.Start, prayer.End)));

            if (!string.IsNullOrEmpty(prayer.ExtraInfo))
            {
                _zmanim.Add(new TimeOfDay(prayer.ExtraInfo));
            }
        }

        private string GetDafYomiText(JewishCalendar jc, JewishCalendar dafYomiDate, HebrewDateFormatter formatter)
        {
            if (jc.Time == dafYomiDate.Time)
            {
                return HebDateHelper.GetDafYomi(dafYomiDate);
            }

            return HebDateHelper.GetDafYomi(dafYomiDate) + " (" + formatter.format(dafYomiDate) + ")";
        }
    }
}
