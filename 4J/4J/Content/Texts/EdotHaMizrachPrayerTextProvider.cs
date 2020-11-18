using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content
{
    public class EdotHaMizrachPrayerTextProvider : CommonPrayerTextProvider
    {
        private static EdotHaMizrachPrayerTextProvider _instance;

        private EdotHaMizrachPrayerTextProvider()
            : this(typeof(PrayersEdotMizrach))
        { }

        protected EdotHaMizrachPrayerTextProvider(Type prayerResourcesType)
            : base(prayerResourcesType)
        { }

        public static EdotHaMizrachPrayerTextProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EdotHaMizrachPrayerTextProvider();
                }

                return _instance;
            }
        }

        public string MeeinShaloshMichya3Israel
        {
            get { return _texts["MeeinShaloshMichya3Israel"]; }
        }

        public string MeeinShaloshMichya3
        {
            get { return _texts["MeeinShaloshMichya3"]; }
        }

        public string BeforeBirkatHamazon
        {
            get { return _texts["BeforeBirkatHamazon"]; }
        }

        public string LeshemYichudMincha
        {
            get { return _texts["LeshemYichudMincha"]; }
        }

        public string KtoretHasamim1
        {
            get { return _texts["KtoretHasamim1"]; }
        }

        public string KtoretHasamim2
        {
            get { return _texts["KtoretHasamim2"]; }
        }

        public string AfterAshreyOfMincha
        {
            get { return _texts["AfterAshreyOfMincha"]; }
        }

        public string NefilatApayimEnding
        {
            get { return _texts["NefilatApayimEnding"]; }
        }

        public string LeshemYichudArvit
        {
            get { return _texts["LeshemYichudArvit"]; }
        }

        public string BedtimeShma2AfterChatzot
        {
            get { return _texts["BedtimeShma2AfterChatzot"]; }
        }

        public string TachanunBHsYg
        {
            get { return _texts["TachanunBHsYg"]; }
        }

        public string TachanunBHS1
        {
            get { return _texts["TachanunBHS1"]; }
        }

        public string TachanunBHS2
        {
            get { return _texts["TachanunBHS2"]; }
        }

        public string TachanunBHS3
        {
            get { return _texts["TachanunBHS3"]; }
        }

        public string TachanunBHS4
        {
            get { return _texts["TachanunBHS4"]; }
        }

        public string PreSfiratHaOmer1
        {
            get { return _texts["PreSfiratHaOmer1"]; }
        }

        public string PreSTNoTachanun
        {
            get { return _texts["PreSTNoTachanun"]; }
        }

        public string GiluyDaat
        {
            get { return _texts["GiluyDaat"]; }
        }

        public string BirkotHashachar1Av9th
        {
            get { return _texts["BirkotHashachar1Av9th"]; }
        }

        public string PatachEliyahu1
        {
            get { return _texts["PatachEliyahu1"]; }
        }

        public string PatachEliyahu2
        {
            get { return _texts["PatachEliyahu2"]; }
        }

        public string PatachEliyahu3
        {
            get { return _texts["PatachEliyahu3"]; }
        }

        public string PatachEliyahu4
        {
            get { return _texts["PatachEliyahu4"]; }
        }

        public string PatachEliyahu5
        {
            get { return _texts["PatachEliyahu5"]; }
        }

        public string PatachEliyahu6
        {
            get { return _texts["PatachEliyahu6"]; }
        }

        public string HanachatTfillin2
        {
            get { return _texts["HanachatTfillin2"]; }
        }

        public string VatitpalelChannah
        {
            get { return _texts["VatitpalelChannah"]; }
        }

        public string PreShacharit1
        {
            get { return _texts["PreShacharit1"]; }
        }

        public string PreShacharit2
        {
            get { return _texts["PreShacharit2"]; }
        }

        public string PreShacharit3
        {
            get { return _texts["PreShacharit3"]; }
        }

        public string PreShacharit4
        {
            get { return _texts["PreShacharit4"]; }
        }

        public string PreShacharit5
        {
            get { return _texts["PreShacharit5"]; }
        }

        public string PreShacharit6
        {
            get { return _texts["PreShacharit6"]; }
        }

        public string PreShacharit7
        {
            get { return _texts["PreShacharit7"]; }
        }

        public string PreShacharit8
        {
            get { return _texts["PreShacharit8"]; }
        }

        public string PreShacharit9
        {
            get { return _texts["PreShacharit9"]; }
        }

        public string PreShacharit10
        {
            get { return _texts["PreShacharit10"]; }
        }

        public string PreShacharit11
        {
            get { return _texts["PreShacharit11"]; }
        }

        public string PreVerseOfDay1
        {
            get { return _texts["PreVerseOfDay1"]; }
        }

        public string PreVerseOfDay2
        {
            get { return _texts["PreVerseOfDay2"]; }
        }

        public string ParashatHaakedah
        {
            get { return _texts["ParashatHaakedah"]; }
        }

        public string Psalm30Shacharit
        {
            get { return _texts["Psalm30Shacharit"]; }
        }

        public string ShacharitVerse1AYT
        {
            get { return _texts["ShacharitVerse1AYT"]; }
        }

        public string YehiChasdecha
        {
            get { return _texts["YehiChasdecha"]; }
        }
    }
}
