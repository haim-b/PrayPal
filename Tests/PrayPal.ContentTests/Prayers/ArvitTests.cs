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
    public class ArvitTests
    {
        [TestMethod]
        public async Task TestArvitSfart()
        {
            TestExecutor.PrepareNusach(Nusach.Sfard);
            await TestExecutor.TestPrayerAsync((jc, l, t) => CreateFromLegacyDebuggedCode(Nusach.Sfard, jc, l), (l, t) => new ArvitSfard(l));
        }

        private IEnumerable<string> CreateFromLegacyDebuggedCode(Nusach nusach, JewishCalendar jc, ILocationService locationService)
        {
            List<TextsModel> texts = new List<TextsModel>();
            //item.Texts.Add(new TextsModel(){Title=});
            var haziKadish = TestExecutor.CreateHaziKadish(jc);
            var kadishShalem = TestExecutor.CreateKadishShalem(jc);
            var kadishYatom = TestExecutor.CreateKadishYatom(jc);

            int yomTov = jc.YomTovIndex;

            if (yomTov == -1 && jc.RoshChodesh)
            {
                yomTov = JewishCalendar.ROSH_CHODESH;
            }

            if (yomTov == JewishCalendar.TISHA_BEAV)
            {
                //System.Windows.MessageBox.Show(AppResources.TishaBeavMessage);
            }

            if ((nusach == Nusach.EdotMizrach || nusach == Nusach.Baladi) && jc.RoshChodesh)
            {
                texts.Add(new TextsModel(AppResources.BarchiNafshiTitle, Psalms.Psalm104));
            }

            if (nusach == Nusach.EdotMizrach)
            {
                texts.Add(new TextsModel(AppResources.VersesBeforeArvitTitle, PrayTexts.ResourceManager.GetString("LeshemYichudArvit")));
            }

            if (yomTov == JewishCalendar.YOM_HAATZMAUT)
            {
                AddYomHaazmautForArvit(texts, nusach);
            }
            else
            {
                JewishCalendar jc2 = jc.CloneEx();
                jc2.forward();

                if (jc2.YomTovIndex == JewishCalendar.YOM_HAATZMAUT && DateTime.Now.Hour > 12)
                {
                    AddYomHaazmautForArvit(texts, nusach);
                }
            }

            if (nusach != Nusach.Ashkenaz && (nusach == Nusach.EdotMizrach || jc.Time.DayOfWeek != DayOfWeek.Sunday))
            {
                TextsModel t;

                if (nusach == Nusach.EdotMizrach)
                {
                    t = texts[texts.Count - 1];
                }
                else
                {
                    t = new TextsModel(AppResources.VersesBeforeArvitTitle);
                    texts.Add(t);
                }

                t.AddRange(new ParagraphModel(PrayTexts.VersesBeforeArvit), haziKadish);
            }

            TextsModel shma = new TextsModel();
            shma.Title = AppResources.KriatShmaAndBlessingTitle;
            shma.Add(new ParagraphModel(PrayTexts.VehuRachum));
            shma.Add(new ParagraphModel(PrayTexts.Barchu));
            shma.Add(new ParagraphModel(PrayTexts.BrachotBeforeShmaArvit));
            shma.Add(new ParagraphModel(PrayTexts.KriatShma1));
            shma.Add(new ParagraphModel(PrayTexts.KriatShma2));
            shma.Add(new ParagraphModel(PrayTexts.KriatShma3));
            shma.Add(new ParagraphModel(PrayTexts.BrachotAfterShmaArvit1));
            shma.Add(new ParagraphModel(PrayTexts.BrachotAfterShmaArvit2));

            if (nusach == Nusach.Sfard && !Settings.IsInIsrael)
            {
                shma.Add(PrayTexts.InArvitInChul);
            }

            shma.Add(haziKadish);

            texts.Add(shma);

            TextsModel shmoneEsre = new TextsModel();
            shmoneEsre.Title = AppResources.SE_Title;
            shmoneEsre.AddRange(TestExecutor.CreateShmoneEsre(Prayer.Arvit, jc, yomTov, nusach));
            texts.Add(shmoneEsre);

            if (IsVeyehiNoam(jc.Time))
            {
                texts[texts.Count - 1].Add(haziKadish);
                texts.Add(new TextsModel(AppResources.ForMotzashTitle, PrayTexts.VeyehiNoam, PrayTexts.VeataKadosh1, PrayTexts.VeataKadosh2));
            }

            texts.Add(kadishShalem);

            if (nusach != Nusach.Ashkenaz)
            {
                texts.Add(new TextsModel(AppResources.Psalm121Title, Psalms.Psalm121));

                texts.Add(kadishYatom);
            }

            int omer = jc.DayOfOmer;

            TextsModel aleinu = new TextsModel();
            aleinu.Title = AppResources.AleinuLeshabeachTitle;

            if (omer == -1)
            {
                if (nusach != Nusach.Ashkenaz)
                {
                    aleinu.Add(new ParagraphModel(PrayTexts.Barchu));
                }
            }
            else if (nusach != Nusach.EdotMizrach)
            {
                AddSfiratHaOmer(texts, omer, nusach);
            }

            aleinu.Add(new ParagraphModel(PrayTexts.AleinuLeshabeach));
            texts.Add(aleinu);

            if (omer > 0 && nusach == Nusach.EdotMizrach)
            {
                AddSfiratHaOmer(texts, omer, nusach);
            }

            if (nusach == Nusach.Ashkenaz)
            {
                texts.Add(kadishYatom);
                texts[texts.Count - 1].Add(new ParagraphModel(PrayTexts.Barchu));

                if (new DayJewishInfo(jc).ShouldSayLeDavid())
                {
                    TestExecutor.AddLedavid(jc, texts);
                }
            }

            return texts.SelectMany(t => t).Select(p => p.Content);
        }

        private static void AddYomHaazmautForArvit(List<TextsModel> texts, Nusach nusach)
        {
            TextsModel hodaa = new TextsModel(AppResources.YomHaazmautARvitHodaaTitle);
            hodaa.Add(new ParagraphModel(Psalms.Psalm107) { Title2 = string.Format(AppResources.PsalmTitle, TestExecutor.PsalmFormatter.formatHebrewNumber(107)) });
            hodaa.Add(new ParagraphModel(Psalms.Psalm97) { Title2 = string.Format(AppResources.PsalmTitle, TestExecutor.PsalmFormatter.formatHebrewNumber(97)) });
            hodaa.Add(new ParagraphModel(Psalms.Psalm98) { Title2 = string.Format(AppResources.PsalmTitle, TestExecutor.PsalmFormatter.formatHebrewNumber(98)) });
        }

        private static void AddSfiratHaOmer(List<TextsModel> texts, int omer, Nusach nusach)
        {
            if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 23)
            {
                //if (!Settings.Instance.UseLocation)
                //{
                //    System.Windows.MessageBox.Show("האפליקציה לא מורשית להשתמש במיקום. ספירת העומר תתעדכן רק ב-12 בלילה.");
                //}
                //else if (LocationManager.GetPosition() == null)
                //{
                //    System.Windows.MessageBox.Show("האפליקציה לא הצליחה לאתר את המיקום. ספירת העומר תתעדכן רק ב-12 בלילה.");
                //}
            }

            TextsModel t = new TextsModel(AppResources.OmerCountTitle);

            if (nusach != Nusach.Ashkenaz)
            {
                t.Add(new ParagraphModel(PrayTexts.Barchu));
            }

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                t.Add(PrayTexts.PreSfiratHaHomer);
            }
            else if (nusach == Nusach.EdotMizrach)
            {
                t.Add(PrayTexts.ResourceManager.GetString("PreSfiratHaOmer1"));
                //t.Add(string.Format(PrayTexts.ResourceManager.GetString("PreSfiratHaOmer2_F0"), omer == 49 ? string.Empty : PrayTexts.ResourceManager.GetString("PreSfiratHaOmer2_MostDays")));
                //t.Add(PrayTexts.ResourceManager.GetString("PreSfiratHaOmer3"));
                //t.Add(PrayTexts.ResourceManager.GetString("PreSfiratHaOmer4"));
                //t.Add(PrayTexts.ResourceManager.GetString("PreSfiratHaOmer5"));
                //t.Add(PrayTexts.ResourceManager.GetString("PreSfiratHaOmer6"));
            }

            t.Add(PrayTexts.SfiratHaOmerBlessing);

            string[] days = omer == 1 ? null : PrayTexts.SfiratHaOmer2To49.Split('|');

            string omerDay = omer > 1 ? days[omer - 2] : null; ///omer -2 because we subtract 1 for having zero-index and another 1 because our array starts from 2 and not 1.
            string omerCount;

            if (omer == 1)
            {
                omerCount = PrayTexts.SfiratHaOmer1;
            }
            else if (omer <= 6)
            {
                omerCount = string.Format(PrayTexts.SfiratHaOmerShort_F0, omerDay);
            }
            else
            {
                string omerInWeeks = PrayTexts.SfiratHaOmerWeeks.Split('|')[(omer / 7) - 1];

                int omerDaysMod = (omer % 7) - 1;

                if (omerDaysMod >= 0)
                {
                    omerInWeeks += PrayTexts.SfiratHaOmerDays.Split('|')[omerDaysMod];
                }

                string omerFormat = omer <= 10 ? PrayTexts.SfiratHaOmerLong2_F1 : PrayTexts.SfiratHaOmerLong_F1;

                omerCount = string.Format(omerFormat, omerDay, omerInWeeks);
            }

            t.Add(omerCount);

            t.Add(PrayTexts.AfterSfiratHaOmer1);

            t.Add(Psalms.Psalm67);

            //if (nusach != Nusach.Ashkenaz)
            {
                t.Add(PrayTexts.AnnaBechoach);
            }

            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                string dayKabbalahDetail1 = PrayTexts.SfiratHaOmerDaysKabbalah1.Split('|')[(omer - 1) % 7];
                string dayKabbalahDetail2 = PrayTexts.SfiratHaOmerDaysKabbalah2.Split('|')[(omer - 1) / 7];
                t.Add(string.Format(PrayTexts.AfterSfiratHaOmer2_F0, dayKabbalahDetail1, dayKabbalahDetail2));
            }

            texts.Add(t);
        }

        public static bool IsVeyehiNoam(DateTime now)
        {
            if (now.DayOfWeek == DayOfWeek.Sunday)
            {
                for (int i = 1; i <= 5; i++)
                {
                    now += TimeSpan.FromDays(1);

                    //Moadim moed = GetMoed(now);

                    //if (moed.HasFlag(Moadim.Pesach) || moed.HasFlag(Moadim.Shavuot) || moed.HasFlag(Moadim.Sukkot))
                    //{
                    //    return false;
                    //}

                    Zmanim.HebrewCalendar.JewishCalendar c = new Zmanim.HebrewCalendar.JewishCalendar(now) { InIsrael = true };

                    int yomTov = c.YomTovIndex;

                    switch (yomTov)
                    {
                        case JewishCalendar.ROSH_HASHANA:
                        case JewishCalendar.YOM_KIPPUR:
                        case JewishCalendar.PESACH:
                        case JewishCalendar.SHAVUOS:
                        case JewishCalendar.SUCCOS:
                        case JewishCalendar.CHOL_HAMOED_PESACH:
                        case JewishCalendar.CHOL_HAMOED_SUCCOS:
                        case JewishCalendar.HOSHANA_RABBA:
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

    }
}
