using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Resources;
using Zmanim;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content.Prayers.BedtimeShma
{
    [Nusach(Nusach.EdotMizrach)]
    public class BedtimeShmaEdotHaMizrach : BedtimeShmaBase
    {
        private readonly ITimeService _timeService;

        public BedtimeShmaEdotHaMizrach(ITimeService timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        protected async override Task CreateOverride()
        {
            ComplexZmanimCalendar zc = await _timeService.GetCurrentZmanimCalendarAsync();

            DateTime chatzot = (DateTime)await _timeService.GetNightChatzotAsync(true);

            DateTime now = DateTime.Now;

            bool afterChazot = now >= chatzot;

            string bedtimeShma2 = afterChazot ? EdotHaMizrachPrayerTextProvider.Instance.BedtimeShma2AfterChatzot : EdotHaMizrachPrayerTextProvider.Instance.BedtimeShma2;

            string bedtimeShma6 = null;

            try
            {
                bedtimeShma6 = string.Format(AppResources.ThreeTimesInst_F0, EdotHaMizrachPrayerTextProvider.Instance.BedtimeShma6.Split('|')[_dayInfo.JewishCalendar.DayOfWeek - 1]);
            }
            catch (Exception) { }

            Add(CommonPrayerTextProvider.Current.BedtimeShma1);
            Add(bedtimeShma2);
            Add(CommonPrayerTextProvider.Current.KriatShma1);
            Add(CommonPrayerTextProvider.Current.KriatShma2);
            Add(CommonPrayerTextProvider.Current.KriatShma3);
            Add(CommonPrayerTextProvider.Current.BedtimeShma3);
            Add(CommonPrayerTextProvider.Current.BedtimeShma4);
            Add(CommonPrayerTextProvider.Current.BedtimeShma5);

            if (HebDateHelper.IsTachanunDay(Prayer.Shacharit, Nusach.EdotMizrach, _dayInfo.JewishCalendar) || afterChazot)
            {
                if (!afterChazot)
                {
                    Add(CommonPrayerTextProvider.Current.Viduy1, CommonPrayerTextProvider.Current.Viduy2);
                }
                else
                {
                    ///Since usually bedtime shma is after sunset, we need to go to the previous day to see if it was a tachanun day.;
                    JewishCalendar jc = HebDateHelper.Clone(_dayInfo.JewishCalendar);
                    jc.back();

                    if (HebDateHelper.IsTachanunDay(Prayer.Shacharit, Nusach.EdotMizrach, jc))
                    {
                        Add(CommonPrayerTextProvider.Current.Viduy1);
                        Add(CommonPrayerTextProvider.Current.Viduy2);
                    }
                }
            }

            Add(CommonPrayerTextProvider.Current.AnnaBechoach);
            Add(bedtimeShma6);
            Add(CommonPrayerTextProvider.Current.BedtimeShma7);
        }
    }
}
