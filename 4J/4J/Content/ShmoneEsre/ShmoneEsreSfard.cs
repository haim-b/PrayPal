﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    public class ShmoneEsreSfard : ShmoneEsreBase
    {
        public ShmoneEsreSfard(Prayer prayer)
            : base(prayer)
        { }

        protected override void AddOpening()
        {
            if (Prayer == Prayer.Mincha || Prayer == Prayer.Mussaf)
            {
                Add(CommonPrayerTextProvider.Current.KiShemHashem);
            }

            base.AddOpening();
        }

        protected override void AddPart16()
        {
            if (Settings.ShowVeanenu || IsMinchaInTeanit)
            {
                Add(CommonPrayerTextProvider.Current.SE16);

                if (IsMinchaInTeanit)
                {
                    Add(string.Format(CommonPrayerTextProvider.Current.Anenu, string.Empty), AppResources.InTfilatYachidTitle);
                }

                if (Settings.ShowVeanenu)
                {
                    //Add Va'Anenu:
                    Add(CommonPrayerTextProvider.Current.VaAnenu, AppResources.VeAnenuTitle);
                }

                Add(CommonPrayerTextProvider.Current.SE16B);
            }
            else
            {
                Add(string.Concat(CommonPrayerTextProvider.Current.SE16, " ", CommonPrayerTextProvider.Current.SE16B));
            }
        }

        protected override void AddPart3Musaf()
        {
            //TODO: In Hoshaana Raba put Kdushat Keter of Shabbat.
            Add(CommonPrayerTextProvider.Current.KdushatKeter, AppResources.KdushaTitle, true);

            AddStringFormat(CommonPrayerTextProvider.Current.SE03, CommonPrayerTextProvider.Current.SE03Hael, false, false, AppResources.InTfilatYachidTitle);
        }

        protected override void AddMussafHolidayPart()
        {
            string holidayPsukim = null;

            if (DayInfo.YomTov == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                holidayPsukim = CommonPrayerTextProvider.Current.MussafPesachPsukim1;
            }
            else if (DayInfo.YomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || DayInfo.YomTov == JewishCalendar.HOSHANA_RABBA)
            {
                switch (DayInfo.JewishCalendar.JewishDayOfMonth)
                {
                    case 16:
                        holidayPsukim = CommonPrayerTextProvider.Current.MussafSukkotPsukim1;
                        break;
                    case 17:
                        holidayPsukim = DayInfo.JewishCalendar.InIsrael ? CommonPrayerTextProvider.Current.MussafSukkotPsukim2 : CommonPrayerTextProvider.Current.MussafSukkotPsukim1;
                        break;
                    case 18:
                        holidayPsukim = DayInfo.JewishCalendar.InIsrael ? CommonPrayerTextProvider.Current.MussafSukkotPsukim3 : CommonPrayerTextProvider.Current.MussafSukkotPsukim2;
                        break;
                    case 19:
                        holidayPsukim = DayInfo.JewishCalendar.InIsrael ? CommonPrayerTextProvider.Current.MussafSukkotPsukim4 : CommonPrayerTextProvider.Current.MussafSukkotPsukim3;
                        break;
                    case 20:
                        holidayPsukim = DayInfo.JewishCalendar.InIsrael ? CommonPrayerTextProvider.Current.MussafSukkotPsukim5 : CommonPrayerTextProvider.Current.MussafSukkotPsukim4;
                        break;
                    case 21:
                        holidayPsukim = DayInfo.JewishCalendar.InIsrael ? CommonPrayerTextProvider.Current.MussafSukkotPsukim6 : CommonPrayerTextProvider.Current.MussafSukkotPsukim5;
                        break;
                    default:
                        break;
                }
            }

            if (holidayPsukim != null)
            {
                Add(holidayPsukim);
            }

            base.AddMussafHolidayPart();
        }
    }
}
