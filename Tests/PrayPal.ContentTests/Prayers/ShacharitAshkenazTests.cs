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
    public class ShacharitAshkenazTests
    {
        [TestMethod]
        public async Task TestShacharitAshkenaz()
        {
            TestExecutor.PrepareNusach(Nusach.Ashkenaz);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Ashkenaz, jc, l), (l, t) => new ShacharitAshkenaz(DummyPermissionsService.Instance));
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


            ///סדר השכמה
            TextsModel t = new TextsModel(AppResources.SederHashkamaTitle, PrayTexts.ModeAni);

            t.Add(new ParagraphModel(AppResources.MorningNetilatYadayimInstruction + ":" + Environment.NewLine + PrayTexts.NetilatYadayimBlessing));
            t.Add(new ParagraphModel(PrayTexts.AsherYatzar) { Title2 = AppResources.AsherYatzarTitle });
            t.Add(new ParagraphModel(PrayTexts.BirkatNeshama));// { Title2 = AppResources.BirkatNeshamaTitle });

            texts.Add(t);

            ///ברכות התורה
            t = new TextsModel(new ParagraphModel(PrayTexts.BirkotHatorah, AppResources.BirkotHatorahTitle));

            t.Add(new ParagraphModel(PrayTexts.ParashatBirkatCohanim));

            texts.Add(t);


            bool isTfillinTime = HebDateHelper.IsTfillinTime(jc);

            ///טלית ותפילין
            if (isTfillinTime)
            {
                t = new TextsModel(new ParagraphModel(PrayTexts.AtifatTalit, AppResources.TalitAndTfillinTitle) { Title2 = AppResources.AtifatTalitTitle });

                t.Add(new ParagraphModel(PrayTexts.HanachatTfillin) { Title2 = AppResources.HanachatTfillinTitle });
                t.AddRange(PrayTexts.ParashahAfterTfillin1, PrayTexts.ParashahAfterTfillin2);
            }
            else
            {
                t = new TextsModel(new ParagraphModel(PrayTexts.AtifatTalit, AppResources.AtifatTalitTitle));
            }


            texts.Add(t);




            ///ברכות השחר
            t = new TextsModel(AppResources.BirkotHashacharTitle);
            t.AddRange(PrayTexts.BirkotHashachar1, PrayTexts.BirkotHashachar2, PrayTexts.BirkotHashachar3, PrayTexts.BirkotHashachar4);

            texts.Add(t);


            ///ברייתא דרבי ישמעאל
            texts.AddRange(new[]{new TextsModel(new ParagraphModel(PrayTexts.BrayitaDerabiYishmael, AppResources.BrayitaDerabiYishmaelTitle)),
                kadishDerabanan });


            ///פסוקי דזמרה
            t = new TextsModel(AppResources.PsukeyDezimraTitle);
            t.Add(Psalms.Psalm30);

            ///קדיש יתום
            t.Add(new ParagraphModel(kadishYatom[0].Content) { Title2 = AppResources.KadishYatomTitle });

            for (int i = 1; i < kadishYatom.Count; i++)
            {
                t.Add(new ParagraphModel(kadishYatom[i].Content));
            }

            ///ברוך שאמר
            t.Add(PrayTexts.BaruchSheamar);

            ///הודו
            t.AddRange(PrayTexts.MizmorLifneyHaAron1, PrayTexts.MizmorLifneyHaAron2, PrayTexts.MizmorLifneyHaAron3);

            if (ShacharitSfardTest.IsMizmorLetoda(jc))
            {
                t.Add(Psalms.Psalm100);
            }

            t.AddRange(PrayTexts.ShacharitVerse2, PrayTexts.Ashrey, Psalms.Psalm146, Psalms.Psalm147, Psalms.Psalm148, Psalms.Psalm149, Psalms.Psalm150,
                Psalms.PsalmEnding, PrayTexts.ShacharitVerse3, PrayTexts.ShacharitVerse4, PrayTexts.ShacharitVerse5);

            texts.Add(t);


            ///שירת הים וישתבח
            texts.Add(new TextsModel(AppResources.ShiratHayamTitle, PrayTexts.ShiratHayam));

            t = new TextsModel(AppResources.YishtabachTitle, PrayTexts.Yishtabach);

            if (jc.IsAseretYameyTshuva())
            {
                t.Add(new ParagraphModel(Psalms.Psalm130));
            }

            t.Add(haziKadish);

            texts.Add(t);

            ///ברכות קריאת שמע
            t = new TextsModel(AppResources.BrachotKriatShmaTitle);
            t.AddRange(PrayTexts.Barchu, PrayTexts.BrachotBeforeShmaShacharit1, PrayTexts.KadoshKadosh, PrayTexts.BrachotBeforeShmaShacharit2, PrayTexts.BrachotBeforeShmaShacharit3);

            texts.Add(t);

            ///קריאת שמע
            t = new TextsModel(AppResources.KriatShmaTitle);
            t.AddRange(PrayTexts.KriatShma1, PrayTexts.KriatShma2, PrayTexts.KriatShma3);

            t.AddRange(PrayTexts.BrachotAfterShmaShacharit1, PrayTexts.BrachotAfterShmaShacharit2, PrayTexts.BrachotAfterShmaShacharit3);

            texts.Add(t);

            ///שמונה עשרה
            t = new TextsModel();
            t.Title = AppResources.SE_Title;
            t.AddRange(TestExecutor.CreateShmoneEsre(Prayer.Shacharit, jc, yomTov, nusach));

            texts.Add(t);

            bool isTachanunDay = HebDateHelper.IsTachanunDay(Prayer.Shacharit, nusach, jc);

            ///תחנון
            TestExecutor.AddTachanun(texts, Prayer.Shacharit, nusach, jc, isTachanunDay);

            ///הלל
            ShacharitSfardTest.AddHallel(texts, jc, yomTov, nusach);

            bool hasMusaf = HebDateHelper.HasMusafToday(jc);

            if (!hasMusaf)
            {
                texts[texts.Count - 1].Add(haziKadish);
            }
            else
            {
                texts.Add(kadishShalem.Clone());
            }

            ///קריאת התורה
            bool torahReadingAdded = TestExecutor.AddTorahReading(texts, nusach, Prayer.Shacharit, jc, isTachanunDay);

            if (torahReadingAdded)
            {
                TestExecutor.AddTorahEnding(texts, nusach, Prayer.Shacharit);
            }

            ///אשרי
            texts.Add(new TextsModel(TestExecutor.GetAshrey()));

            ///למנצח, ובא לציון
            t = new TextsModel(AppResources.KdushaDesidraTitle);

            if (!hasMusaf && HebDateHelper.IsLamnatzeachDay(nusach, jc))
            {
                t.Add(Psalms.Psalm20);
            }

            t.AddRange(PrayTexts.UvaLetzion, PrayTexts.VeataKadosh1, PrayTexts.VeataKadosh2);

            texts.Add(t);

            ///קדיש שלם
            if (!hasMusaf)
            {
                texts.Add(kadishShalem.Clone());
            }

            if (hasMusaf)
            {
                texts[texts.Count - 1].Add(haziKadish);

                texts.Add(new TextsModel(TestExecutor.CreateShmoneEsreMussaf(jc, nusach).ToArray()) { Title = AppResources.MussafTitle });

                texts.Add(kadishShalem.Clone());
            }

            ///עלינו לשבח
            texts.Add(new TextsModel(AppResources.AleinuLeshabeachTitle, PrayTexts.AleinuLeshabeach));

            ///קדיש יתום
            texts.Add(kadishYatom.Clone());

            ///שיר של יום
            ShacharitSfardTest.AddDayVerse(texts, nusach, jc, isTachanunDay);

            ///קווה אל ה'
            texts.Add(new TextsModel(AppResources.PrayerEndingTitle, PrayTexts.KavehElHashem, PrayTexts.PitumHaktoret1, PrayTexts.KavehElHashemEnding));

            ///קדיש דרבנן
            texts.Add(kadishDerabanan.Clone());

            if (new DayJewishInfo(jc).ShouldSayLeDavid())
            {
                TestExecutor.AddLedavid(jc, texts);
            }

            if (isTachanunDay)
            {
                texts.Add(new TextsModel(new ParagraphModel(Psalms.Psalm49) { Title2 = AppResources.InMourningHouseTitle, IsCollapsible = true }));
            }
            else
            {
                texts.Add(new TextsModel(new ParagraphModel(Psalms.Psalm16) { Title2 = AppResources.InMourningHouseTitle, IsCollapsible = true }));
            }

            return texts.SelectMany(t => t).Select(p => p.Content);
        }

    }
}
