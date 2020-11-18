using PrayPal.Common.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Zmanim.HebrewCalendar;

namespace PrayPal.Common
{
    public static class HebDateHelper
    {
        internal static readonly HebrewDateFormatter _dateFormatter = new HebrewDateFormatter() { HebrewFormat = true };

        //public static PrayerInfo GetNextPrayer()
        //{
        //    Geoposition position = LocationManager.GetPosition();
        //    return GetNextPrayer(position);
        //}

        public static PrayerInfo GetNextPrayer(Geoposition position)
        {
            DateTime now = DateTime.Now;

            PrayerInfo[] prayers = GetPrayersInfo(position);

            foreach (PrayerInfo prayer in prayers)
            {
                if (prayer.Start <= now && now <= prayer.End)
                {
                    return prayer;
                }
            }

            return new PrayerInfo() { Prayer = Prayer.Unknown };
        }

        public static PrayerInfo[] GetPrayersInfo(Geoposition position, DateTime? date = null, bool relativePrayers = true)
        {
            DateTime now = date ?? DateTime.Now;

            List<PrayerInfo> prayers = new List<PrayerInfo>();

            if (position != null)
            {
                Zmanim.TimeZone.ITimeZone timeZone = new Zmanim.TimeZone.WindowsTimeZone(System.TimeZoneInfo.Local);
                Zmanim.Utilities.GeoLocation location = new Zmanim.Utilities.GeoLocation(string.Empty, position.Latitude, position.Longitude, position.Altitude ?? 0, timeZone);

                if (relativePrayers)
                {
                    AddRelativePrayersInfo(now, location, prayers);

                    if (prayers.Count < 3)
                    {
                        AddRelativePrayersInfo(now + TimeSpan.FromDays(1), location, prayers);
                    }
                }
                else
                {
                    AddPrayersInfo(now, location, prayers);
                }
            }

            return prayers.ToArray();
        }

        private static void AddRelativePrayersInfo(DateTime date, Zmanim.Utilities.GeoLocation location, List<PrayerInfo> prayers)
        {
            DateTime now = DateTime.Now;

            Zmanim.ComplexZmanimCalendar zc = new Zmanim.ComplexZmanimCalendar(date, location);

            DateTime? hanetz = zc.GetSunrise();

            TimeCalcMethod timeCalcMethod = Settings.TimeCalcMethod;

            DateTime? sofZmanTfila;

            if (timeCalcMethod == TimeCalcMethod.Gra)
            {
                sofZmanTfila = zc.GetSofZmanTfilaGRA();
            }
            else
            {
                sofZmanTfila = zc.GetSofZmanTfilaMGA();
            }

            if (hanetz != null && sofZmanTfila != null && sofZmanTfila > now)
            {
                string extraInfo = null;
                DateTime? sofZmanShma = null;

                if (timeCalcMethod == TimeCalcMethod.Gra)
                {
                    sofZmanShma = zc.GetSofZmanShmaGRA();
                }
                else
                {
                    sofZmanShma = zc.GetSofZmanShmaMGA();
                }

                if (sofZmanShma != null)
                {
                    extraInfo = string.Format("{0}: ‭{1:t}", CommonResources.EndTimeOfShma, sofZmanShma.Value.ToString("t"));
                }

                prayers.Add(new PrayerInfo() { Prayer = Prayer.Shacharit, Start = hanetz.Value, End = sofZmanTfila.Value, PrayerName = CommonResources.Shacharit, ExtraInfo = extraInfo });
            }

            if (prayers.Count == 3)
            {
                return;
            }

            DateTime? minchaGdola = zc.GetMinchaGedola();
            DateTime? sunset = zc.GetSunset();

            if (minchaGdola != null && sunset != null && sunset > now)
            {
                prayers.Add(new PrayerInfo() { Prayer = Prayer.Mincha, Start = minchaGdola.Value, End = sunset.Value, PrayerName = CommonResources.Mincha });
            }

            if (prayers.Count == 3)
            {
                return;
            }

            DateTime? tzaisHakokhavim = zc.GetTzais();
            //DateTime? hazot=zc

            zc = new Zmanim.ComplexZmanimCalendar(date + TimeSpan.FromDays(1), location);

            hanetz = zc.GetSunrise();

            if (tzaisHakokhavim != null && hanetz != null)
            {
                prayers.Add(new PrayerInfo() { Prayer = Prayer.Arvit, Start = tzaisHakokhavim.Value, End = hanetz.Value, PrayerName = CommonResources.Arvit });
            }
        }

