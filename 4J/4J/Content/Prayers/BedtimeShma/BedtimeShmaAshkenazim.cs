using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content.Prayers.BedtimeShma
{
    [Nusach(Nusach.Sfard)]
    [Nusach(Nusach.Ashkenaz)]
    public class BedtimeShmaAshkenazim : BedtimeShmaBase
    {
        protected override Task CreateOverrideAsync()
        {
            Add(CommonPrayerTextProvider.Current.BedtimeShma1);
            Add(CommonPrayerTextProvider.Current.KriatShma1);
            Add(CommonPrayerTextProvider.Current.VeyehiNoam);
            Add(CommonPrayerTextProvider.Current.BedtimeShma2);
            Add(CommonPrayerTextProvider.Current.BedtimeShma3);
            Add(CommonPrayerTextProvider.Current.BedtimeShma4);
            Add(CommonPrayerTextProvider.Current.BedtimeShma5);
            Add(CommonPrayerTextProvider.Current.BedtimeShma6);
            Add(CommonPrayerTextProvider.Current.BedtimeShma7);
            Add(CommonPrayerTextProvider.Current.BedtimeShma8);
            Add(Psalms.Psalm128);
            Add(CommonPrayerTextProvider.Current.BedtimeShma9);
            Add(CommonPrayerTextProvider.Current.BedtimeShma10);

            return Task.FromResult(0);
        }
    }
}
