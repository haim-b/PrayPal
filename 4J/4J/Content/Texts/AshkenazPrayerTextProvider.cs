using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content
{
    public class AshkenazPrayerTextProvider : CommonPrayerTextProvider
    {
        private static AshkenazPrayerTextProvider _instance;

        private AshkenazPrayerTextProvider()
            : base(typeof(PrayersAshkenaz))
        { }

        public static AshkenazPrayerTextProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AshkenazPrayerTextProvider();
                }

                return _instance;
            }
        }

        public string SE19ShalomRav
        {
            get { return _texts["SE19_ShalomRav"]; }
        }
    }
}