        private static void AddPrayersInfo(DateTime date, Zmanim.Utilities.GeoLocation location, List<PrayerInfo> prayers)
        {
            Zmanim.ComplexZmanimCalendar zc = new Zmanim.ComplexZmanimCalendar(date, location);

            DateTime? hanetz = zc.GetSunrise();

            TimeCalcMethod timeCalcMethod = Settings.TimeCalcMethod;

            DateTime? sofZmanTfila;

            if (timeCalcMethod == TimeCalcMethod.Gra)
            {
                sofZmanTfila = zc.GetSofZmanTfilaGRA();
            }
            else
            {
                sofZmanTfila = zc.GetSofZmanTfilaMGA();
            }

            if (hanetz != null && sofZmanTfila != null)
            {
                string extraInfo = null;
                DateTime? sofZmanShma;

                if (timeCalcMethod == TimeCalcMethod.Gra)
                {
                    sofZmanShma = zc.GetSofZmanShmaGRA();
                }
                else
                {
                    sofZmanShma = zc.GetSofZmanShmaMGA();
                }

                if (sofZmanShma != null)
                {
                    extraInfo = string.Format("{0}: ‭{1:t}", CommonResources.EndTimeOfShma, sofZmanShma.Value.ToString("t"));
                }

                prayers.Add(new PrayerInfo() { Prayer = Prayer.Shacharit, Start = hanetz.Value, End = sofZmanTfila.Value, PrayerName = CommonResources.Shacharit, ExtraInfo = extraInfo });
            }

            if (prayers.Count == 3)
            {
                return;
            }

            DateTime? minchaGdola = zc.GetMinchaGedola();
            DateTime? sunset = zc.GetSunset();

            if (minchaGdola != null && sunset != null)
            {
                prayers.Add(new PrayerInfo() { Prayer = Prayer.Mincha, Start = minchaGdola.Value, End = sunset.Value, PrayerName = CommonResources.Mincha });
            }

            if (prayers.Count == 3)
            {
                return;
            }

            DateTime? tzaisHakokhavim = zc.GetTzais();
            //DateTime? hazot=zc

            zc = new Zmanim.ComplexZmanimCalendar(date + TimeSpan.FromDays(1), location);

            DateTime? alotHashachar = zc.GetAlos90();

            if (tzaisHakokhavim != null && alotHashachar != null)
            {
                prayers.Add(new PrayerInfo() { Prayer = Prayer.Arvit, Start = tzaisHakokhavim.Value, End = alotHashachar.Value, PrayerName = CommonResources.Arvit });
            }
        }

        public static bool IsMoridHatal(JewishCalendar jc)
        {
            Zmanim.HebrewCalendar.JewishDate cal = jc;

            if (cal.JewishMonth == 1) ///Nissan
            {
                if (cal.JewishDayOfMonth < 15) ///Pesach 1st
                {
                    return false;
                }

                return true;
            }
            else if (cal.JewishMonth == 7) ///Tishrey
            {
                if (cal.JewishDayOfMonth < 22) ///Shmini Atzeret
                {
                    return true;
                }

                return false;
            }
            else if (cal.JewishMonth < 7)
            {
                return true;
            }

            return false;
        }

        public static bool IsVetenBracha(JewishCalendar jc)
        {
            if (jc.JewishMonth == 1) ///Nissan
            {
                if (jc.JewishDayOfMonth < 15) ///Pesach 1st
                {
                    return false;
                }

                return true;
            }
            else if (jc.JewishMonth == 8) ///Heshvan
            {
                if (IsInIsrael())
                {
                    return jc.JewishDayOfMonth < 7; ///ז' חשוון
                }
                else
                {
                    return true;
                }
            }
            else if (jc.JewishMonth < 8)
            {
                return true;
            }
            else if (!IsInIsrael())
            {
                DateTime now = DateTime.Now;

                if (now.Month >= 4 && now.Month < 12)
                {
                    return true;
                }

                // Abroad, they start to ask for rains since December 5th on a regular year, and December 6th on a leap year:
                return now.Day < 5 + DateTime.DaysInMonth(now.Year, 2) - 28;
            }

            return false;
        }

