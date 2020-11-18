using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrayPal.Common;
using PrayPal.Common.Services;
using Tests.PrayPal.Content.LegacyModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zmanim;
using Zmanim.HebrewCalendar;
using PrayPal.Resources;
using Tests.PrayPal.Content.LegacyTextProviders;
using System.Linq;
using System.Reflection;
using PrayPal.Content;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Tests.PrayPal.Content
{
    [TestClass]
    public class BirkatHamazonTests
    {
        [TestMethod]
        public async Task TestBirkatHamazonSfard()
        {
            TestExecutor.PrepareNusach(Nusach.Sfard);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Sfard, jc), (l, t) => new BirkatHamazonSfard());
        }

        [TestMethod]
        public async Task TestBirkatHamazonEdotHaMizrach()
        {
            TestExecutor.PrepareNusach(Nusach.EdotMizrach);

            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.EdotMizrach, jc), (l, t) => new BirkatHamazonEdotHaMizrach());
        }

        [TestMethod]
        public async Task TestBirkatHamazonAshkenaz()
        {
            TestExecutor.PrepareNusach(Nusach.Ashkenaz);

            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Ashkenaz, jc), (l, t) => new BirkatHamazonSfard());
        }

        private static IEnumerable<string> CreateFromLegacyDebuggedCode(Nusach nusach, JewishCalendar jc)
        {
            TextsModel t = new TextsModel();

            int yomTov = jc.YomTovIndex;

            if (yomTov == -1 && jc.RoshChodesh)
            {
                yomTov = JewishCalendar.ROSH_CHODESH;
            }

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                if (HebDateHelper.IsTachanunDay(Prayer.Shacharit, nusach, jc))
                {
                    t.Add(Psalms.Psalm137);
                }
                else
                {
                    t.Add(Psalms.Psalm126);
                }
            }
            else if (nusach == Nusach.EdotMizrach)
            {
                t.Add(new ParagraphModel(PrayTexts.ResourceManager.GetString("BeforeBirkatHamazon")) { IsCollapsible = true, Title2 = AppResources.BlessingPreparation });
            }

            t.Add(new ParagraphModel(PrayTexts.Zimmun) { IsCollapsible = true, Title2 = AppResources.ZimmunTitle });

            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P1) { Title2 = AppResources.BirkatHamazonTitle });
            t.Add(PrayTexts.BirkatHamazon_P2);

            if (yomTov == JewishCalendar.CHANUKAH)
            {
                t.Add(new ParagraphModel(PrayTexts.AlHanissimHannukah) { Title2 = AppResources.AlHanissimHannukahTitle });
            }
            else if (yomTov == JewishCalendar.PURIM)
            {
                t.Add(new ParagraphModel(PrayTexts.AlHanissimPurim) { Title2 = AppResources.AlHanissimPurimTitle });
            }

            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P3));
            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P4));

            ParagraphModel yaalehVeYavo = TestExecutor.GetYaalehVeYavo(jc);

            if (yaalehVeYavo != null)
            {
                t.Add(yaalehVeYavo);
            }

            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P5));
            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P6));
            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P7));
            t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P8) { Title2 = nusach == Nusach.EdotMizrach ? AppResources.BirkatHaoreach : null });

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_P9));
            }

            if (jc.RoshChodesh)
            {
                t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_RoshHodesh));
            }
            else if (yomTov == JewishCalendar.SUCCOS || yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA)
            {
                t.Add(new ParagraphModel(PrayTexts.BirkatHamazon_Sukkot));
            }

            string magdil = PrayTexts.Magdil;

            if (jc.RoshChodesh || yomTov == JewishCalendar.SUCCOS || yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA || yomTov == JewishCalendar.PESACH || yomTov == JewishCalendar.CHOL_HAMOED_PESACH || yomTov == JewishCalendar.CHANUKAH)
            {
                magdil = PrayTexts.Migdol;
            }

            ParagraphModel p10 = new ParagraphModel(string.Format(PrayTexts.BirkatHamazon_P10, magdil));
            t.Add(p10);

            if (nusach == Nusach.EdotMizrach)
            {
                t.Add(TestExecutor.GetOseShalom(jc));
            }

            return t.Select(p => p.Content);
        }


    }

}
