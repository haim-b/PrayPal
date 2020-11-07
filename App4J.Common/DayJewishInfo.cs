using System;
using System.Collections.Generic;
using System.Text;
using Zmanim.HebrewCalendar;

namespace PrayPal.Common
{
    public class DayJewishInfo
    {
        private readonly JewishCalendar _jc;
        private bool? _AYT;
        private int? _yomTov;
        private bool? _tachanunDay;
        private int? _dayOfOmer;

        public DayJewishInfo(JewishCalendar jc/*, bool createdWithLocation*/)
        {
            if (jc == null)
            {
                throw new ArgumentNullException("jc");
            }

            _jc = jc;
            //CreatedWithLocation = createdWithLocation;
        }

        public bool CreatedWithLocation { get; private set; }

        public bool AseretYameyTshuva
        {
            get
            {
                if (_AYT == null)
                {
                    int hday = _jc.JewishDayOfMonth;

                    _AYT = hday >= 3 && hday <= 10 && _jc.JewishMonth == JewishCalendar.TISHREI;
                }

                return _AYT.Value;
            }
        }

        public bool Teanit
        {
            get { return _jc.Taanis; }
        }

        public JewishCalendar JewishCalendar
        {
            get { return _jc; }
        }

        public bool IsCholHamoed
        {
            get { return YomTov == JewishCalendar.SUCCOS || YomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || YomTov == JewishCalendar.HOSHANA_RABBA || YomTov == JewishCalendar.PESACH || YomTov == JewishCalendar.CHOL_HAMOED_PESACH; }
        }

        public int YomTov
        {
            get
            {
                if (_yomTov == null)
                {
                    _yomTov = _jc.YomTovIndex;
                }

                return _yomTov.Value;
            }
        }

        public bool IsIbburTime
        {
            get { return _jc.JewishLeapYear && _jc.JewishMonth >= JewishDate.TISHREI && _jc.JewishMonth <= JewishDate.ADAR_II; }
        }

        public bool IsTachanunDay(Nusach nusach, bool checkingForMincha = false)
        {
            if (_tachanunDay == null)
            {
                if (!IsTachanunDayPrivate(nusach, false))
                {
                    _tachanunDay = false;
                    goto Result;
                }

                if (checkingForMincha)
                {
                    if (_jc.DayOfWeek == 6) ///Friday has no tachanun in Mincha
                    {
                        _tachanunDay = false;
                        goto Result;
                    }

                    JewishCalendar clone = Clone(_jc);
                    clone.forward();

                    DayJewishInfo nextDay = new DayJewishInfo(clone);

                    if (nextDay.YomTov != JewishCalendar.PESACH_SHENI && !nextDay.IsTachanunDayPrivate(nusach, true))
                    {
                        _tachanunDay = false;
                        goto Result;
                    }
                }

                _tachanunDay = true;
                goto Result;
            }

        Result:
            return (bool)_tachanunDay;
        }

        private static JewishCalendar Clone(JewishCalendar jc)
        {
            return new JewishCalendar(jc.JewishYear, jc.JewishMonth, jc.JewishDayOfMonth, jc.InIsrael) { UseModernHolidays = jc.UseModernHolidays };
        }

        private bool IsTachanunDayPrivate(Nusach nusach, bool isDayBefore)
        {
            if (_jc.RoshChodesh)
            {
                return false;
            }

            bool isInIsrael = _jc.InIsrael;

            ///Mark not in israel to include issru chag:
            try
            {
                _jc.InIsrael = false;

                switch (_jc.YomTovIndex)
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
                _jc.InIsrael = isInIsrael;
            }

            if (_jc.DayOfOmer == 33)
            {
                return false;
            }

            if (_jc.JewishMonth == 1) //Nissan
            {
                return false;
            }

            if (_jc.JewishMonth == 3 && _jc.JewishDayOfMonth >= 1 && _jc.JewishDayOfMonth <= 12) //Sivan
            {
                return false;
            }

            if (_jc.JewishMonth == 6 && _jc.JewishDayOfMonth >= 29) //before Rosh Hashana
            {
                if (nusach == Nusach.EdotMizrach || nusach == Nusach.Baladi)
                {
                    return true;
                }

                if (!isDayBefore)
                {
                    return false;
                }
            }

            if (_jc.JewishMonth == 7 && _jc.JewishDayOfMonth >= 9) //Tishrey
            {
                if ((nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz) && isDayBefore && _jc.JewishDayOfMonth == 9) //before Yom Kippur
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        public int DayOfOmer
        {
            get
            {
                if (_dayOfOmer == null)
                {
                    _dayOfOmer = _jc.DayOfOmer;
                }

                return (int)_dayOfOmer;
            }
        }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)(_jc.DayOfWeek - 1);
            }
        }
    }
}