        //public static JewishCalendar GetCalendarToday(Geoposition position = null, DateTime? now = null, bool forArvit = false)
        //{
        //    //****
        //    if (now == null)
        //    {
        //        now = GetCurrentDateTime(position, null, forArvit);
        //    }
        //    //****

        //    //LocationManager.ShowMessage("Final now is " + now);

        //    //LocationManager.ShowMessage("Getting moed for: " + now);
        //    JewishCalendar c = new JewishCalendar((DateTime)now) { InIsrael = IsInIsrael() };
        //    c.UseModernHolidays = true;

        //    return c;
        //}

        //public async static Task<DayJewishInfo> GetTodayInfoAsync(Geoposition position = null, DateTime? now = null, bool forArvit = false)
        //{
        //    if (now == null)
        //    {
        //        now = await GetCurrentDateTimeAsync(position, null, forArvit);
        //    }

        //    JewishCalendar c = new JewishCalendar((DateTime)now) { InIsrael = IsInIsrael() };
        //    c.UseModernHolidays = true;

        //    return new DayJewishInfo(c);
        //}

        //private static Moadim GetMoed(DateTime date)
        //{
        //    LocationManager.ShowMessage("Getting moed for: " + date);
        //    Zmanim.HebrewCalendar.JewishCalendar c = new Zmanim.HebrewCalendar.JewishCalendar(new DateTime(date.Year, date.Month, date.Day));
        //    //var hebDate = civ2heb(date.Day, date.Month, date.Year);
        //    int hDay = c.JewishDayOfMonth;//int.Parse(hebDate.Substring(0, hebDate.IndexOf(' ')));
        //    //string hmE = hebDate.Substring(hebDate.IndexOf(' ') + 1, hebDate.Length - 1 - hebDate.IndexOf(' '));
        //    int hMonth = c.JewishMonth - 1;//int.Parse(hmE.Substring(0, hmE.IndexOf(' ')));
        //    //int hYear = hmE.Substring(hmE.IndexOf(' ') + 1, hmE.Length);

        //    Moadim result = GetMoed(date.Day, date.Month, date.Year, hDay, hMonth, (int)date.DayOfWeek + 1);// DOW(date.Day, date.Month, date.Year));

        //    LocationManager.ShowMessage("Moed is: " + result);
        //    return result;
        //}

        //public static DateTime? GetKnissatShabbat(Geoposition position, DateTime? forDate = null)
        //{
        //    if (position == null)
        //    {
        //        return null;
        //    }

        //    DateTime now = forDate ?? DateTime.Now;

        //    while (now.DayOfWeek != DayOfWeek.Saturday)
        //    {
        //        now += TimeSpan.FromDays(1);
        //    }

        //    now = GetCurrentDateTime(position, now);

        //    Zmanim.TimeZone.ITimeZone timeZone = new Zmanim.TimeZone.WindowsTimeZone(System.TimeZoneInfo.Local);
        //    Zmanim.Utilities.GeoLocation location = new Zmanim.Utilities.GeoLocation(string.Empty, position.Latitude, position.Longitude, position.Altitude, timeZone);
        //    Zmanim.ComplexZmanimCalendar zc = new Zmanim.ComplexZmanimCalendar(now, location);

        //    DateTime? knissatShabbat = zc.GetCandelLighting();

        //    if (knissatShabbat != null)
        //    {
        //        if (!TimeZoneInfo.Local.IsDaylightSavingTime(now) && TimeZoneInfo.Local.IsDaylightSavingTime(knissatShabbat.Value))
        //        {
        //            knissatShabbat = knissatShabbat.Value + TimeSpan.FromHours(1);
        //        }
        //        else if (TimeZoneInfo.Local.IsDaylightSavingTime(now) && !TimeZoneInfo.Local.IsDaylightSavingTime(knissatShabbat.Value))
        //        {
        //            knissatShabbat = knissatShabbat.Value - TimeSpan.FromHours(1);
        //        }
        //    }

        //    return knissatShabbat;
        //}

        //public static async Task<DateTime> GetCurrentDateTimeAsync()
        //{
        //    return await GetCurrentDateTimeAsync(null);
        //}

