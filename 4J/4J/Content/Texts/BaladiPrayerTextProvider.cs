using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content
{
    public class BaladiPrayerTextProvider : EdotHaMizrachPrayerTextProvider
    {
        public BaladiPrayerTextProvider()
            : base(typeof(PrayersBaladi))
        { }
    }
}
