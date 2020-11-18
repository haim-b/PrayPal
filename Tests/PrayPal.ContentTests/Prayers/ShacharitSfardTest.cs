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
    public class ShacharitSfardTest
    {
        [TestMethod]
        public async Task TestShacharitSfard()
        {
            TestExecutor.PrepareNusach(Nusach.Sfard);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Sfard, jc, l), (l, t) => new ShacharitSfard());
        }

        private IEnumerable<string> CreateFromLegacyDebuggedCode(Nusach nusach, JewishCalendar jc, ILocationService locationService)
        {
            List<TextsModel> texts = new List<TextsModel>(50);

            var haziKadish = TestExecutor.CreateHaziKadish(jc);
            var kadishShalem = TestExecutor.CreateKadishShalem(jc);
            var kadishYatom = TestExecutor.CreateKadishYatom(jc);
            var kadishDerabanan = TestExecutor.CreateKadishDerabanan(jc);

            int yomTov = jc.YomTovIndex;

            // סדר השכמה
            TextsModel t = new TextsModel(AppResources.SederHashkamaTitle, PrayTexts.ModeAni);

            t.Add(new ParagraphModel(AppResources.MorningNetilatYadayimInstruction + ":" + Environment.NewLine + PrayTexts.NetilatYadayimBlessing));
            t.Add(new ParagraphModel(PrayTexts.AsherYatzar) { Title2 = AppResources.AsherYatzarTitle });
            t.Add(new ParagraphModel(PrayTexts.BirkatNeshama));// { Title2 = AppResources.BirkatNeshamaTitle });

            texts.Add(t);

            // ברכות התורה
            t = new TextsModel(new ParagraphModel(PrayTexts.BirkotHatorah, AppResources.BirkotHatorahTitle));

            t.Add(new ParagraphModel(PrayTexts.ParashatBirkatCohanim));

            texts.Add(t);

            bool isTfillinTime = HebDateHelper.IsTfillinTime(jc);

            // טלית ותפילין
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



            // ברכות השחר
            t = new TextsModel(AppResources.BirkotHashacharTitle);
            t.AddRange(PrayTexts.BirkotHashachar1, PrayTexts.BirkotHashachar2, PrayTexts.BirkotHashachar3, PrayTexts.BirkotHashachar4);

            texts.Add(t);


            // ברייתא דרבי ישמעאל
            texts.AddRange(new[]{new TextsModel(new ParagraphModel(PrayTexts.BrayitaDerabiYishmael, AppResources.BrayitaDerabiYishmaelTitle)),
                kadishDerabanan });


            // תפילות השחר
            t = new TextsModel(AppResources.TfilotHashacharTitle);
            t.AddRange(PrayTexts.MizmorLifneyHaAron1, PrayTexts.MizmorLifneyHaAron2, PrayTexts.MizmorLifneyHaAron3, Psalms.Psalm30);

            t.AddRange(PrayTexts.ShacharitVerse1, Psalms.Psalm67);

            texts.Add(t);


            // פסוקי דזמרה
            t = new TextsModel(AppResources.PsukeyDezimraTitle);
            t.Add(PrayTexts.BaruchSheamar);

            if (IsMizmorLetoda(jc))
            {
                t.Add(Psalms.Psalm100);
            }

            t.AddRange(PrayTexts.ShacharitVerse2, PrayTexts.Ashrey, Psalms.Psalm146, Psalms.Psalm147, Psalms.Psalm148, Psalms.Psalm149, Psalms.Psalm150,
                Psalms.PsalmEnding, PrayTexts.ShacharitVerse3, PrayTexts.ShacharitVerse4, PrayTexts.ShacharitVerse5);

            texts.Add(t);


            // שירת הים וישתבח
            texts.Add(new TextsModel(AppResources.ShiratHayamTitle, PrayTexts.ShiratHayam));

            t = new TextsModel(AppResources.YishtabachTitle, PrayTexts.Yishtabach);

            if (jc.IsAseretYameyTshuva())
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
            AddHallel(texts, jc, yomTov, nusach);

            bool hasMusaf = HebDateHelper.HasMusafToday(jc);

            if (!hasMusaf)
            {
                texts[texts.Count - 1].Add(haziKadish);
            }
            else
            {
                texts.Add(kadishShalem.Clone());

                // שיר של יום
                AddDayVerse(texts, nusach, jc, isTachanunDay);
            }

            // קריאת התורה
            bool torahReadingAdded = TestExecutor.AddTorahReading(texts, nusach, Prayer.Shacharit, jc, isTachanunDay);

            // אשרי
            texts.Add(TestExecutor.GetAshrey());

            // למנצח, ובא לציון
            t = new TextsModel(AppResources.KdushaDesidraTitle);

            if (!hasMusaf && HebDateHelper.IsLamnatzeachDay(nusach, jc))
            {
                t.Add(Psalms.Psalm20);
            }

            t.AddRange(PrayTexts.UvaLetzion, PrayTexts.VeataKadosh1, PrayTexts.VeataKadosh2);

            texts.Add(t);

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
            }

            // שיר של יום
            if (!hasMusaf)
            {
                AddDayVerse(texts, nusach, jc, isTachanunDay);
            }

            // קווה אל ה'
            texts.Add(new TextsModel(AppResources.PrayerEndingTitle, PrayTexts.KavehElHashem, PrayTexts.PitumHaktoret1, PrayTexts.PitumHaktoret2, PrayTexts.KavehElHashemEnding));

            // קדיש דרבנן
            texts.Add(kadishDerabanan.Clone());

            // עלינו לשבח
            TextsModel aleinu = new TextsModel();
            aleinu.Title = AppResources.AleinuLeshabeachTitle;

            aleinu.Add(new ParagraphModel(PrayTexts.Barchu));

            aleinu.Add(new ParagraphModel(PrayTexts.AleinuLeshabeach));
            texts.Add(aleinu);

            texts.Add(kadishYatom.Clone());

            if (yomTov != JewishCalendar.CHOL_HAMOED_SUCCOS && yomTov != JewishCalendar.HOSHANA_RABBA // We say LeDavid after verse of day on Chol Hamoed Succot.
                && new DayJewishInfo(jc).ShouldSayLeDavid())
            {
                TestExecutor.AddLedavid(jc, texts);
            }

            return texts.SelectMany(t => t).Select(p => p.Content);
        }

        public static bool IsMizmorLetoda(JewishCalendar jc)
        {
            if (jc.JewishMonth == JewishDate.TISHREI && jc.GregorianDayOfMonth == 9)///Erev Kippur
            {
                return false;
            }

            if (jc.JewishMonth == JewishDate.NISSAN && jc.JewishDayOfMonth >= 14 && jc.JewishDayOfMonth <= 20)
            {
                return false;
            }

            return true;
        }

        internal static void AddDayVerse(List<TextsModel> texts, Nusach nusach, JewishCalendar jc, bool isTachanunDay)
        {
            //CultureInfo culture = new CultureInfo("he-IL");
            DayOfWeek day = jc.Time.DayOfWeek;

            //TextsModel t = new TextsModel(string.Format(AppResources.DayVerseTitle_F0, culture.DateTimeFormat.DayNames[(int)day]));
            TextsModel t = new TextsModel(AppResources.DayVerseTitle);

            string opening;
            string verse;

            switch (day)
            {
                case DayOfWeek.Sunday:
                    opening = PrayTexts.DayVerseDay1;
                    verse = Psalms.Psalm24;
                    break;
                case DayOfWeek.Monday:
                    opening = PrayTexts.DayVerseDay2;
                    verse = Psalms.Psalm48;
                    break;
                case DayOfWeek.Tuesday:
                    opening = PrayTexts.DayVerseDay3;
                    verse = Psalms.Psalm82;
                    break;
                case DayOfWeek.Wednesday:
                    opening = PrayTexts.DayVerseDay4;
                    verse = Psalms.Psalm94 + " " + PrayTexts.DayVerseWedEnd;
                    break;
                case DayOfWeek.Thursday:
                    opening = PrayTexts.DayVerseDay5;
                    verse = Psalms.Psalm81;
                    break;
                case DayOfWeek.Friday:
                    opening = PrayTexts.DayVerseDay6;
                    verse = Psalms.Psalm93;
                    break;
                default:
                    return;
            }

            t.AddRange(string.Format(PrayTexts.DayVerseOpening_F0, opening), verse);

            if (nusach != Nusach.Ashkenaz)
            {
                t.Add(PrayTexts.DayVerseEnd);
            }

            if (nusach == Nusach.EdotMizrach)
            {
                int yomTov = jc.YomTovIndex;

                if (yomTov == JewishCalendar.FAST_OF_GEDALYAH || yomTov == JewishCalendar.TENTH_OF_TEVES)
                {
                    t.Add(Psalms.Psalm83);
                }
                else if (yomTov == JewishCalendar.CHANUKAH)
                {
                    t.Add(Psalms.Psalm30);
                }
                else if (yomTov == JewishCalendar.FAST_OF_ESTHER || yomTov == JewishCalendar.PURIM)
                {
                    t.Add(Psalms.Psalm22);
                }
                else if (yomTov == JewishCalendar.SEVENTEEN_OF_TAMMUZ)
                {
                    t.Add(Psalms.Psalm79);
                }
                else if (HebDateHelper.IsAfterYomKippur())
                {
                    t.Add(Psalms.Psalm85);
                }

                t.Add(new ParagraphModel(Psalms.Psalm49) { Title2 = AppResources.InMoarningHouseTitle, IsCollapsible = true });

                t.AddRange(TestExecutor.CreateKadishYatom(jc, true));
            }
            else if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                if (nusach == Nusach.Sfard && jc.RoshChodesh)
                {
                    t.AddRange(TestExecutor.CreateKadishYatom(jc, true));
                    t.Add(new ParagraphModel(Psalms.Psalm104) { Title2 = AppResources.BarchiNafshiTitle });
                }

                t.AddRange(TestExecutor.CreateKadishYatom(jc, true));

                if (nusach == Nusach.Sfard)
                {
                    int yomTov = jc.YomTovIndex;

                    if (yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA)
                    {
                        t.Add(Psalms.Psalm27);
                        t.AddRange(TestExecutor.CreateKadishYatom(jc, true));
                    }

                    if (isTachanunDay)
                    {
                        t.Add(new ParagraphModel(Psalms.Psalm49) { Title2 = AppResources.InMoarningHouseTitle, IsCollapsible = true });
                    }
                    else
                    {
                        t.Add(new ParagraphModel(Psalms.Psalm16) { Title2 = AppResources.InMoarningHouseTitle, IsCollapsible = true });
                    }
                }
            }

            texts.Add(t);
        }

        internal static void AddHallel(List<TextsModel> texts, JewishCalendar jc, int yomTov, Nusach nusach)
        {
            ///0=no hallel, 1=partial hallel, 2=full hallel
            int hallelMode = 0;

            if (yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA || yomTov == JewishCalendar.CHANUKAH || yomTov == JewishCalendar.YOM_HAATZMAUT || yomTov == JewishCalendar.YOM_YERUSHALAYIM)
            {
                hallelMode = 2;
            }
            else if (jc.RoshChodesh || yomTov == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                hallelMode = 1;
            }

            if (hallelMode == 0)
            {
                return;
            }

            TextsModel t = new TextsModel();
            t.Title = AppResources.HallelTitle;

            if (hallelMode == 2 || nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                if (!(yomTov == JewishCalendar.YOM_HAATZMAUT && nusach == Nusach.EdotMizrach)) //Edot Hamizrach don't bless on full Hallel of Yom Haazmaut.
                {
                    t.Add(PrayTexts.HallelBlessing);
                }
            }

            t.Add(Psalms.Psalm113);
            t.Add(Psalms.Psalm114);

            if (hallelMode == 2)
            {
                t.Add(Psalms.Psalm115.Substring(0, 656));
            }

            t.Add(Psalms.Psalm115.Substring(657));

            if (hallelMode == 2)
            {
                t.Add(Psalms.Psalm116.Substring(0, 621));
            }

            t.Add(Psalms.Psalm116.Substring(622));

            t.Add(Psalms.Psalm117);

            t.Add(Psalms.Psalm118.Substring(0, 1032));

            t.AddRange(PrayTexts.HallelEnding1, PrayTexts.HallelEnding2, PrayTexts.HallelEnding3);

            if (hallelMode == 2 || nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                t.Add(PrayTexts.HallelEnding4);
            }

            texts.Add(t);
        }

    }
}