        //public static async Task<DateTime> GetCurrentDateTimeAsync(Geoposition position = null, DateTime? relativeToDate = null, bool forArvit = false)
        //{
        //    if (position == null)
        //    {
        //        position = LocationManager.GetPosition();
        //    }

        //    DateTime now = relativeToDate ?? DateTime.Now;

        //    if (position != null)
        //    {
        //        Zmanim.ComplexZmanimCalendar zc = GetCurrentZmanimCalendar(position);

        //        DateTime? sunset = zc.GetSunset();

        //        if (sunset != null)
        //        {
        //            TimeSpan sunsetTimeOfDay = sunset.Value.TimeOfDay;

        //            if (forArvit)
        //            {
        //                sunsetTimeOfDay = sunsetTimeOfDay - TimeSpan.FromMinutes(10);
        //            }

        //            if (sunsetTimeOfDay < now.TimeOfDay)
        //            {
        //                now += TimeSpan.FromDays(1);
        //            }
        //        }
        //    }

        //    return now;
        //}

        //public static Zmanim.ComplexZmanimCalendar GetCurrentZmanimCalendar(Geoposition position)
        //{
        //    if (position == null)
        //    {
        //        position = LocationManager.GetPosition();
        //    }

        //    if (position == null)
        //    {
        //        return null;
        //    }

        //    Zmanim.TimeZone.ITimeZone timeZone = new Zmanim.TimeZone.WindowsTimeZone(System.TimeZoneInfo.Local);
        //    Zmanim.Utilities.GeoLocation location = new Zmanim.Utilities.GeoLocation(string.Empty, position.Latitude, position.Longitude, Math.Max(position.Altitude ?? 0, 0), timeZone);
        //    return new Zmanim.ComplexZmanimCalendar(location);
        //}

        public static bool ShouldNotifyVetenTalUmatar(JewishCalendar jc)
        {
            if (jc.JewishMonth == 8) ///Heshvan
            {
                if (jc.JewishDayOfMonth == 7 || (jc.JewishDayOfMonth == 8 && jc.DayOfWeek == 1)) ///ז' חשוון
                {
                    return true;
                }
            }

            return false;
        }

        //public static int GetSfiratHaomer(Geoposition position)
        //{
        //    if (position == null)
        //    {
        //        return 0;
        //    }

        //    //Zmanim.TimeZone.ITimeZone timeZone = new Zmanim.TimeZone.WindowsTimeZone(System.TimeZoneInfo.Local);
        //    //Zmanim.Utilities.GeoLocation location = new Zmanim.Utilities.GeoLocation(string.Empty, position.Latitude, position.Longitude, position.Altitude, timeZone);
        //    Zmanim.HebrewCalendar.JewishCalendar zc = new Zmanim.HebrewCalendar.JewishCalendar(GetCurrentDateTime(position)) { InIsrael = IsInIsrael() };

        //    return zc.DayOfOmer;
        //}

        public static bool IsTachanunDay(Prayer prayer, Nusach nusach, JewishCalendar jc)
        {
            if (!IsTachanunDay(jc, nusach, false))
            {
                return false;
            }

            if (prayer == Prayer.Mincha)
            {
                if (jc.DayOfWeek == 6) ///Friday has no tachanun in Mincha
                {
                    return false;
                }
                jc = Clone(jc);
                jc.forward();

                if (jc.YomTovIndex != JewishCalendar.PESACH_SHENI && !IsTachanunDay(jc, nusach, true))
                {
                    return false;
                }
            }

            return true;
        }

        private static JewishCalendar Clone(JewishCalendar jc)
        {
            return jc.CloneEx();
        }

