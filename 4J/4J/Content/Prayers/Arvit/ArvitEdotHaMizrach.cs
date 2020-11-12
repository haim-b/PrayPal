﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;

namespace PrayPal.Content.Prayers.Arvit
{
    [Nusach(Nusach.EdotMizrach)]
    public class ArvitEdotHaMizrach : ArvitBase
    {
        public ArvitEdotHaMizrach(ILocationService locationService)
            : base(locationService)
        { }

        protected override void AddVersesBeforeArvit()
        {
            if (_dayInfo.JewishCalendar.RoshChodesh)
            {
                Add(AppResources.BarchiNafshiTitle, Psalms.Psalm104);
            }

            Add(AppResources.VersesBeforeArvitTitle,
                new ParagraphModel(EdotHaMizrachPrayerTextProvider.Instance.LeshemYichudArvit),
                new ParagraphModel(CommonPrayerTextProvider.Current.VersesBeforeArvit),
                PrayersHelper.GetHalfKadish(_dayInfo));
        }

        protected override ShmoneEsreBase GetShmoneEsre()
        {
            return new ShmoneEsreEdotHamizrach(Prayer.Arvit);
        }

        protected override void AddEnding()
        {
            /*Nusach Edot HaMizrach ending has the following structure:
             * - Barchu
             * - Aleinu Leshabeach
             * - Sfirat HaOmer (if relevant)
             */

            AddAleinuLeshabeach();

            if (_dayInfo.DayOfOmer != -1)
            {
                AddSfiratHaOmer();
            }
        }

        protected override void AddAleinuLeshabeach()
        {
            Add(AppResources.AleinuLeshabeachTitle, CommonPrayerTextProvider.Current.Barchu, CommonPrayerTextProvider.Current.AleinuLeshabeach);
        }

        protected override void AddTextBeforeSforatHaOmer(SpanModel sfiratHaOmer)
        {
            sfiratHaOmer.Add(CommonPrayerTextProvider.Current.Barchu, EdotHaMizrachPrayerTextProvider.Instance.PreSfiratHaOmer1);
        }
    }
}