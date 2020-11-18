using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Content;
using PrayPal.Content.Prayers.Arvit;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.PrayPal.Content.LegacyModels;
using Tests.PrayPal.Content.LegacyTextProviders;
using Zmanim.HebrewCalendar;

namespace Tests.PrayPal.Content.Prayers
{
    [TestClass]
    public class MinchaTests
    {
        [TestMethod]
        public async Task TestMinchaSfard()
        {
            TestExecutor.PrepareNusach(Nusach.Sfard);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Sfard, jc, l), (l, t) => new MinchaSfard());
        }

        [TestMethod]
        public async Task TestMinchaEdotHaMizrach()
        {
            TestExecutor.PrepareNusach(Nusach.EdotMizrach);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.EdotMizrach, jc, l), (l, t) => new MinchaEdotHaMizrach());
        }

        [TestMethod]
        public async Task TestMinchaAshkenaz()
        {
            TestExecutor.PrepareNusach(Nusach.Ashkenaz);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Ashkenaz, jc, l), (l, t) => new MinchaAshkenaz());
        }

        private IEnumerable<string> CreateFromLegacyDebuggedCode(Nusach nusach, JewishCalendar jc, ILocationService locationService)
        {
            HebrewDateFormatter psalmFormatter = new HebrewDateFormatter() { UseGershGershayim = true };

            List<TextsModel> texts = new List<TextsModel>();

            if (nusach == Nusach.EdotMizrach)
            {
                AddEdotMizrachTextsBeforeMincha(texts);
            }

            texts.Add(new TextsModel(TestExecutor.GetAshrey()));

            if (nusach == Nusach.EdotMizrach)
            {
                texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.ResourceManager.GetString("AfterAshreyOfMincha")));
            }

            texts[texts.Count - 1].Add(TestExecutor.CreateHaziKadish(jc));

            int yomTov = jc.YomTovIndex;

            if (yomTov == -1 && jc.RoshChodesh)
            {
                yomTov = JewishCalendar.ROSH_CHODESH;
            }

            bool isTachanunDay = HebDateHelper.IsTachanunDay(Prayer.Mincha, nusach, jc);

            if (TestExecutor.AddTorahReading(texts, nusach, Prayer.Mincha, jc, isTachanunDay))
            {
                TestExecutor.AddTorahEnding(texts, nusach, Prayer.Mincha);
            }

            TextsModel shmoneEsre = new TextsModel();
            shmoneEsre.Title = AppResources.SE_Title;
            shmoneEsre.AddRange(TestExecutor.CreateShmoneEsre(Prayer.Mincha, jc, yomTov, nusach));
            texts.Add(shmoneEsre);

            if (jc.YomTovIndex == JewishCalendar.TISHA_BEAV)
            {
                //System.Windows.MessageBox.Show(AppResources.TishaBeavMessage);
            }

            TestExecutor.AddTachanun(texts, Prayer.Mincha, nusach, jc, isTachanunDay);

            texts.Add(TestExecutor.CreateKadishShalem(jc));

            if (nusach == Nusach.EdotMizrach)
            {
                ///למנצח
                TextsModel psalm67 = new TextsModel();
                psalm67.Title = string.Format(AppResources.PsalmTitle, psalmFormatter.formatHebrewNumber(67));
                psalm67.Add(new ParagraphModel(Psalms.Psalm67));
                texts.Add(psalm67);

                texts.Add(TestExecutor.CreateKadishYatom(jc));

                if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    TextsModel psalm93 = new TextsModel();
                    psalm93.Title = string.Format(AppResources.PsalmTitle, psalmFormatter.formatHebrewNumber(93));
                    psalm93.Add(new ParagraphModel(Psalms.Psalm93));
                    texts.Add(psalm93);
                }

                if (jc.Taanis)
                {
                    TextsModel psalm102 = new TextsModel();
                    psalm102.Title = string.Format(AppResources.PsalmTitle, psalmFormatter.formatHebrewNumber(102));
                    psalm102.Add(new ParagraphModel(Psalms.Psalm102));
                    texts.Add(psalm102);
                }
            }

            TextsModel aleinu = new TextsModel();
            aleinu.Title = AppResources.AleinuLeshabeachTitle;
            aleinu.Add(new ParagraphModel(PrayTexts.AleinuLeshabeach));
            texts.Add(aleinu);

            if (nusach == Nusach.Ashkenaz || nusach == Nusach.Sfard)
            {
                texts.Add(TestExecutor.CreateKadishYatom(jc));
            }

            if (nusach == Nusach.Sfard && new DayJewishInfo(jc).ShouldSayLeDavid())
            {
                TestExecutor.AddLedavid(texts, jc);
            }

            return texts.SelectMany(t => t).Select(p => p.Content);
        }

        private static void AddEdotMizrachTextsBeforeMincha(List<TextsModel> texts)
        {
            texts.Add(new TextsModel(AppResources.VersesBeforeMinchaTitle, PrayTexts.ResourceManager.GetString("LeshemYichudMincha")));
            texts.Add(TestExecutor.GetPsalm(84));//new TextsModel(string.Format(AppResources.PsalmTitle, psalmFormatter.formatHebrewNumber(84)), Psalms.Psalm84));
            texts.Add(new TextsModel(AppResources.KtoretHasamimTitle, PrayTexts.ResourceManager.GetString("KtoretHasamim1"), PrayTexts.ResourceManager.GetString("KtoretHasamim2")));
            texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.PitumHaktoret1));
            texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.PitumHaktoret2));
        }

    }
}
