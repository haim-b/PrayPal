using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content
{
    [Nusach(Nusach.Sfard, Nusach.Ashkenaz)]
    public class BirkatHamazonSfard : BirkatHamazonBase
    {
        protected override void AddOpening()
        {
            if (_dayInfo.IsTachanunDay(Settings.Nusach))
            {
                Add(Psalms.Psalm137);
            }
            else
            {
                Add(Psalms.Psalm126);
            }
        }

        protected override void AddPart3()
        {
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P9);
        }
    }
}
