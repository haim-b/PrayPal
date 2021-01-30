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
    public class ShmoneEsreBaladi : ShmoneEsreBase
    {
        public ShmoneEsreBaladi(Prayer prayer)
            : base(prayer)
        { }

        protected override void AddPart3()
        {
            if (Prayer != Prayer.Arvit)
            {
                Add(CommonPrayerTextProvider.Current.Kdusha);
            }

            AddStringFormat(Prayer == Prayer.Arvit ? "{0}" : AppResources.SingleSaysInstruction_F0, CommonPrayerTextProvider.Current.SE03);

            if (DayInfo.AseretYameyTshuva)
            {
                Add(BaladiPrayerTextProvider.Instance.KdushaAYT);
            }
        }

        protected override void AddBirkatHaShanim()
        {
            if (DayInfo.IsVetenBracha())
            {
                Add(CommonPrayerTextProvider.Current.SE09Summer);
                Settings.ShowVeanenu = false;
            }
            else
            {
                Add(CommonPrayerTextProvider.Current.SE09Winter);
            }
        }

        protected override void AddPart16()
        {
            if (IsMinchaInTeanit)
            {
                Add(CommonPrayerTextProvider.Current.SE16);

                Add(string.Format(CommonPrayerTextProvider.Current.Anenu, string.Empty), AppResources.InTfilatYachidTitle);

                Add(CommonPrayerTextProvider.Current.SE16B);
            }
            else
            {
                Add(string.Concat(CommonPrayerTextProvider.Current.SE16, " ", CommonPrayerTextProvider.Current.SE16B));
            }
        }

        public override bool IsPart9Bold
        {
            get
            {
                return false;
            }
        }

        protected override void AddEnding2()
        {
            base.AddEnding2();

            if (Prayer == Prayer.Arvit)
            {
                Add(EdotHaMizrachPrayerTextProvider.Instance.NoTachanunText);
            }
        }

        protected override void AddPart3Musaf()
        {
            //TODO: In Hoshaana Raba put Kdushat Keter of Shabbat.
            Add(CommonPrayerTextProvider.Current.KdushatKeter, AppResources.KdushaTitle, true);

            AddStringFormat(CommonPrayerTextProvider.Current.SE03, CommonPrayerTextProvider.Current.SE03Hael);
        }
    }
}