        private static bool IsTachanunDay(JewishCalendar jc, Nusach nusach, bool isCheckingDayBefore)
        {
            if (jc.RoshChodesh)
            {
                return false;
            }

            bool isInIsrael = jc.InIsrael;

            ///Mark not in israel to include issru chag:
            try
            {
                jc.InIsrael = false;

                switch (jc.YomTovIndex)
                {
                    case JewishCalendar.SUCCOS:
                    case JewishCalendar.CHOL_HAMOED_SUCCOS:
                    case JewishCalendar.HOSHANA_RABBA:
                    case JewishCalendar.SIMCHAS_TORAH: ///Equals issru chag
                    case JewishCalendar.CHANUKAH:
                    case JewishCalendar.TU_BESHVAT:
                    case JewishCalendar.PURIM:
                    case JewishCalendar.SHUSHAN_PURIM:
                    case JewishCalendar.PURIM_KATAN:
                    case JewishCalendar.PESACH: ///Includes issru chag
                    case JewishCalendar.CHOL_HAMOED_PESACH:
                    case JewishCalendar.PESACH_SHENI:
                    case JewishCalendar.YOM_HAATZMAUT:
                    case JewishCalendar.YOM_YERUSHALAYIM:
                    case JewishCalendar.TISHA_BEAV:
                    case JewishCalendar.TU_BEAV:
                        return false;
                }
            }
            finally
            {
                jc.InIsrael = isInIsrael;
            }

            if (jc.DayOfOmer == 33)
            {
                return false;
            }

            if (jc.JewishMonth == 1) //Nissan
            {
                return false;
            }

            if (jc.JewishMonth == 3 && jc.JewishDayOfMonth >= 1 && jc.JewishDayOfMonth <= 12) //Sivan
            {
                return false;
            }

            if (jc.JewishMonth == 6 && jc.JewishDayOfMonth >= 29) //before Rosh Hashana
            {
                if (nusach == Nusach.EdotMizrach || nusach == Nusach.Baladi)
                {
                    return true;
                }

                if (!isCheckingDayBefore)
                {
                    return false;
                }
            }

            if (jc.JewishMonth == 7 && jc.JewishDayOfMonth >= 9) //Tishrey
            {
                if ((nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz) && isCheckingDayBefore && jc.JewishDayOfMonth == 9) //before Yom Kippur
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        //public static DateTime? GetSunset(Geoposition position = null)
        //{
        //    if (position == null)
        //    {
        //        position = LocationManager.GetPosition();
        //    }

        //    if (position == null)
        //    {
        //        return null;
        //    }

        //    Zmanim.ZmanimCalendar c = new Zmanim.ZmanimCalendar(new Zmanim.Utilities.GeoLocation(position.Latitude, position.Longitude, new Zmanim.TimeZone.WindowsTimeZone(TimeZoneInfo.Local)));
        //    return c.GetSunset();
        //}

        public static bool IsLamnatzeachDay(Nusach nusach, JewishCalendar jc)
        {
            if (jc.RoshChodesh)
            {
                return false;
            }

            bool isInIsrael = jc.InIsrael;

            ///Mark not in israel to include issru chag:
            try
            {
                jc.InIsrael = false;

                switch (jc.YomTovIndex)
                {
                    case JewishCalendar.SUCCOS:
                    case JewishCalendar.CHOL_HAMOED_SUCCOS:
                    case JewishCalendar.HOSHANA_RABBA:
                    case JewishCalendar.SIMCHAS_TORAH: ///Equals issru chag
                    case JewishCalendar.CHANUKAH:
                    case JewishCalendar.TU_BESHVAT:
                    case JewishCalendar.PURIM:
                    case JewishCalendar.SHUSHAN_PURIM:
                    case JewishCalendar.PURIM_KATAN:
                    case JewishCalendar.PESACH: ///Includes issru chag
                    case JewishCalendar.CHOL_HAMOED_PESACH:
                    case JewishCalendar.PESACH_SHENI:
                    case JewishCalendar.YOM_HAATZMAUT:
                    case JewishCalendar.YOM_YERUSHALAYIM:
                    case JewishCalendar.TISHA_BEAV:
                    case JewishCalendar.TU_BEAV:
                        return false;
                }
            }
            finally
            {
                jc.InIsrael = isInIsrael;
            }

            if (jc.ErevYomTov)
            {
                return false;
            }

            return true;
        }

        //public static bool IsLedavid(JewishCalendar jc)
        //{
        //    return jc.JewishMonth == 6 || (jc.JewishMonth == 7 && jc.JewishDayOfMonth <= 21);
        //}

        //public static bool IsVeyehiNoam(Geoposition position = null)
        //{
        //    DateTime now = GetCurrentDateTime(position);

        //    if (now.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        for (int i = 1; i <= 5; i++)
        //        {
        //            now += TimeSpan.FromDays(1);

        //            //Moadim moed = GetMoed(now);

        //            //if (moed.HasFlag(Moadim.Pesach) || moed.HasFlag(Moadim.Shavuot) || moed.HasFlag(Moadim.Sukkot))
        //            //{
        //            //    return false;
        //            //}

        //            Zmanim.HebrewCalendar.JewishCalendar c = new Zmanim.HebrewCalendar.JewishCalendar(now) { InIsrael = IsInIsrael() };

        //            int yomTov = c.YomTovIndex;

        //            switch (yomTov)
        //            {
        //                case JewishCalendar.ROSH_HASHANA:
        //                case JewishCalendar.YOM_KIPPUR:
        //                case JewishCalendar.PESACH:
        //                case JewishCalendar.SHAVUOS:
        //                case JewishCalendar.SUCCOS:
        //                case JewishCalendar.CHOL_HAMOED_PESACH:
        //                case JewishCalendar.CHOL_HAMOED_SUCCOS:
        //                case JewishCalendar.HOSHANA_RABBA:
        //                    return false;
        //            }
        //        }

        //        return true;
        //    }

        //    return false;
        //}

        public static bool IsAfterYomKippur(JewishCalendar jc)
        {
            DateTime yesterday = jc.Time.AddDays(-1);

            Zmanim.HebrewCalendar.JewishCalendar c = new Zmanim.HebrewCalendar.JewishCalendar(yesterday, IsInIsrael());

            return c.YomTovIndex == Zmanim.HebrewCalendar.JewishCalendar.YOM_KIPPUR;
        }

        //public static bool IsMizmorLetoda(Geoposition position = null)
        //{
        //    DateTime now = GetCurrentDateTime(position);

        //    Zmanim.HebrewCalendar.JewishDate jDate = new Zmanim.HebrewCalendar.JewishDate(now);

        //    if (jDate.JewishMonth == JewishDate.TISHREI && jDate.GregorianDayOfMonth == 9)///Erev Kippur
        //    {
        //        return false;
        //    }

        //    if (jDate.JewishMonth == JewishDate.NISSAN && jDate.JewishDayOfMonth >= 14 && jDate.JewishDayOfMonth <= 20)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public static bool HasMusafToday(JewishCalendar jc)
        {
            if (jc.RoshChodesh || jc.CholHamoed || jc.YomTovIndex == JewishCalendar.HOSHANA_RABBA)
            {
                return true;
            }

            return false;
        }

        public static bool IsTfillinTime(JewishCalendar jc)
        {
            return !jc.CholHamoed && jc.YomTovIndex != JewishCalendar.HOSHANA_RABBA;
        }

        public static bool IsInIsrael()
        {
            return Settings.IsInIsrael;

            //Geoposition position = LocationManager.GetNativePosition();

            //if (position == null || position.Coordinate == null)
            //{
            //    return true;
            //}

            //return IsInIsrael(position).Result;

            ////CivicAddressResolver resolver = new CivicAddressResolver();

            ////System.Device.Location.CivicAddress address = resolver.ResolveAddress(new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude));

            ////if (!address.IsUnknown)
            ////{
            ////    return string.Equals(address.StateProvince, "Israel", StringComparison.OrdinalIgnoreCase);
            ////}

            ////return true;
        }

        //private static async Task<bool> IsInIsrael(Geoposition position)
        //{
        //    //IList<Microsoft.Phone.Maps.Services.MapLocation> result = /*await*/ Query(position);

        //    //if (result == null)
        //    //{
        //    //}
        //    if (TimeZoneInfo.Local.StandardName == "")
        //    {

        //    }

        //    return true;
        //}

        public static string GetParasha(JewishCalendar jc)
        {
            if (jc.DayOfWeek < 7)
            {
                jc = Clone(jc);

                do
                {
                    jc.forward();
                } while (jc.DayOfWeek < 7);
            }

            return _dateFormatter.formatParsha(jc);
        }

        public static string GetDafYomi(JewishCalendar jc)
        {
            return _dateFormatter.formatDafYomiBavli(jc.DafYomiBavli);
        }

        public static string GetMoedTitle(JewishCalendar jc, bool includeIsruChag)
        {
            //return _dateFormatter.formatYomTov(jc);
            jc = Clone(jc);

            if (includeIsruChag)
            {
                jc.InIsrael = false;
            }

            switch (jc.YomTovIndex)
            {
                case -1:
                case JewishCalendar.CHANUKAH:
                    int candleIndex = jc.DayOfChanukah;

                    if (candleIndex != -1)
                    {
                        return CommonResources.ResourceManager.GetString("Chanuka" + candleIndex);
                    }

                    break;
                case JewishCalendar.ROSH_HASHANA:
                    return CommonResources.RoshHashanaTitle;
                case JewishCalendar.FAST_OF_GEDALYAH:
                    return CommonResources.GdaliahFastTitle;
                case JewishCalendar.YOM_KIPPUR:
                    return CommonResources.YomKippurTitle;
                case JewishCalendar.SUCCOS:
                    return CommonResources.SukkotTitle;
                case JewishCalendar.CHOL_HAMOED_SUCCOS:
                    return CommonResources.SukkotHmTitle;
                case JewishCalendar.HOSHANA_RABBA:
                    return CommonResources.HoshaanaRabaTitle;
                case JewishCalendar.SHEMINI_ATZERES:
                    return CommonResources.ShminiAtzeretTitle;
                case JewishCalendar.SIMCHAS_TORAH:
                    return CommonResources.SukkotDTitle;
                case JewishCalendar.TENTH_OF_TEVES:
                    return CommonResources.Tevet10thTitle;
                case JewishCalendar.TU_BESHVAT:
                    return CommonResources.Shvat15thTitle;
                case JewishCalendar.FAST_OF_ESTHER:
                    return CommonResources.TeanitEstherTitle;
                case JewishCalendar.PURIM:
                    return CommonResources.PurimTitle;
                case JewishCalendar.SHUSHAN_PURIM:
                    return CommonResources.ShushanPurimTitle;
                //case JewishCalendar.TaanitBechorot:
                //    return Resources.TeanitBechorotTitle;
                case JewishCalendar.CHOL_HAMOED_PESACH:
                    return CommonResources.PesachHmTitle;
                case JewishCalendar.PESACH:
                    if (jc.JewishDayOfMonth == 21)
                    {
                        return CommonResources.PesachDTitle;
                    }
                    return CommonResources.PesachTitle;
                case JewishCalendar.YOM_HAZIKARON:
                    return CommonResources.YomHazikaronTitle;
                case JewishCalendar.YOM_HAATZMAUT:
                    return CommonResources.YomHaazmautTitle;
                case JewishCalendar.PESACH_SHENI:
                    return CommonResources.Pesach2Title;
                case JewishCalendar.YOM_YERUSHALAYIM:
                    return CommonResources.YomYerushalaiymTitle;
                case JewishCalendar.SHAVUOS:
                    if (jc.JewishDayOfMonth == 7)
                    {
                        return CommonResources.ShavuotDTitle;
                    }
                    return CommonResources.ShavuotTitle;
                case JewishCalendar.SEVENTEEN_OF_TAMMUZ:
                    return CommonResources.Tamuz17thTitle;
                case JewishCalendar.TISHA_BEAV:
                    return CommonResources.Av9thTitle;
                case JewishCalendar.TU_BEAV:
                    return CommonResources.Av15thTitle;
                case JewishCalendar.PURIM_KATAN:
                    return CommonResources.PurimKatanTitle;
                //case JewishCalendar.EREV_ROSH_HASHANA:
                //    return 1;
                //case JewishCalendar.EREV_SHAVUOS:
                //case JewishCalendar.EREV_PESACH:
                //case JewishCalendar.EREV_SUCCOS:
                //case JewishCalendar.EREV_YOM_KIPPUR:
                default:
                    break;
            }

            if (jc.DayOfOmer == 33)
            {
                return CommonResources.LagBaomerTitle;
            }

            return null;
        }

        //public static string GetMoedTitle(Moadim moed)
        //{
        //    switch (moed)
        //    {
        //        case Moadim.None:
        //            return null;
        //        case Moadim.FastOfGedalia:
        //            return Resources.GdaliahFastTitle;
        //        case Moadim.Sukkot:
        //            return Resources.SukkotHmTitle;
        //        case Moadim.SukkotD:
        //            return Resources.SukkotDTitle;
        //        case Moadim.Chanukkah:
        //            return null;
        //        case Moadim.FastOfTevet:
        //            return Resources.Tevet10thTitle;
        //        case Moadim.TuBeShvat:
        //            return Resources.Shvat15thTitle;
        //        case Moadim.TaanitEsther:
        //            return Resources.TeanitEstherTitle;
        //        case Moadim.Purim:
        //            return Resources.PurimTitle;
        //        case Moadim.ShushanPurim:
        //            return Resources.ShushanPurimTitle;
        //        case Moadim.TaanitBechorot:
        //            return Resources.TeanitBechorotTitle;
        //        case Moadim.Pesach:
        //            return Resources.PesachHmTitle;
        //        case Moadim.PesachD:
        //            return Resources.PesachDTitle;
        //        case Moadim.YomHaAtzmaut:
        //            return Resources.YomHaazmautTitle;
        //        case Moadim.PesahSheni:
        //            return Resources.Pesach2Title;
        //        case Moadim.LagBaOmer:
        //            return Resources.LagBaomerTitle;
        //        case Moadim.YomYerushalayim:
        //            return Resources.YomYerushalaiymTitle;
        //        case Moadim.ShavuotD:
        //            return Resources.ShavuotDTitle;
        //        case Moadim.FastOfTammuz:
        //            return Resources.Tamuz17thTitle;
        //        case Moadim.TishaBeAv:
        //            return Resources.Av9thTitle;
        //        case Moadim.TuBeAv:
        //            return Resources.Av15thTitle;
        //        case Moadim.PurimKatan:
        //            return Resources.PurimKatanTitle;
        //        default:
        //            break;
        //    }

        //    return null;
        //}

        //private static /*Task<*/IList<Microsoft.Phone.Maps.Services.MapLocation>/*>*/ Query(Geoposition position)
        //{

        //    Microsoft.Phone.Maps.Services.ReverseGeocodeQuery q = new Microsoft.Phone.Maps.Services.ReverseGeocodeQuery();
        //    q.GeoCoordinate = new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude);

        //    IList<Microsoft.Phone.Maps.Services.MapLocation> result = null;

        //    q.QueryCompleted += (s, e) =>
        //    {
        //        result = e.Result;
        //    };

        //    //return Task<IList<Microsoft.Phone.Maps.Services.MapLocation>>.Run(new Func<IList<Microsoft.Phone.Maps.Services.MapLocation>>(() =>
        //    //{
        //    q.QueryAsync();

        //    while (q.IsBusy)
        //    { }

        //    return result;
        //    //}));
        //}

        public static bool IsRoshHodeshLammed(JewishCalendar jc)
        {
            return jc.JewishDayOfMonth == 30;
        }

        public static bool IsIbburTime(JewishCalendar jc)
        {
            return jc.JewishLeapYear && jc.JewishMonth >= JewishDate.TISHREI && jc.JewishMonth <= JewishDate.ADAR_II;
        }

        public static bool IsAseretYameyTshuva(this JewishCalendar jc)
        {
            if (jc == null)
            {
                throw new ArgumentNullException("jc");
            }

            int hday = jc.JewishDayOfMonth;

            return (hday >= 3 && hday <= 10 && jc.JewishMonth == JewishCalendar.TISHREI);
        }



        public static string GatNusachTitle()
        {
            Nusach nusach = Settings.Nusach;

            switch (nusach)
            {
                case Nusach.Ashkenaz:
                    return CommonResources.NusachAshkenazTitle;
                case Nusach.Sfard:
                    return CommonResources.NusachSfardTitle;
                case Nusach.EdotMizrach:
                    return CommonResources.NusachEdotMizrachTitle;
                case Nusach.Baladi:
                    return CommonResources.NusachBaladiTitle;
                default:
                    return CommonResources.UnknownNusach;
            }
        }

        public static bool ShowAttaChonantanu(JewishCalendar jc)
        {
            //return HebDateHelper.GetCurrentDateTime().DayOfWeek == DayOfWeek.Sunday;
            jc = Clone(jc);
            jc.back();
            int yomTov = jc.YomTovIndex;
            return jc.DayOfWeek == 7 || yomTov == JewishCalendar.PESACH || yomTov == JewishCalendar.SIMCHAS_TORAH || yomTov == JewishCalendar.SHEMINI_ATZERES || yomTov == JewishCalendar.SHAVUOS || yomTov == JewishCalendar.ROSH_HASHANA || yomTov == JewishCalendar.YOM_KIPPUR;
        }
    }

    public enum Prayer
    {
        Unknown, Shacharit, Mincha, Arvit, Mussaf
    }
}
