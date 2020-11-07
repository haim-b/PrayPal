using System;
using System.Collections.Generic;
using System.Text;
using Zmanim.HebrewCalendar;

namespace PrayPal.Common
{
    public static class DayJewishInfoExtensions
    {

        public static bool IsMoridHatal(this DayJewishInfo info)
        {
            if (info.JewishCalendar.JewishMonth == 1) ///Nissan
            {
                if (info.JewishCalendar.JewishDayOfMonth < 15) ///Pesach 1st
                {
                    return false;
                }

                return true;
            }
            else if (info.JewishCalendar.JewishMonth == 7) ///Tishrey
            {
                if (info.JewishCalendar.JewishDayOfMonth < 22) ///Shmini Atzeret
                {
                    return true;
                }

                return false;
            }
            else if (info.JewishCalendar.JewishMonth < 7)
            {
                return true;
            }

            return false;
        }

        public static bool IsVetenBracha(this DayJewishInfo info)
        {
            if (info.JewishCalendar.JewishMonth == 1) //Nissan
            {
                if (info.JewishCalendar.JewishDayOfMonth < 15) //Pesach 1st
                {
                    return false;
                }

                return true;
            }
            else if (info.JewishCalendar.JewishMonth < 8) //Between Nissan and Heshvan
            {
                return true;
            }
            else if (info.JewishCalendar.JewishMonth == 8) //Heshvan
            {
                if (Settings.IsInIsrael)
                {
                    return info.JewishCalendar.JewishDayOfMonth < 7; //ז' חשוון
                }
                else
                {
                    return true;
                }
            }
            else if (!Settings.IsInIsrael)
            {
                DateTime now = DateTime.Now;

                if (now.Month >= 4 && now.Month < 12)
                {
                    return true;
                }

                // Abroad, they start to ask for rain since December 5th on a regular year, and December 6th on a leap year:
                return now.Day < 5 + DateTime.DaysInMonth(now.Year, 2) - 28;
            }

            return false;
        }

        public static bool ShowAttaChonantanu(this DayJewishInfo info)
        {
            JewishCalendar jc = HebDateHelper.Clone(info.JewishCalendar);
            jc.back();
            int yomTov = jc.YomTovIndex;
            return jc.DayOfWeek == 7 || yomTov == JewishCalendar.PESACH || yomTov == JewishCalendar.SIMCHAS_TORAH || yomTov == JewishCalendar.SHEMINI_ATZERES || yomTov == JewishCalendar.SHAVUOS || yomTov == JewishCalendar.ROSH_HASHANA || yomTov == JewishCalendar.YOM_KIPPUR;
        }
    }
}
