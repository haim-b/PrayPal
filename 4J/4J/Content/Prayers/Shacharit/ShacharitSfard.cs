using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    [Nusach(Nusach.Sfard)]
    public class ShacharitSfard : ShacharitBase
    {
        public ShacharitSfard(IPermissionsService permissionsService)
            : base(permissionsService)
        { }

        protected override ShmoneEsreBase GetShmoneEsre(Prayer prayer)
        {
            return new ShmoneEsreSfard(prayer);
        }

        protected override bool ShouldAddAvinuMalkenu()
        {
            if (base.ShouldAddAvinuMalkenu())
            {
                return true;
            }
            else if (DayInfo.Teanit && DayInfo.YomTov != JewishCalendar.TISHA_BEAV)
            {
                return true;
            }

            return false;
        }

        protected override void AddAvinuMalkenu()
        {
            SpanModel avinuMalkenu = new SpanModel(AppResources.AvinuMalkenuTitle);

            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu1);

            if (DayInfo.AseretYameyTshuva)
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

        protected override string GetNefilatApayimTitle()
        {
            return AppResources.NefilatAppayimTitle;
        }

        protected override bool ShouldAddHallelBlessing(HallelMode hallelMode)
        {
            return true;
        }

        protected override bool ShouldAddHallelEnding(HallelMode hallelMode)
        {
            return true;
        }

        protected override void AddMondayThursdayText(SpanModel tachanun)
        {
            tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH1, CommonPrayerTextProvider.Current.TachanunBH2, CommonPrayerTextProvider.Current.TachanunBH3, CommonPrayerTextProvider.Current.TachanunBH4, CommonPrayerTextProvider.Current.TachanunBH5, CommonPrayerTextProvider.Current.TachanunBH6, CommonPrayerTextProvider.Current.TachanunBH7, CommonPrayerTextProvider.Current.TachanunBH8);
        }

        protected override void AddTachanunEnding(SpanModel tachanun)
        {
            tachanun.Add(CommonPrayerTextProvider.Current.TachanunEnding);
        }

        protected override void AddAronKodeshOpeningText(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.VeyehiBinsoa);
            span.Add(CommonPrayerTextProvider.Current.BrichShmeh);
        }

        protected override void AddTextBeforeTorahReading(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.Vatigaleh);
        }

        protected override void AddTextAfterTorahReading(SpanModel span)
        {
            span.Add(new ParagraphModel(AppResources.HagbahaVeglilahTitle, CommonPrayerTextProvider.Current.VezotHatorah));

            if (DayInfo.IsTachanunDay(GetNusach()))
            {
                span.Add(CommonPrayerTextProvider.Current.YehiRatzonAfterTorah);
            }
        }

        protected override SpanModel AddTorahReadingEnding()
        {
            SpanModel span = new SpanModel(AppResources.TorahBookReplacingTitle, CommonPrayerTextProvider.Current.TorahBookReplacing1, Psalms.Psalm24, CommonPrayerTextProvider.Current.TorahBookReplacing2);
            _items.Add(span);
            return span;
        }

        protected override void AddDayVerseExtras(SpanModel dayVerse)
        {
            if (DayInfo.JewishCalendar.RoshChodesh)
            {
                dayVerse.AddRange(PrayersHelper.GetKadishYatom(DayInfo, true));
                dayVerse.Add(new ParagraphModel(AppResources.BarchiNafshiTitle, Psalms.Psalm104));
            }

            dayVerse.AddRange(PrayersHelper.GetKadishYatom(DayInfo, true));

            int yomTov = DayInfo.YomTov;

            if (yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA)
            {
                dayVerse.Add(Psalms.Psalm27);
                dayVerse.AddRange(PrayersHelper.GetKadishYatom(DayInfo, true));
            }

            if (DayInfo.IsTachanunDay(Nusach.Sfard))
            {
                dayVerse.Add(new ParagraphModel(AppResources.InMourningHouseTitle, Psalms.Psalm49) { IsCollapsible = true });
            }
            else
            {
                dayVerse.Add(new ParagraphModel(AppResources.InMourningHouseTitle, Psalms.Psalm16) { IsCollapsible = true });
            }
        }
    }
}
