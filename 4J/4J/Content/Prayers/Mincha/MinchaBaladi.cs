using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    [Nusach(Nusach.Baladi)]
    public class MinchaBaladi : MinchaBase
    {
        protected override void AddOpening()
        {
            // Parashat HaAkedah:
            Add(AppResources.VersesBeforeMinchaTitle,
                BaladiPrayerTextProvider.Instance.PreMincha1,
                EdotHaMizrachPrayerTextProvider.Instance.PreShacharit3,
                EdotHaMizrachPrayerTextProvider.Instance.ParashatHaakedah,
                string.Format(BaladiPrayerTextProvider.Instance.PreMincha2, DayInfo.IsCholHamoed ? "" : BaladiPrayerTextProvider.Instance.PreMincha2Reg));


            _items.Add(PrayersHelper.GetPsalm(84));

            Add(AppResources.KtoretHasamimTitle,
                EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim1,
                BaladiPrayerTextProvider.Instance.PreMincha3,
                EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim2,
                CommonPrayerTextProvider.Current.PitumHaktoret1,
                BaladiPrayerTextProvider.Instance.PreMincha4);
        }

        protected override void AddTorahBookHotzaa(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.Gadlu);
            span.Add(CommonPrayerTextProvider.Current.TorahBookWalking);
            span.Add(new ParagraphModel(AppResources.HagbahaTitle, CommonPrayerTextProvider.Current.VezotHatorah));
        }

        protected override void AddTorahBookReplacing(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.TorahBookReplacing1);
        }

        protected override ShmoneEsreBase GetShmoneEsre()
        {
            return new ShmoneEsreBaladi(Prayer.Mincha);
        }

        protected override void AddAvinuMalkenu()
        {
            Add(AppResources.AvinuMalkenuTitle,
                CommonPrayerTextProvider.Current.AvinuMalkenu1);
        }

        protected override void AddViduyAnd13Midot(SpanModel tachanun)
        {
            // Baladi doesn't say this
        }

        protected override void AddAleinuLeshabeach()
        {
            /////למנצח
            //SpanModel psalm67 = PrayersHelper.GetPsalm(67);
            //_items.Add(psalm67);

            //AddKadishYatom();

            if (DayInfo.Teanit)
            {
                SpanModel psalm102 = PrayersHelper.GetPsalm(102);
                _items.Add(psalm102);
            }

            //_items.Add(new ParagraphModel(AppResources.InMourningHouseTitle, Psalms.Psalm49) { IsCollapsible = true }));

            if (DayInfo.YomTov == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                SpanModel psalm107 = PrayersHelper.GetPsalm(107);
                _items.Add(psalm107);
            }
            else if (DayInfo.YomTov == JewishCalendar.CHOL_HAMOED_SUCCOS)
            {
                SpanModel psalm42 = PrayersHelper.GetPsalm(42);
                _items.Add(psalm42);
                SpanModel psalm43 = PrayersHelper.GetPsalm(43);
                _items.Add(psalm43);
            }
        }
    }
}
