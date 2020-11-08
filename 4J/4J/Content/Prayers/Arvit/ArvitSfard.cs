using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;

namespace PrayPal.Content.Prayers.Arvit
{
    [Nusach(Nusach.Sfard)]
    public class ArvitSfard : ArvitBase
    {
        public ArvitSfard(ILocationService locationService)
            : base(locationService)
        { }

        protected override void AddVersesBeforeArvit()
        {
            // Nusach sfard don't say Shir Hamaalot on Motzaey Shabbat:
            if (_dayInfo.JewishCalendar.DayOfWeek != 1)
            {
                Add(AppResources.VersesBeforeArvitTitle,
                    new ParagraphModel(CommonPrayerTextProvider.Current.VersesBeforeArvit),
                    PrayersHelper.GetHalfKadish(_dayInfo));
            }
        }

        protected override IEnumerable<string> GetShmaAndBlessings()
        {
            IEnumerable<string> texts = base.GetShmaAndBlessings();

            if (IsAbroadForShmaBlessing())
            {
                texts.Concat(new string[] { CommonPrayerTextProvider.Current.InArvitInChul });
            }

            return texts;
        }

        protected virtual bool IsAbroadForShmaBlessing()
        {
            return !HebDateHelper.IsInIsrael();
        }

        protected override ShmoneEsreBase GetShmoneEsre()
        {
            return new ShmoneEsreSfard(Prayer.Arvit);
        }

        protected override void AddEnding()
        {
            /*Nusach Sfard ending has the following structure:
             * - Barchu
             * - Sfirat HaOmer (if relevant)
             * - Aleinu Leshabeach
             */

            if (_dayInfo.DayOfOmer != -1)
            {
                AddSfiratHaOmer();
            }

            AddAleinuLeshabeach();
        }

        protected override void AddAleinuLeshabeach()
        {
            SpanModel aleinu = new SpanModel(AppResources.AleinuLeshabeachTitle);

            if (_dayInfo.DayOfOmer == -1)
            {
                aleinu.Add(CommonPrayerTextProvider.Current.Barchu);
            }

            aleinu.Add(CommonPrayerTextProvider.Current.AleinuLeshabeach);

            _items.Add(aleinu);
        }

        protected override void AddTextBeforeSforatHaOmer(SpanModel sfiratHaOmer)
        {
            sfiratHaOmer.Add(CommonPrayerTextProvider.Current.Barchu);
            sfiratHaOmer.Add(CommonPrayerTextProvider.Current.PreSfiratHaHomer);
        }

        protected override void AddTextAfterSforatHaOmer(SpanModel sfiratHaOmer)
        {
            int omer = _dayInfo.DayOfOmer;
            string dayKabbalahDetail1 = CommonPrayerTextProvider.Current.SfiratHaOmerDaysKabbalah1.Split('|')[(omer - 1) % 7];
            string dayKabbalahDetail2 = CommonPrayerTextProvider.Current.SfiratHaOmerDaysKabbalah2.Split('|')[(omer - 1) / 7];
            sfiratHaOmer.Add(string.Format(CommonPrayerTextProvider.Current.AfterSfiratHaOmer2_F0, dayKabbalahDetail1, dayKabbalahDetail2));
        }
    }
}
