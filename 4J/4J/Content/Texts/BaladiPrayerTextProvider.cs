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
        private static BaladiPrayerTextProvider _instance;

        public BaladiPrayerTextProvider()
            : base(typeof(PrayersBaladi))
        { }

        public static new BaladiPrayerTextProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BaladiPrayerTextProvider();
                }

                return _instance;
            }
        }

        public string AdonHaOlaminMorning
        {
            get { return _texts["AdonHaOlaminMorning"]; }
        }

        public string BirkotHashachar2NotAv9th
        {
            get { return _texts["BirkotHashachar2NotAv9th"]; }
        }
    }
}
