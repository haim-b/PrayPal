using PrayPal.Common.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zmanim;
using Zmanim.HebrewCalendar;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace PrayPal.Common.Services
{
    public class TimeService : ITimeService
    {
        private readonly ILocationService _locationService;

        public TimeService(ILocationService locationService)
        {
            if (locationService == null)
            {
                throw new ArgumentNullException("locationService");
            }

            _locationService = locationService;
        }

        public async Task<DayJewishInfo> GetDayInfoAsync(Geoposition location = null, DateTime? relativeToDate = null, bool dayCritical = false)
        {
            return await Task.Run(async () =>
            {
                DateTime date = relativeToDate ?? DateTime.Now;

                Geoposition position = location ?? await _locationService.GetCurrentPositionAsync();

                if (position != null)
                {
                    ComplexZmanimCalendar zc = await GetCurrentZmanimCalendarAsync(date, position);

                    DateTime? sunset = zc.GetSunset();

                    if (sunset != null)
                    {
                        TimeSpan sunsetTimeOfDay = sunset.Value.TimeOfDay;

                        if (dayCritical)
                        {
                            sunsetTimeOfDay -= TimeSpan.FromMinutes(10);
                        }

                        if (sunsetTimeOfDay < date.TimeOfDay)
                        {
                            date += TimeSpan.FromDays(1);
                        }
                    }
                }

                return new DayJewishInfo(new JewishCalendar(date, Settings.IsInIsrael));
            });
        }

        public async Task<ComplexZmanimCalendar> GetCurrentZmanimCalendarAsync(DateTime? date = null, Geoposition position = null)
        {
            if (position == null)
            {
                position = await _locationService.GetCurrentPositionAsync();
            }

            GeoLocation location = ToGeoLocation(position);

            if (location == null)
            {
                location = new GeoLocation();
            }

            return new ComplexZmanimCalendar(date ?? DateTime.Now, location);
        }

        public async Task<PrayerInfo> GetShacharitInfoAsync(Geoposition location = null, DateTime? relativeToDate = null)
        {
            DateTime date = relativeToDate ?? DateTime.Now;

            ComplexZmanimCalendar zc = await GetCurrentZmanimCalendarAsync(date, location);

            if (zc == null)
            {
                return null;
            }

            DateTime? hanetz = zc.GetSunrise();
            DateTime? sofZmanTfila = null;

            TimeCalcMethod timeCalcMethod = Settings.TimeCalcMethod;

            if (timeCalcMethod == TimeCalcMethod.Gra)
            {
                sofZmanTfila = zc.GetSofZmanTfilaGRA();
            }
            else
            {
                sofZmanTfila = zc.GetSofZmanTfilaMGA();
            }

            if (hanetz != null && sofZmanTfila != null)
            {
                string extraInfo = null;
                DateTime? sofZmanShma = null;

                if (timeCalcMethod == TimeCalcMethod.Gra)
                {
                    sofZmanShma = zc.GetSofZmanShmaGRA();
                }
                else
                {
                    sofZmanShma = zc.GetSofZmanShmaMGA();
                }

                if (sofZmanShma != null)
                {
                    extraInfo = string.Format("{0}: ‭{1:t}", CommonResources.EndTimeOfShma, sofZmanShma.Value.ToString("t"));
                }

                return new PrayerInfo() { Prayer = Prayer.Shacharit, Start = hanetz.Value, End = sofZmanTfila.Value, PrayerName = CommonResources.Shacharit, ExtraInfo = extraInfo };
            }

            return null;
        }

        public async Task<PrayerInfo> GetMinchaInfoAsync(Geoposition location = null, DateTime? relativeToDate = null)
        {
            DateTime date = relativeToDate ?? DateTime.Now;

            ComplexZmanimCalendar zc = await GetCurrentZmanimCalendarAsync(date, location);

            if (zc == null)
            {
                return null;
            }

            DateTime? minchaGdola = zc.GetMinchaGedola();
            DateTime? sunset = zc.GetSunset();

            if (minchaGdola != null && sunset != null)
            {
                return new PrayerInfo() { Prayer = Prayer.Mincha, Start = minchaGdola.Value, End = sunset.Value, PrayerName = CommonResources.Mincha };
            }

            return null;
        }

        public async Task<PrayerInfo> GetArvitInfoAsync(Geoposition location = null, DateTime? relativeToDate = null)
        {
            DateTime now = relativeToDate ?? DateTime.Now;

            ComplexZmanimCalendar zc = await GetCurrentZmanimCalendarAsync(now, location);

            if (zc == null)
            {
                return null;
            }

            DateTime? tzaisHakokhavim = zc.GetTzais();

            zc = await GetCurrentZmanimCalendarAsync(now + TimeSpan.FromDays(1));

            if (zc == null)
            {
                return null;
            }

            DateTime? hanetz = zc.GetSunrise();

            if (tzaisHakokhavim != null && hanetz != null)
            {
                return new PrayerInfo() { Prayer = Prayer.Arvit, Start = tzaisHakokhavim.Value, End = hanetz.Value, PrayerName = CommonResources.Arvit };
            }

            return null;
        }

        public async Task<DateTime?> GetSunsetAsync(Geoposition position = null)
        {
            if (position == null)
            {
                position = await _locationService.GetCurrentPositionAsync();
            }

            if (position == null)
            {
                return null;
            }

            GeoLocation location = ToGeoLocation(position);

            return new ZmanimCalendar(location).GetSunset();
        }

        private static GeoLocation ToGeoLocation(Geoposition position)
        {
            if (position == null)
            {
                return null;
            }

            ITimeZone timeZone = new WindowsTimeZone(TimeZoneInfo.Local);
            return new GeoLocation(string.Empty, position.Latitude, position.Longitude, Math.Max(position.Altitude ?? 0, 0), timeZone);
        }

        public async Task<DateTime?> GetKnissatShabbatAsync(Geoposition location, DateTime? forDate = null)
        {
            if (location == null)
            {
                return null;
            }

            DateTime now = forDate ?? DateTime.Now;

            DayJewishInfo dayInfo = await GetDayInfoAsync(location, now);
            now = dayInfo.JewishCalendar.Time;

            while (now.DayOfWeek != DayOfWeek.Saturday)
            {
                now += TimeSpan.FromDays(1);
            }

            ComplexZmanimCalendar zc = await GetCurrentZmanimCalendarAsync(now, location);

            if (zc == null)
            {
                return null;
            }

            DateTime? knissatShabbat = zc.GetCandelLighting();

            if (knissatShabbat != null)
            {
                if (!TimeZoneInfo.Local.IsDaylightSavingTime(now) && TimeZoneInfo.Local.IsDaylightSavingTime(knissatShabbat.Value))
                {
                    knissatShabbat = knissatShabbat.Value + TimeSpan.FromHours(1);
                }
                else if (TimeZoneInfo.Local.IsDaylightSavingTime(now) && !TimeZoneInfo.Local.IsDaylightSavingTime(knissatShabbat.Value))
                {
                    knissatShabbat = knissatShabbat.Value - TimeSpan.FromHours(1);
                }
            }

            return knissatShabbat;
        }

        public async Task<DateTime?> GetNightChatzotAsync(bool fallBackToCivilMidnight, Geoposition position = null, DateTime? date = null)
        {
            ComplexZmanimCalendar zc = await GetCurrentZmanimCalendarAsync(date, position);

            DateTime? chatzot = zc?.GetChatzos();

            if (chatzot == null)
            {
                if (!fallBackToCivilMidnight)
                {
                    return null;
                }

                DateTime now = DateTime.Now;

                return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            }
            else
            {
                return (DateTime)chatzot - TimeSpan.FromHours(12);
            }
        }
    }
}
