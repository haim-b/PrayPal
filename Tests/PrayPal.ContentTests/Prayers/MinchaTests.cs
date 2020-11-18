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
        public async Task TestMinchaSfart()
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

            texts.Add(new TextsModel(new ParagraphModel(PrayTexts.Ashrey, AppResources.AshreyTitle)));

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

            if (AddTorahReading(texts, nusach, Prayer.Mincha, jc, isTachanunDay))
            {
                AddTorahEnding(texts, nusach, Prayer.Mincha);
            }

            TextsModel shmoneEsre = new TextsModel();
            shmoneEsre.Title = AppResources.SE_Title;
            shmoneEsre.AddRange(TestExecutor.CreateShmoneEsre(Prayer.Mincha, jc, yomTov, nusach));
            texts.Add(shmoneEsre);

            if (jc.YomTovIndex == JewishCalendar.TISHA_BEAV)
            {
                //System.Windows.MessageBox.Show(AppResources.TishaBeavMessage);
            }

            AddTachanun(texts, Prayer.Mincha, nusach, jc, isTachanunDay);

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
                AddLedavid(texts, jc);
            }

            return texts.SelectMany(t => t).Select(p => p.Content);
        }

        private static void AddTachanun(List<TextsModel> texts, Prayer pray, Nusach nusach, JewishCalendar jc, bool isTachanunDay)
        {
            bool showTachanun = false;

            if (jc.IsAseretYameyTshuva())
            {
                AddAvinuMalkenu(texts, nusach, true);
                showTachanun = true;
            }
            else if ((nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz) && jc.Taanis && jc.YomTovIndex != JewishCalendar.TISHA_BEAV && !(pray == Prayer.Mincha && IsRealTaanisEsther(jc)))
            {
                AddAvinuMalkenu(texts, nusach, false);
                showTachanun = true;
            }

            showTachanun |= isTachanunDay;

            bool showSlichot = false;

            if (showTachanun)
            {
                TextsModel tachanun = new TextsModel();

                bool isShacharitAndBH = pray == Prayer.Shacharit && (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday);

                if ((nusach == Nusach.Ashkenaz && isShacharitAndBH && !showSlichot) || nusach == Nusach.Sfard || nusach == Nusach.EdotMizrach)
                {
                    tachanun.Add(new ParagraphModel(PrayTexts.Viduy1));
                    tachanun.Add(new ParagraphModel(PrayTexts.Viduy2));
                    tachanun.Add(new ParagraphModel(PrayTexts.PreYgMidot));
                    tachanun.Add(new ParagraphModel(PrayTexts.YgMidot));
                }

                if (isShacharitAndBH && nusach == Nusach.Ashkenaz)
                {
                    tachanun.AddRange(PrayTexts.TachanunBH1, PrayTexts.TachanunBH2, PrayTexts.TachanunBH3, PrayTexts.TachanunBH4, PrayTexts.TachanunBH5, PrayTexts.TachanunBH6);
                }

                tachanun.Add(new ParagraphModel(GetNefilatApayim(nusach)) { Title2 = (nusach == Nusach.Ashkenaz || nusach == Nusach.Sfard) ? AppResources.NefilatAppayimTitle : null });

                if (nusach == Nusach.EdotMizrach)
                {
                    tachanun.Add(new ParagraphModel(PrayTexts.TachanunEnding));
                }

                if (isShacharitAndBH)
                {
                    if (nusach == Nusach.EdotMizrach)
                    {
                        tachanun.AddRange(PrayTexts.ResourceManager.GetString("TachanunBHsYg"), PrayTexts.ResourceManager.GetString("TachanunBHS1"));
                        tachanun.AddRange(PrayTexts.ResourceManager.GetString("TachanunBHsYg"), PrayTexts.ResourceManager.GetString("TachanunBHS2"));
                        tachanun.AddRange(PrayTexts.ResourceManager.GetString("TachanunBHsYg"), PrayTexts.ResourceManager.GetString("TachanunBHS3"));
                        tachanun.AddRange(PrayTexts.ResourceManager.GetString("TachanunBHS4"));
                    }

                    if (nusach != Nusach.Ashkenaz)
                    {
                        tachanun.AddRange(PrayTexts.TachanunBH1, PrayTexts.TachanunBH2, PrayTexts.TachanunBH3, PrayTexts.TachanunBH4, PrayTexts.TachanunBH5, PrayTexts.TachanunBH6);
                    }

                    if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
                    {
                        tachanun.AddRange(PrayTexts.TachanunBH7, PrayTexts.TachanunBH8);
                    }
                }

                if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
                {
                    tachanun.Add(new ParagraphModel(PrayTexts.TachanunEnding));
                }

                tachanun.Title = AppResources.TachanunTitle;
                texts.Add(tachanun);
            }
            else
            {
                //texts[texts.Count - 1].Add(PrayTexts.ResourceManager.GetString("NoTachanunText"));
            }
        }

        private static void AddAvinuMalkenu(List<TextsModel> texts, Nusach nusach, bool isAYT)
        {
            TextsModel t = new TextsModel();
            t.Title = AppResources.AvinuMalkenuTitle;

            t.Add(PrayTexts.AvinuMalkenu1);

            if (nusach == Nusach.Ashkenaz || nusach == Nusach.Sfard)
            {
                if (isAYT)
                {
                    t.Add(PrayTexts.AvinuMalkenu2AYT);
                }
                else
                {
                    t.Add(PrayTexts.AvinuMalkenu2Teanit);
                }
            }

            t.Add(PrayTexts.AvinuMalkenu3);
            t.Add(PrayTexts.AvinuMalkenu4);

            texts.Add(t);
        }

        private static bool IsRealTaanisEsther(JewishCalendar jc)
        {
            return jc.JewishDayOfMonth == 13 && jc.JewishMonth == (jc.JewishLeapYear ? JewishCalendar.ADAR_II : JewishCalendar.ADAR);
        }

        private static void AddLedavid(List<TextsModel> texts, JewishCalendar jc)
        {
            TextsModel ledavid = TestExecutor.GetPsalm(27);
            texts.Add(ledavid);

            TextsModel kadishYatom = new TextsModel();
            kadishYatom.Title = AppResources.KadishYatomTitle;
            kadishYatom.AddRange(TestExecutor.CreateKadishYatom(jc));

            texts.Add(kadishYatom);
        }

        private static string GetNefilatApayim(Nusach nushach)
        {
            if (nushach == Nusach.Sfard || nushach == Nusach.Ashkenaz)
            {
                return PrayTexts.NefilatApaim;
            }
            else if (nushach == Nusach.EdotMizrach)
            {
                return Psalms.Psalm25 + PrayTexts.ResourceManager.GetString("NefilatApayimEnding");
            }

            return string.Empty;
        }

        private static void AddEdotMizrachTextsBeforeMincha(List<TextsModel> texts)
        {
            texts.Add(new TextsModel(AppResources.VersesBeforeMinchaTitle, PrayTexts.ResourceManager.GetString("LeshemYichudMincha")));
            texts.Add(TestExecutor.GetPsalm(84));//new TextsModel(string.Format(AppResources.PsalmTitle, psalmFormatter.formatHebrewNumber(84)), Psalms.Psalm84));
            texts.Add(new TextsModel(AppResources.KtoretHasamimTitle, PrayTexts.ResourceManager.GetString("KtoretHasamim1"), PrayTexts.ResourceManager.GetString("KtoretHasamim2")));
            texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.PitumHaktoret1));
            texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.PitumHaktoret2));
        }

        private static bool AddTorahReading(List<TextsModel> texts, Nusach nusach, Prayer prayer, JewishCalendar jc, bool isTachanunDay)
        {
            int yomTov = jc.YomTovIndex;

            if (prayer == Prayer.Shacharit)
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Monday
                    && DateTime.Now.DayOfWeek != DayOfWeek.Thursday
                    && !jc.Taanis
                    && !jc.RoshChodesh
                    && yomTov != JewishCalendar.CHANUKAH
                    && yomTov != JewishCalendar.PURIM
                    && yomTov != JewishCalendar.CHOL_HAMOED_SUCCOS
                    && yomTov != JewishCalendar.HOSHANA_RABBA
                    && yomTov != JewishCalendar.CHOL_HAMOED_PESACH)
                {
                    return false;
                }
            }
            else if (prayer == Prayer.Mincha)
            {
                if (yomTov != JewishCalendar.FAST_OF_GEDALYAH
                    && yomTov != JewishCalendar.FAST_OF_ESTHER
                    && yomTov != JewishCalendar.TENTH_OF_TEVES
                    && yomTov != JewishCalendar.SEVENTEEN_OF_TAMMUZ
                    && yomTov != JewishCalendar.TISHA_BEAV)
                {
                    return false;
                }
            }

            TextsModel t = new TextsModel() { Title = AppResources.TorahBookHotzaaTitle };
            texts.Add(t);

            if (prayer == Prayer.Shacharit)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                {
                    bool isInIsrael = jc.InIsrael;

                    try
                    {
                        jc.InIsrael = false;

                        if (!jc.RoshChodesh
                            && yomTov != JewishCalendar.SUCCOS
                            && yomTov != JewishCalendar.CHOL_HAMOED_SUCCOS
                            && yomTov != JewishCalendar.HOSHANA_RABBA
                            && yomTov != JewishCalendar.PESACH
                            && yomTov != JewishCalendar.CHOL_HAMOED_PESACH
                            && yomTov != JewishCalendar.CHANUKAH
                            && yomTov != JewishCalendar.PURIM
                            && yomTov != JewishCalendar.YOM_HAATZMAUT
                            && yomTov != JewishCalendar.YOM_YERUSHALAYIM
                            && yomTov != JewishCalendar.TISHA_BEAV
                            && yomTov != JewishCalendar.TU_BEAV
                            && !jc.ErevYomTov)
                        {
                            t.Add(PrayTexts.PreST);
                        }
                        else if (nusach == Nusach.EdotMizrach)
                        {
                            t.Add(PrayTexts.ResourceManager.GetString("PreSTNoTachanun"));
                        }
                    }
                    finally
                    {
                        jc.InIsrael = isInIsrael;
                    }
                }
            }

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                t.Add(PrayTexts.VeyehiBinsoa);
                t.Add(PrayTexts.BrichShmeh);
            }

            t.Add(PrayTexts.Gadlu);

            t.Add(PrayTexts.TorahBookWalking);

            if (nusach == Nusach.EdotMizrach)
            {
                t.Add(new ParagraphModel(PrayTexts.VezotHatorah) { Title2 = AppResources.HagbahaTitle });
            }

            if (prayer == Prayer.Shacharit && (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz))
            {
                t.Add(PrayTexts.Vatigaleh);
            }

            if (prayer == Prayer.Mincha)
            {
                t = new TextsModel() { Title = AppResources.TorahReadingAndHaftarahTitle };
                texts.Add(t);
            }

            t.Add(PrayTexts.BarchuTorah);
            t.Add(PrayTexts.BrachaBeforeTorah);
            t.Add(PrayTexts.BrachaAfterTorah);

            if (prayer == Prayer.Mincha)
            {
                t.Add(new ParagraphModel(AppResources.TeanitReadingCohen) { Title2 = AppResources.CohenTitle });
                t.Add(new ParagraphModel(AppResources.TeanitReadingLevi) { Title2 = AppResources.LeviTitle });
                t.Add(new ParagraphModel(AppResources.TeanitReadingIsrael) { Title2 = AppResources.IsraelTitle });

                t.Add(new ParagraphModel(PrayTexts.BeforeHaftarahBlessing) { Title2 = AppResources.HaftarahBlessingTitle });

                t.Add(new ParagraphModel(AppResources.TeanitHaftarah) { Title2 = AppResources.HaftarahTitle });
                t.Add(new ParagraphModel(PrayTexts.AfterHaftarahBlessing) { Title2 = AppResources.AfterHaftarahTitle });

            }

            ///חצי קדיש
            t.Add(TestExecutor.CreateHaziKadish(jc));

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                t.Add(new ParagraphModel(PrayTexts.VezotHatorah) { Title2 = AppResources.HagbahaVeglilahTitle });

                if (isTachanunDay && prayer == Prayer.Shacharit)
                {
                    t.Add(PrayTexts.YehiRatzonAfterTorah);
                }
            }

            return true;
        }

        private static void AddTorahEnding(List<TextsModel> texts, Nusach nusach, Prayer prayer)
        {
            TextsModel t = new TextsModel(AppResources.TorahBookReplacingTitle, PrayTexts.TorahBookReplacing1);

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                t.AddRange(Psalms.Psalm24, PrayTexts.TorahBookReplacing2);
            }

            texts.Add(t);
        }

    }
}
