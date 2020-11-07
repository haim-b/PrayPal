using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zmanim;

namespace PrayPal.Common.Services
{
    public interface ITimeService
    {
        /// <summary>
        /// Returns the information about the current day, relative to Judaism.
        /// </summary>
        /// <param name="relativeToDate">The exact time to calculate relatively to.</param>
        /// <param name="dayCritical">Perform more sensitive calculation for prayers that are day sensitive like Arvit and Birkat HaMazon.</param>
        /// <returns></returns>
        Task<DayJewishInfo> GetDayInfoAsync(Geoposition location = null, DateTime? relativeToDate = null, bool dayCritical = false);

        Task<PrayerInfo> GetShacharitInfoAsync(Geoposition location = null, DateTime? relativeToDate = null);

        Task<PrayerInfo> GetMinchaInfoAsync(Geoposition location = null, DateTime? relativeToDate = null);

        Task<PrayerInfo> GetArvitInfoAsync(Geoposition location = null, DateTime? relativeToDate = null);

        Task<ComplexZmanimCalendar> GetCurrentZmanimCalendarAsync(DateTime? date = null, Geoposition position = null);

        Task<DateTime?> GetSunsetAsync(Geoposition position = null);

        Task<DateTime?> GetKnissatShabbatAsync(Geoposition position, DateTime? date);

        Task<DateTime?> GetNightChatzotAsync(bool fallBackToCivilMidnight, Geoposition position = null, DateTime? date = null);
    }
}
