using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;

namespace PrayPal.Content
{
    [Nusach(Nusach.Ashkenaz)]
    public class ShacharitAshkenaz : ShacharitSfard
    {
        protected override ShmoneEsreBase GetShmoneEsre(Prayer prayer)
        {
            return new ShmoneEsreAshkenaz(prayer);
        }

        protected override void AddAvinuMalkenu()
        {
            SpanModel avinuMalkenu = new SpanModel(AppResources.AvinuMalkenuTitle);

            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu1);

            if (_dayInfo.AseretYameyTshuva)
            {
                avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu2AYT);
            }
            else
            {
                avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu2Teanit);
            }

            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu3);
            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu4);

            _items.Add(avinuMalkenu);
        }

        protected override bool ShouldAddHallelBlessing(HallelMode hallelMode)
        {
            return true;
        }

        protected override bool ShouldAddHallelEnding(HallelMode hallelMode)
        {
            return true;
        }

        protected override void AddViduyAnd13Midot(SpanModel tachanun)
        {
            if (IsMondayOrThursday() && !ShouldShowSlichot())
            {
                // Show viduy and 13 Midot only on Monday and Thursday when there are no Slichot:
                base.AddViduyAnd13Midot(tachanun);
            }

            if (IsMondayOrThursday())
            {
                tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH1, CommonPrayerTextProvider.Current.TachanunBH2, CommonPrayerTextProvider.Current.TachanunBH3, CommonPrayerTextProvider.Current.TachanunBH4, CommonPrayerTextProvider.Current.TachanunBH5, CommonPrayerTextProvider.Current.TachanunBH6);
            }
        }

        protected override void AddMondayThursdayText(SpanModel tachanun)
        {
            tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH7, CommonPrayerTextProvider.Current.TachanunBH8);
        }

        protected override void AddDayVerseEnding(SpanModel span)
        {

        }

        protected override void AddDayVerseExtras(SpanModel dayVerse)
        {
            dayVerse.AddRange(PrayersHelper.GetKadishYatom(_dayInfo, true));
        }
    }
}
