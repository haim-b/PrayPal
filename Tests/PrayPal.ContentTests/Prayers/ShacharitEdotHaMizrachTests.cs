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
    public class ShacharitEdotHaMizrachTests
    {
        [TestMethod]
        public async Task TestShacharitEdotHaMizrach()
        {
            TestExecutor.PrepareNusach(Nusach.EdotMizrach);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.EdotMizrach, jc, l), (l, t) => new ShacharitEdotHaMizrach(DummyPermissionsService.Instance));
        }

        private IEnumerable<string> CreateFromLegacyDebuggedCode(Nusach nusach, JewishCalendar jc, ILocationService locationService)
        {
            HebrewDateFormatter psalmFormatter = new HebrewDateFormatter() { UseGershGershayim = true };

            List<TextsModel> texts = new List<TextsModel>();

            var haziKadish = TestExecutor.CreateHaziKadish(jc);
            var kadishShalem = TestExecutor.CreateKadishShalem(jc);
            var kadishYatom = TestExecutor.CreateKadishYatom(jc);
            var kadishDerabanan = TestExecutor.CreateKadishDerabanan(jc);

            int yomTov = jc.YomTovIndex;


            // סדר השכמה
            TextsModel t = new TextsModel(AppResources.SederHashkamaTitle, PrayTexts.ResourceManager.GetString("GiluyDaat"), PrayTexts.ModeAni);

            t.Add(new ParagraphModel(AppResources.MorningNetilatYadayimInstruction + ":" + Environment.NewLine + PrayTexts.NetilatYadayimBlessing));
            t.Add(new ParagraphModel(PrayTexts.AsherYatzar) { Title2 = AppResources.AsherYatzarTitle });
            t.Add(new ParagraphModel(PrayTexts.BirkatNeshama));// { Title2 = AppResources.BirkatNeshamaTitle });

            texts.Add(t);

            // ברכות השחר
            t = new TextsModel(AppResources.BirkotHashacharTitle);
            t.AddRange(jc.YomTovIndex == JewishCalendar.TISHA_BEAV ? PrayTexts.ResourceManager.GetString("BirkotHashachar1Av9th") : PrayTexts.BirkotHashachar1, PrayTexts.BirkotHashachar2, PrayTexts.BirkotHashachar3, PrayTexts.BirkotHashachar4);

            texts.Add(t);

            // ברכות התורה
            t = new TextsModel(new ParagraphModel(PrayTexts.BirkotHatorah, AppResources.BirkotHatorahTitle));

            t.Add(new ParagraphModel(PrayTexts.ParashatBirkatCohanim));

            texts.Add(t);

            // פתח אליהו
            texts.Add(new TextsModel(AppResources.PatachEliyahuTitle, PrayTexts.ResourceManager.GetString("PatachEliyahu1"), PrayTexts.ResourceManager.GetString("PatachEliyahu2"),
                PrayTexts.ResourceManager.GetString("PatachEliyahu3"), PrayTexts.ResourceManager.GetString("PatachEliyahu4"), PrayTexts.ResourceManager.GetString("PatachEliyahu5"),
                PrayTexts.ResourceManager.GetString("PatachEliyahu6")));



            bool isTfillinTime = HebDateHelper.IsTfillinTime(jc);

            // טלית ותפילין
            if (isTfillinTime)
            {
                t = new TextsModel(new ParagraphModel(PrayTexts.AtifatTalit, AppResources.TalitAndTfillinTitle) { Title2 = AppResources.AtifatTalitTitle });

                t.Add(new ParagraphModel(PrayTexts.HanachatTfillin) { Title2 = AppResources.HanachatTfillinTitle });
                t.Add(PrayTexts.ResourceManager.GetString("HanachatTfillin2"));
                t.AddRange(PrayTexts.ParashahAfterTfillin1, PrayTexts.ParashahAfterTfillin2);
            }
            else
            {
                t = new TextsModel(new ParagraphModel(PrayTexts.AtifatTalit, AppResources.AtifatTalitTitle));
            }

            texts.Add(t);

            texts.Add(new TextsModel(AppResources.VatitpallelChannahTitle, PrayTexts.ResourceManager.GetString("VatitpalelChannah"), PrayTexts.ResourceManager.GetString("PreShacharit1")));
            texts.Add(new TextsModel(AppResources.LeshemYichudTitle, PrayTexts.ResourceManager.GetString("PreShacharit2")));

            TextsModel p = new TextsModel(AppResources.ParashatHaakedahTitle, PrayTexts.ResourceManager.GetString("PreShacharit3"),
            PrayTexts.ResourceManager.GetString("ParashatHaakedah"),
            PrayTexts.ResourceManager.GetString("PreShacharit4"),
            PrayTexts.ResourceManager.GetString("PreShacharit5"),
            PrayTexts.ResourceManager.GetString("PreShacharit6"),
            PrayTexts.ResourceManager.GetString("PreShacharit7"),
            PrayTexts.ResourceManager.GetString("PreShacharit8"),
            PrayTexts.ResourceManager.GetString("PreShacharit9"),
            PrayTexts.ResourceManager.GetString("PreShacharit10"),
            PrayTexts.ResourceManager.GetString("AnnaBechoach"),
            PrayTexts.ResourceManager.GetString("PreShacharit11"));

            texts.Add(p);

            texts.Add(new TextsModel(AppResources.KtoretHasamimTitle, PrayTexts.ResourceManager.GetString("KtoretHasamim1"), PrayTexts.ResourceManager.GetString("KtoretHasamim2")));
            texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.PitumHaktoret1));
            texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.PitumHaktoret2));



            // ברייתא דרבי ישמעאל
            texts.AddRange(new[]{new TextsModel(new ParagraphModel(PrayTexts.BrayitaDerabiYishmael, AppResources.BrayitaDerabiYishmaelTitle)),
                kadishDerabanan });


            // תפילות השחר
            t = new TextsModel(AppResources.TfilotHashacharTitle);
            t.AddRange(PrayTexts.MizmorLifneyHaAron1, PrayTexts.MizmorLifneyHaAron2, PrayTexts.MizmorLifneyHaAron3, PrayTexts.ResourceManager.GetString("Psalm30Shacharit"));

            bool isAYT = jc.IsAseretYameyTshuva();

            t.AddRange(string.Format(PrayTexts.ShacharitVerse1, isAYT ? PrayTexts.ResourceManager.GetString("ShacharitVerse1AYT") : ""), Psalms.Psalm67);

            texts.Add(t);


            // פסוקי דזמרה
            t = new TextsModel(AppResources.PsukeyDezimraTitle);
            t.Add(PrayTexts.BaruchSheamar);

            t.Add(Psalms.Psalm100);

            t.AddRange(PrayTexts.ShacharitVerse2, PrayTexts.Ashrey, Psalms.Psalm146, Psalms.Psalm147, Psalms.Psalm148, Psalms.Psalm149, Psalms.Psalm150,
                Psalms.PsalmEnding, PrayTexts.ShacharitVerse3, PrayTexts.ShacharitVerse4, PrayTexts.ShacharitVerse5);

            texts.Add(t);


            // שירת הים וישתבח
            texts.Add(new TextsModel(AppResources.ShiratHayamTitle, PrayTexts.ShiratHayam));

            t = new TextsModel(AppResources.YishtabachTitle, PrayTexts.Yishtabach);

            if (isAYT)
            {
                t.Add(new ParagraphModel(Psalms.Psalm130));
            }

            t.Add(haziKadish);

            texts.Add(t);

            // ברכות קריאת שמע
            t = new TextsModel(AppResources.BrachotKriatShmaTitle);
            t.AddRange(PrayTexts.Barchu, PrayTexts.BrachotBeforeShmaShacharit1, PrayTexts.KadoshKadosh, PrayTexts.BrachotBeforeShmaShacharit2, PrayTexts.BrachotBeforeShmaShacharit3);

            texts.Add(t);

            // קריאת שמע
            t = new TextsModel(AppResources.KriatShmaTitle);
            t.AddRange(PrayTexts.KriatShma1, PrayTexts.KriatShma2, PrayTexts.KriatShma3);

            t.AddRange(PrayTexts.BrachotAfterShmaShacharit1, PrayTexts.BrachotAfterShmaShacharit2, PrayTexts.BrachotAfterShmaShacharit3);

            texts.Add(t);

            // שמונה עשרה
            t = new TextsModel();
            t.Title = AppResources.SE_Title;
            t.AddRange(TestExecutor.CreateShmoneEsre(Prayer.Shacharit, jc, yomTov, nusach));

            texts.Add(t);

            bool isTachanunDay = HebDateHelper.IsTachanunDay(Prayer.Shacharit, nusach, jc);

            // תחנון
            TestExecutor.AddTachanun(texts, Prayer.Shacharit, nusach, jc, isTachanunDay);

            // הלל
            ShacharitSfardTest.AddHallel(texts, jc, yomTov, nusach);

            bool hasMusaf = HebDateHelper.HasMusafToday(jc);

            if (!hasMusaf)
            {
                texts[texts.Count - 1].Add(haziKadish);
            }
            else
            {
                texts.Add(kadishShalem);
            }

            // קריאת התורה
            bool torahReadingAdded = TestExecutor.AddTorahReading(texts, nusach, Prayer.Shacharit, jc, isTachanunDay);

            // אשרי
            texts.Add(new TextsModel(AppResources.AshreyTitle, PrayTexts.ResourceManager.GetString("YehiChasdecha"), TestExecutor.GetAshrey().Content));

            // למנצח, ובא לציון
            t = new TextsModel(AppResources.KdushaDesidraTitle);

            if (isTachanunDay)
            {
                t.Add(Psalms.Psalm20);
            }

            if (jc.YomTovIndex != JewishCalendar.TISHA_BEAV)
            {
                t.Add(PrayTexts.UvaLetzion);
            }

            t.AddRange(PrayTexts.VeataKadosh1, PrayTexts.VeataKadosh2);

            texts.Add(t);

            // (שיר של יום (ביום של מוסף
            if (hasMusaf)
            {
                texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.ResourceManager.GetString("PreVerseOfDay2")));

                ShacharitSfardTest.AddDayVerse(texts, nusach, jc, isTachanunDay);
            }

            // קדיש שלם
            if (!hasMusaf)
            {
                texts.Add(kadishShalem.Clone());
            }

            if (torahReadingAdded)
            {
                TestExecutor.AddTorahEnding(texts, nusach, Prayer.Shacharit);
            }

            if (hasMusaf)
            {
                texts[texts.Count - 1].Add(haziKadish);

                texts.Add(new TextsModel(TestExecutor.CreateShmoneEsreMussaf(jc, nusach).ToArray()) { Title = AppResources.MussafTitle });

                texts.Add(kadishShalem.Clone());

                texts.Add(new TextsModel(AppResources.BarchiNafshiTitle, Psalms.Psalm104));
            }
            // (שיר של יום (רגיל
            if (!hasMusaf)
            {
                if (isTachanunDay)
                {
                    texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.ResourceManager.GetString("PreVerseOfDay1")));
                }

                texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.ResourceManager.GetString("PreVerseOfDay2")));

                ShacharitSfardTest.AddDayVerse(texts, nusach, jc, isTachanunDay);
            }

            // קווה אל ה'
            texts.Add(new TextsModel(AppResources.PrayerEndingTitle, PrayTexts.KavehElHashem, PrayTexts.ResourceManager.GetString("KtoretHasamim2"), PrayTexts.PitumHaktoret1, PrayTexts.PitumHaktoret2, PrayTexts.KavehElHashemEnding));

            // קדיש דרבנן
            texts.Add(kadishDerabanan.Clone());

            // עלינו לשבח
            TextsModel aleinu = new TextsModel();
            aleinu.Title = AppResources.AleinuLeshabeachTitle;

            aleinu.Add(new ParagraphModel(PrayTexts.Barchu));

            aleinu.Add(new ParagraphModel(PrayTexts.AleinuLeshabeach));
            texts.Add(aleinu);


            return texts.SelectMany(t => t).Select(p => p.Content);
        }

    }
}
