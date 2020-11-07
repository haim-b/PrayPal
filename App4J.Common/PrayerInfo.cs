using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Common
{
    public class PrayerInfo
    {
        public Prayer Prayer { get; set; }

        public string PrayerName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string ExtraInfo { get; set; }
    }
}
