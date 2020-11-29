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
    [Nusach(Nusach.Sfard)]
    public class MinchaSfard : MinchaBase
    {
        protected override void AddTorahBookHotzaa(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.VeyehiBinsoa);
            span.Add(CommonPrayerTextProvider.Current.BrichShmeh);
            span.Add(CommonPrayerTextProvider.Current.Gadlu);
            span.Add(CommonPrayerTextProvider.Current.TorahBookWalking);
        }

        protected override void AddAfterTorahReading(SpanModel span)
        {
            base.AddAfterTorahReading(span);

            span.Add(new ParagraphModel(AppResources.HagbahaVeglilahTitle, CommonPrayerTextProvider.Current.VezotHatorah));
        }

        protected override void AddTorahBookReplacing(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.TorahBookReplacing1, Psalms.Psalm24, CommonPrayerTextProvider.Current.TorahBookReplacing2);
        }

        protected override ShmoneEsreBase GetShmoneEsre()
        {
            return new ShmoneEsreSfard(Prayer.Mincha);
        }

        protected override bool ShouldAddAvinuMalkenu()
        {
            return base.ShouldAddAvinuMalkenu() || DayInfo.Teanit && DayInfo.YomTov != JewishCalendar.TISHA_BEAV && !IsRealTeanitEsther();
        }

        private bool IsRealTeanitEsther()
        {
            return DayInfo.JewishCalendar.JewishDayOfMonth == 13 && DayInfo.JewishCalendar.JewishMonth == (DayInfo.JewishCalendar.JewishLeapYear ? JewishCalendar.ADAR_II : JewishCalendar.ADAR);
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

        protected override void AddAleinuLeshabeach()
        {
            SpanModel lastSpan = _items.Last();

            // למנצח collapsed
            SpanModel psalm67 = PrayersHelper.GetPsalm(67);
            lastSpan.Add(new ParagraphModel(AppResources.InPrayerWithEdotHaMizrach, psalm67.First()) { SubTitle = psalm67.Title, IsCollapsible = true });


            base.AddAleinuLeshabeach();

            AddKadishYatom();

            AddLeDavid();
        }

        protected virtual void AddLeDavid()
        {
            if (DayInfo.ShouldSayLeDavid())
            {
                SpanModel ledavid = PrayersHelper.GetPsalm(27);
                _items.Add(ledavid);

                AddKadishYatom();
            }
        }
    }
}
