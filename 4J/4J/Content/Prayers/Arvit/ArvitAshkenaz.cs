using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;

namespace PrayPal.Content.Prayers.Arvit
{
    [Nusach(Nusach.Ashkenaz)]
    public class ArvitAshkenaz : ArvitSfard
    {
        public ArvitAshkenaz(ILocationService locationService)
            : base(locationService)
        { }

        protected override string GetTitle()
        {
            return CommonResources.MaarivTitle;
        }

        protected override void AddVersesBeforeArvit()
        {
            // Nusach Ashkenaz has no verses before Arvit:
        }

        protected override bool IsAbroadForShmaBlessing()
        {
            // Nusach Ashkenaz has no blessing for abroad on Shma:
            return false;
        }

        protected override ShmoneEsreBase GetShmoneEsre()
        {
            return new ShmoneEsreAshkenaz(Prayer.Arvit);
        }

        protected override void AddPsalm121()
        {

        }

        protected override void AddEnding()
        {
            /*Nusach Ashkenaz ending has the following structure:
             * - Sfirat HaOmer (if relevant)
             * - Aleinu Leshabeach
             * - Kadish yatom
             * - Barchu
             * - LeDavid (= psalm 27) (if relevant)
             */

            if (_dayInfo.DayOfOmer != -1)
            {
                AddSfiratHaOmer();
            }

            AddAleinuLeshabeach();

            if (_dayInfo.ShouldSayLeDavid())
            {
                SpanModel ledavid = PrayersHelper.GetPsalm(27);

                ledavid.AddRange(PrayersHelper.GetKadishYatom(_dayInfo, true));

                _items.Add(ledavid);
            }
        }

        protected override void AddTextBeforeSforatHaOmer(SpanModel sfiratHaOmer)
        {
            sfiratHaOmer.Add(CommonPrayerTextProvider.Current.PreSfiratHaHomer);
        }

        protected override void AddAleinuLeshabeach()
        {
            SpanModel aleinu = new SpanModel(AppResources.AleinuLeshabeachTitle);

            aleinu.Add(CommonPrayerTextProvider.Current.AleinuLeshabeach);

            aleinu.AddRange(PrayersHelper.GetKadishYatom(_dayInfo, true));

            aleinu.Add(CommonPrayerTextProvider.Current.Barchu);

            _items.Add(aleinu);
        }
    }
}
