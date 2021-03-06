﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;

namespace PrayPal.Content
{
    public class ShmoneEsreAshkenaz : ShmoneEsreSfard
    {
        public ShmoneEsreAshkenaz(Prayer prayer)
            : base(prayer)
        { }

        protected override void AddPart3()
        {
            bool aseretYameyTshuva = DayInfo.AseretYameyTshuva;

            if (Prayer != Prayer.Arvit)
            {
                AddStringFormat(CommonPrayerTextProvider.Current.Kdusha, aseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, aseretYameyTshuva, false, AppResources.KdushaTitle, true);
                AddStringFormat(CommonPrayerTextProvider.Current.SE03, DayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, aseretYameyTshuva, false, AppResources.InTfilatYachidTitle);
            }
            else
            {
                AddStringFormat(CommonPrayerTextProvider.Current.SE03, aseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, aseretYameyTshuva);
            }
        }

        protected override void AddPart19()
        {
            if (!DayInfo.Teanit && (Prayer == Prayer.Mincha || Prayer == Prayer.Arvit))
            {
                Add(PrayersHelper.CreateParagraphForStringFormat(AshkenazPrayerTextProvider.Instance.SE19ShalomRav, DayInfo.AseretYameyTshuva ? new RunModel(CommonPrayerTextProvider.Current.SE19AYT, false, true) : null));
            }
            else
            {
                base.AddPart19();
            }
        }
    }
}
