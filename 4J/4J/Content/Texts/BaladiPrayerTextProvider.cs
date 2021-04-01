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

        public string ShofetKolHaAretz
        {
            get { return _texts["ShofetKolHaAretz"]; }
        }

        public string PsalmsEnding
        {
            get { return _texts["PsalmsEnding"]; }
        }

        public string ShiratHayamHtml
        {
            get { return _texts["ShiratHayamHtml"]; }
        }

        public string VatikachMiryam
        {
            get { return _texts["VatikachMiryam"]; }
        }

        public string Refaeni
        {
            get { return _texts["Refaeni"]; }
        }

        public string BarchuShacharit
        {
            get { return _texts["BarchuShacharit"]; }
        }

        public string KdushaAYT
        {
            get { return _texts["KdushaAYT"]; }
        }

        public string TachanunBHMon
        {
            get { return _texts["TachanunBHMon"]; }
        }

        public string TachanunBHThu
        {
            get { return _texts["TachanunBHThu"]; }
        }

        public string PreSTNoTachanunWithMussaf
        {
            get { return _texts["PreSTNoTachanunWithMussaf"]; }
        }

        public string BeforeTorahReading
        {
            get { return _texts["BeforeTorahReading"]; }
        }
    }
}
