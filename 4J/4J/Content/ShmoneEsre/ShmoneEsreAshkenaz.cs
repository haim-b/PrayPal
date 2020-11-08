using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content
{
    public class ShmoneEsreAshkenaz : ShmoneEsreSfard
    {
        public ShmoneEsreAshkenaz(Prayer prayer)
            : base(prayer)
        { }

        protected override void AddPart3()
        {
            bool aseretYameyTshuva = _dayInfo.AseretYameyTshuva;

            if (_prayer != Prayer.Arvit)
            {
                AddStringFormat(CommonPrayerTextProvider.Current.Kdusha, aseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, aseretYameyTshuva, false, AppResources.KdushaTitle, true);
                AddStringFormat(CommonPrayerTextProvider.Current.SE03, _dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, aseretYameyTshuva, false, AppResources.InTfilatYachidTitle);
            }
            else
            {
                AddStringFormat(CommonPrayerTextProvider.Current.SE03, aseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, aseretYameyTshuva);
            }
        }

        protected override void AddPart19()
        {
            if (!_dayInfo.Teanit && (_prayer == Prayer.Mincha || _prayer == Prayer.Arvit))
            {
                Add(AshkenazPrayerTextProvider.Instance.SE19ShalomRav, _dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.SE19AYT : string.Empty, _dayInfo.AseretYameyTshuva);
            }
            else
            {
                base.AddPart19();
            }
        }
    }
}
