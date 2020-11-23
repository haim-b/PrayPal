using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content
{
    [Nusach(Nusach.EdotMizrach)]
    public class BirkatHamazonEdotHaMizrach : BirkatHamazonBase
    {
        protected override void AddOpening()
        {
            Add(EdotHaMizrachPrayerTextProvider.Instance.BeforeBirkatHamazon, AppResources.BlessingPreparation, true);
        }

        protected override void AddPart2()
        {
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P8, AppResources.BirkatHaoreach);
        }

        protected override void AddEnding()
        {
            _items.Add(PrayersHelper.GetOseShalom(DayInfo));
        }
    }
}
