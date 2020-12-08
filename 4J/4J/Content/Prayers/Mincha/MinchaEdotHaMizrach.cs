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
    [Nusach(Nusach.EdotMizrach)]
    public class MinchaEdotHaMizrach : MinchaBase
    {
        protected override void AddOpening()
        {
            Add(AppResources.VersesBeforeMinchaTitle, EdotHaMizrachPrayerTextProvider.Instance.LeshemYichudMincha);

            _items.Add(PrayersHelper.GetPsalm(84));

            Add(AppResources.KtoretHasamimTitle,
                EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim1,
                EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim2,
                CommonPrayerTextProvider.Current.PitumHaktoret1,
                CommonPrayerTextProvider.Current.PitumHaktoret2);
        }

        protected override IEnumerable<ParagraphModel> GetAshrey()
        {
            yield return new ParagraphModel(CommonPrayerTextProvider.Current.Ashrey);

            yield return new ParagraphModel(EdotHaMizrachPrayerTextProvider.Instance.AfterAshreyOfMincha);
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
            return new ShmoneEsreEdotHamizrach(Prayer.Mincha);
        }

        protected override void AddAvinuMalkenu()
        {
            Add(AppResources.AvinuMalkenuTitle,
                CommonPrayerTextProvider.Current.AvinuMalkenu1,
                CommonPrayerTextProvider.Current.AvinuMalkenu3,
                CommonPrayerTextProvider.Current.AvinuMalkenu4);
        }

        protected override bool AddTachanun()
        {
            bool tachanunAdded = base.AddTachanun();

            if (!tachanunAdded)
            {
                Add(AppResources.AfterHazarahTitle, EdotHaMizrachPrayerTextProvider.Instance.NoTachanunText);
            }

            return tachanunAdded;
        }

        protected override void AddAleinuLeshabeach()
        {
            ///למנצח
            SpanModel psalm67 = PrayersHelper.GetPsalm(67);
            _items.Add(psalm67);

            AddKadishYatom();

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                SpanModel psalm93 = PrayersHelper.GetPsalm(93);
                _items.Add(psalm93);
            }

            if (DayInfo.Teanit)
            {
                SpanModel psalm102 = PrayersHelper.GetPsalm(102);
                _items.Add(psalm102);
            }

            base.AddAleinuLeshabeach();
        }
    }
}
