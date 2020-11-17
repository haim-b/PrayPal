﻿using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Content;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tests.PrayPal.Content.LegacyModels;
using Tests.PrayPal.Content.LegacyTextProviders;
using Zmanim;
using Zmanim.HebrewCalendar;

namespace Tests.PrayPal.Content
{
    static class TestExecutor
    {
        public static readonly HebrewDateFormatter PsalmFormatter = new HebrewDateFormatter() { UseGershGershayim = false };
        public static bool IsInIsrael = true;
        public static bool ShowVeanenu = false;

        public static async Task TestPrayerAsync(Func<JewishCalendar, ILocationService, ITimeService, IEnumerable<string>> legacyPrayerFactory, Func<ILocationService, ITimeService, IPrayer> newPrayerFactory, DateTime? fromTime = null, DateTime? toTime = null)
        {
            StubLocationService locationService = new StubLocationService();
            var realTimeService = new TimeService(locationService);
            StubTimeService timeService = new StubTimeService(realTimeService);
            var logger = new DummyLogger();

            DateTime startDate = fromTime ?? new DateTime(2020, 1, 1);
            DateTime endDate = toTime ?? new DateTime(2020, 12, 31);

            for (DateTime now = startDate; now <= endDate; now = now.AddDays(1))
            {
                timeService.SetFakeTime(now);

                DayJewishInfo dayJewishInfo = await timeService.GetDayInfoAsync(await locationService.GetCurrentPositionAsync(), now, true);
                IEnumerable<string> legacyText = legacyPrayerFactory(dayJewishInfo.JewishCalendar, locationService, timeService);

                var t = newPrayerFactory(locationService, timeService);
                await t.CreateAsync(await timeService.GetDayInfoAsync(), logger);
                IEnumerable<string> newText = Enumerable.Range(0, t.GetItemsCount()).Select(i => t.GetItemAtIndex(i)).Select(p => p.Content).Where(c => c != null).SelectMany(c => c).Select(r => r.Text);

                //CollectionAssert.AreEqual(legacyText.ToList(), newText.ToList(), "Mismatching paragraph on date '{0}'.", now);
                //Assert.AreEqual(string.Join(Environment.NewLine, legacyText), string.Join(Environment.NewLine, newText), "Mismatching paragraph on date '{0}'.", now);

                string[] legacy = string.Join(" ", legacyText).Replace(Environment.NewLine, " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] newStrings = string.Join(" ", newText).Replace(Environment.NewLine, " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < legacy.Length; i++)
                {
                    Assert.AreEqual(legacy[i], newStrings[i], "Mismatching word '{0}' on date '{1}'.", legacy[i], now);//, string.Join(" ", Enumerable.Range(i - 5, 5).Select(j => legacy[j])));
                }
            }
        }

        internal static void PrepareNusach(Nusach nusach)
        {
            SetPraysResources(nusach);
            PrayersHelper.SetPrayerTextProvider(nusach);
        }

        public static void SetPraysResources(Nusach nusach)
        {
            string ns = typeof(PrayTexts).Namespace;

            if (nusach == Nusach.EdotMizrach)
            {
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(ns + ".PrayTexts.EdotMizrach", typeof(PrayTexts).Assembly);
                typeof(PrayTexts).GetField("resourceMan", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, temp);
            }
            else if (nusach == Nusach.Ashkenaz)
            {
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(ns + ".PrayTexts.Ashkenaz", typeof(PrayTexts).Assembly);
                typeof(PrayTexts).GetField("resourceMan", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, temp);
            }
            else
            {
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(ns + ".PrayTexts", typeof(PrayTexts).Assembly);
                typeof(PrayTexts).GetField("resourceMan", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, temp);
            }

            //RefreshKadishim(HebDateHelper.GetCalendarToday());

            //ItemViewModel arvitItem;

            //if (_tfilotBrachot.TryGetValue("Arvit", out arvitItem))
            //{
            //    SetArvitTitle(arvitItem);
            //}
        }

        public static ParagraphModel GetOseShalom(JewishCalendar jc)
        {
            return new ParagraphModel(string.Format(PrayTexts.OseShalom, jc.IsAseretYameyTshuva() ? PrayTexts.OseShalomAseretYamim : PrayTexts.OseShalomRegular));
        }

        public static ParagraphModel GetYaalehVeYavo(JewishCalendar jc)
        {
            if (jc.RoshChodesh)
            {
                return new ParagraphModel(string.Format(PrayTexts.YaalehVeyavo, PrayTexts.YaalehVeyavoRoshHodesh)) { Title2 = AppResources.YaalehVeyavoRoshHodeshTitle };
            }

            int yomTov = jc.YomTovIndex;

            if (yomTov == JewishCalendar.SUCCOS || yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA)
            {
                return new ParagraphModel(string.Format(PrayTexts.YaalehVeyavo, PrayTexts.YaalehVeyavoSukkot)) { Title2 = AppResources.YaalehVeyavoSukkotTitle };
            }

            if (yomTov == JewishCalendar.PESACH || yomTov == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                return new ParagraphModel(string.Format(PrayTexts.YaalehVeyavo, PrayTexts.YaalehVeyavoPesach)) { Title2 = AppResources.YaalehVeyavoPesachTitle };
            }

            return null;
        }

        public static ParagraphModel CreateHaziKadish(JewishCalendar jc)
        {
            return new ParagraphModel(string.Format(PrayTexts.HaziKadish, jc.IsAseretYameyTshuva() ? PrayTexts.KadishLeelaAYT : PrayTexts.KadishLeelaRegular)) { IsCollapsible = true, Title2 = AppResources.HaziKadishTitle };
        }

        public static TextsModel CreateKadishShalem(JewishCalendar jc)
        {
            IEnumerable<ParagraphModel> Create()
            {
                yield return new ParagraphModel(string.Format(PrayTexts.KadishShalem, jc.IsAseretYameyTshuva() ? PrayTexts.KadishLeelaAYT : PrayTexts.KadishLeelaRegular), AppResources.KadishShalemTitle);

                yield return GetOseShalom(jc);
            }

            return new TextsModel(Create().ToArray());
        }

        public static TextsModel CreateKadishYatom(JewishCalendar jc, bool addSecondaryTitle = false)
        {
            IEnumerable<ParagraphModel> Create()
            {
                yield return new ParagraphModel(string.Format(PrayTexts.KadishYatom, jc.IsAseretYameyTshuva() ? PrayTexts.KadishLeelaAYT : PrayTexts.KadishLeelaRegular), AppResources.KadishYatomTitle) { Title2 = addSecondaryTitle ? AppResources.KadishYatomTitle : null };

                yield return GetOseShalom(jc);
            }

            return new TextsModel(Create().ToArray());
        }

        public static TextsModel CreateKadishDerabanan(JewishCalendar jc)
        {
            IEnumerable<ParagraphModel> Create()
            {
                yield return new ParagraphModel(string.Format(PrayTexts.KadishDerabanan, jc.IsAseretYameyTshuva() ? PrayTexts.KadishLeelaAYT : PrayTexts.KadishLeelaRegular), AppResources.KadishDerabananTitle);

                yield return GetOseShalom(jc);
            }

            return new TextsModel(Create().ToArray());
        }

        public static void AddLedavid(JewishCalendar jc, List<TextsModel> texts)
        {
            TextsModel ledavid = GetPsalm(27);
            texts.Add(ledavid);

            TextsModel kadishYatom = new TextsModel();
            kadishYatom.Title = AppResources.KadishYatomTitle;
            kadishYatom.AddRange(CreateKadishYatom(jc));

            texts.Add(kadishYatom);
        }

        public static TextsModel GetPsalm(int number)
        {
            if (number <= 0 || number > 150)
            {
                throw new ArgumentOutOfRangeException("Number " + number + " is not valid for psalm verse.");
            }

            return new TextsModel(string.Format(AppResources.PsalmTitle, PsalmFormatter.formatHebrewNumber(number)), Psalms.ResourceManager.GetString("Psalm" + number));
        }

        public static IEnumerable<ParagraphModel> CreateShmoneEsre(Prayer prayType, JewishCalendar jc, int yomTov, Nusach nusach)
        {
            bool isTeanit = jc.Taanis;
            bool isAYT = jc.IsAseretYameyTshuva();

            if (prayType == Prayer.Mincha && (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz))
            {
                yield return new ParagraphModel(PrayTexts.KiShemHashem);
            }

            yield return new ParagraphModel(PrayTexts.SfatayTiftach);

            yield return new ParagraphModel(string.Format(PrayTexts.SE01, isAYT ? PrayTexts.SE01AYT : string.Empty));

            string SE2;

            if (HebDateHelper.IsMoridHatal())
            {
                SE2 = string.Format(PrayTexts.SE02, PrayTexts.SE02Summer, "{0}");
            }
            else
            {
                SE2 = string.Format(PrayTexts.SE02, PrayTexts.SE02Winter, "{0}");
            }

            yield return new ParagraphModel(string.Format(SE2, isAYT ? PrayTexts.SE02AYT : string.Empty));

            if (prayType != Prayer.Arvit)
            {
                if (nusach == Nusach.Ashkenaz)
                {
                    yield return new ParagraphModel(string.Format(PrayTexts.Kdusha, isAYT ? PrayTexts.SE03Hamelech : PrayTexts.SE03Hael)) { Title2 = AppResources.KdushaTitle, IsCollapsible = true };
                    yield return new ParagraphModel(string.Format(PrayTexts.SE03, isAYT ? PrayTexts.SE03Hamelech : PrayTexts.SE03Hael)) { Title2 = AppResources.InTfilatYachidTitle };
                }
                else
                {
                    yield return new ParagraphModel(PrayTexts.Kdusha) { Title2 = AppResources.KdushaTitle, IsCollapsible = true };
                }
            }

            if (nusach != Nusach.Ashkenaz || prayType == Prayer.Arvit)
            {
                yield return new ParagraphModel(string.Format(PrayTexts.SE03, isAYT ? PrayTexts.SE03Hamelech : PrayTexts.SE03Hael));
            }

            string SE4 = string.Empty;

            if (prayType == Prayer.Arvit && HebDateHelper.ShowAttaChonantanu(jc))
            {
                SE4 = PrayTexts.SE04Havdalah;
            }

            yield return new ParagraphModel(string.Format(PrayTexts.SE04, SE4));

            yield return new ParagraphModel(PrayTexts.SE05);
            yield return new ParagraphModel(PrayTexts.SE06);
            yield return new ParagraphModel(PrayTexts.SE07);

            if ((prayType == Prayer.Mincha || prayType == Prayer.Shacharit) && isTeanit)
            {
                yield return new ParagraphModel(string.Format(PrayTexts.Anenu, PrayTexts.AnenuEnding)) { Title2 = AppResources.InHazarahTitle };
            }

            yield return new ParagraphModel(PrayTexts.SE08);

            string SE9;

            if (IsVetenBracha(jc))
            {
                SE9 = string.Format(PrayTexts.SE09, PrayTexts.SE09Summer);
            }
            else
            {
                SE9 = string.Format(PrayTexts.SE09, PrayTexts.SE09Winter);
            }

            yield return new ParagraphModel(SE9);

            yield return new ParagraphModel(PrayTexts.SE10);

            yield return new ParagraphModel(string.Format(PrayTexts.SE11, isAYT ? PrayTexts.SE11Aseret : PrayTexts.SE11Regular));

            yield return new ParagraphModel(PrayTexts.SE12);
            yield return new ParagraphModel(PrayTexts.SE13);

            yield return new ParagraphModel(string.Format(PrayTexts.SE14, yomTov == JewishCalendar.TISHA_BEAV ? PrayTexts.Nachem : PrayTexts.SE14B));

            yield return new ParagraphModel(PrayTexts.SE15);

            if (isTeanit)
            {
                yield return new ParagraphModel(PrayTexts.SE16);

                if (prayType == Prayer.Mincha)
                {
                    yield return new ParagraphModel(string.Format(PrayTexts.Anenu, string.Empty)) { Title2 = AppResources.InTfilatYachidTitle };
                }

                if ((nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz) && ShowVeanenu)
                {
                    yield return new ParagraphModel(PrayTexts.VaAnenu) { Title2 = AppResources.VeAnenuTitle };
                }

                yield return new ParagraphModel(PrayTexts.SE16B);
            }
            else
            {
                if ((nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz) && ShowVeanenu)
                {
                    yield return new ParagraphModel(PrayTexts.SE16);

                    if ((nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz) && ShowVeanenu)
                    {
                        yield return new ParagraphModel(PrayTexts.VaAnenu) { Title2 = AppResources.VeAnenuTitle };
                    }

                    yield return new ParagraphModel(PrayTexts.SE16B);
                }
                else
                {
                    yield return new ParagraphModel(string.Concat(PrayTexts.SE16, " ", PrayTexts.SE16B));
                }
            }

            yield return new ParagraphModel(PrayTexts.SE17);

            ParagraphModel yaalehVeYavo = GetYaalehVeYavo(jc);

            if (yaalehVeYavo != null)
            {
                yield return yaalehVeYavo;
            }

            yield return new ParagraphModel(PrayTexts.SE17B);

            yield return new ParagraphModel(PrayTexts.Modim);

            if (prayType != Prayer.Arvit)
            {
                yield return new ParagraphModel(PrayTexts.ModimDeRabanan) { Title2 = AppResources.ModimDeRabananTitle };
            }

            if (yomTov == JewishCalendar.CHANUKAH)
            {
                yield return new ParagraphModel(PrayTexts.AlHanissimHannukah) { Title2 = AppResources.AlHanissimHannukahTitle };
            }
            else if (yomTov == JewishCalendar.PURIM)
            {
                yield return new ParagraphModel(PrayTexts.AlHanissimPurim) { Title2 = AppResources.AlHanissimPurimTitle };
            }

            yield return new ParagraphModel(string.Format(PrayTexts.SE18, isAYT ? PrayTexts.SE18AYT : string.Empty));

            if (prayType == Prayer.Shacharit || prayType == Prayer.Mussaf || (prayType == Prayer.Mincha && isTeanit && jc.YomTovIndex != JewishCalendar.TISHA_BEAV))
            {
                ///Birkat Cohanim:
                string title = AppResources.BirkatCohanimTitle;

                if (prayType == Prayer.Mincha && isTeanit)
                {
                    title = AppResources.BirkatCohanimInTeanitTitle;
                }

                yield return new ParagraphModel(PrayTexts.BirkatCohanim) { Title2 = title, IsCollapsible = true };
            }

            if (nusach == Nusach.Ashkenaz && !isTeanit && (prayType == Prayer.Mincha || prayType == Prayer.Arvit))
            {
                yield return new ParagraphModel(string.Format(PrayTexts.ResourceManager.GetString("SE19_ShalomRav"), isAYT ? PrayTexts.SE19AYT : string.Empty));
            }
            else
            {
                yield return new ParagraphModel(string.Format(PrayTexts.SE19, isAYT ? PrayTexts.SE19AYT : string.Empty));
            }

            yield return new ParagraphModel(PrayTexts.Yhiu);
            yield return new ParagraphModel(PrayTexts.ElokayNezor);

            yield return GetOseShalom(jc);

            ParagraphModel p = new ParagraphModel(PrayTexts.SE_End);

            if (nusach == Nusach.EdotMizrach)
            {
                p.Title = PrayTexts.ResourceManager.GetString("AfterHazarahTitle");
            }

            yield return p;
        }

        private static bool IsVetenBracha(this JewishCalendar jc)
        {
            if (jc.JewishMonth == 1) //Nissan
            {
                if (jc.JewishDayOfMonth < 15) //Pesach 1st
                {
                    return false;
                }

                return true;
            }
            else if (jc.JewishMonth < 8) //Between Nissan and Heshvan
            {
                return true;
            }
            else if (jc.JewishMonth == 8) //Heshvan
            {
                if (IsInIsrael)
                {
                    return jc.JewishDayOfMonth < 7; //ז' חשוון
                }
                else
                {
                    return true;
                }
            }
            else if (!IsInIsrael)
            {
                DateTime now = DateTime.Now;

                if (now.Month >= 4 && now.Month < 12)
                {
                    return true;
                }

                // Abroad, they start to ask for rain since December 5th on a regular year, and December 6th on a leap year:
                return now.Day < 5 + DateTime.DaysInMonth(now.Year, 2) - 28;
            }

            return false;
        }

        public static IEnumerable<ParagraphModel> CreateShmoneEsreMussaf(JewishCalendar jc, Nusach nusach)
        {
            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                yield return new ParagraphModel(PrayTexts.KiShemHashem);
            }

            int yomTovIndex = jc.YomTovIndex;

            yield return new ParagraphModel(PrayTexts.SfatayTiftach);

            yield return new ParagraphModel(string.Format(PrayTexts.SE01, string.Empty));

            string SE2;

            if (HebDateHelper.IsMoridHatal())
            {
                SE2 = string.Format(PrayTexts.SE02, PrayTexts.SE02Summer, "");
            }
            else
            {
                SE2 = string.Format(PrayTexts.SE02, PrayTexts.SE02Winter, "");
            }

            yield return new ParagraphModel(SE2);

            //UNDONE: In Hoshaana Raba put Kdushat Keter of Shabbat.
            if (nusach == Nusach.Sfard || nusach == Nusach.Ashkenaz)
            {
                yield return new ParagraphModel(PrayTexts.KdushatKeter) { Title2 = AppResources.KdushaTitle, IsCollapsible = true };

                yield return new ParagraphModel(string.Format(PrayTexts.SE03, PrayTexts.SE03Hael)) { Title2 = AppResources.InTfilatYachidTitle };
            }
            else
            {
                yield return new ParagraphModel(PrayTexts.KdushatKeter) { Title2 = AppResources.KdushaTitle, IsCollapsible = true };

                yield return new ParagraphModel(string.Format(PrayTexts.SE03, PrayTexts.SE03Hael));
            }

            if (jc.RoshChodesh)
            {
                yield return new ParagraphModel(PrayTexts.MussafRoshChodeshSE1);
                yield return new ParagraphModel(string.Format(PrayTexts.MussafRoshChodeshSE2, HebDateHelper.IsIbburTime(jc) ? PrayTexts.MussafRoshChodeshInIbbur : string.Empty));
            }
            else
            {
                string holidayText1 = null;
                string holidayText2 = null;
                string holidayPsukim = null;

                if (yomTovIndex == JewishCalendar.CHOL_HAMOED_PESACH)
                {
                    holidayText1 = PrayTexts.MussafPesach1;
                    holidayText2 = PrayTexts.YaalehVeyavoPesach;
                    holidayPsukim = PrayTexts.MussafPesachPsukim1;
                }
                else if (yomTovIndex == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTovIndex == JewishCalendar.HOSHANA_RABBA)
                {
                    holidayText1 = PrayTexts.MussafSukkot1;
                    holidayText2 = PrayTexts.YaalehVeyavoSukkot;

                    if (nusach != Nusach.EdotMizrach)
                    {
                        switch (jc.JewishDayOfMonth)
                        {
                            case 16:
                                holidayPsukim = PrayTexts.MussafSukkotPsukim1;
                                break;
                            case 17:
                                holidayPsukim = jc.InIsrael ? PrayTexts.MussafSukkotPsukim2 : PrayTexts.MussafSukkotPsukim1;
                                break;
                            case 18:
                                holidayPsukim = jc.InIsrael ? PrayTexts.MussafSukkotPsukim3 : PrayTexts.MussafSukkotPsukim2;
                                break;
                            case 19:
                                holidayPsukim = jc.InIsrael ? PrayTexts.MussafSukkotPsukim4 : PrayTexts.MussafSukkotPsukim3;
                                break;
                            case 20:
                                holidayPsukim = jc.InIsrael ? PrayTexts.MussafSukkotPsukim5 : PrayTexts.MussafSukkotPsukim4;
                                break;
                            case 21:
                                holidayPsukim = jc.InIsrael ? PrayTexts.MussafSukkotPsukim6 : PrayTexts.MussafSukkotPsukim5;
                                break;
                            default:
                                break;
                        }
                    }
                }

                yield return new ParagraphModel(string.Format(PrayTexts.MusasfCholHamoed1, holidayText1));
                yield return new ParagraphModel(string.Format(PrayTexts.MusasfCholHamoed2, holidayText2));

                if (nusach != Nusach.EdotMizrach)
                {
                    yield return new ParagraphModel(holidayPsukim);
                }

                yield return new ParagraphModel(PrayTexts.MusasfCholHamoed3);
                yield return new ParagraphModel(PrayTexts.MusasfCholHamoed4);
            }

            yield return new ParagraphModel(PrayTexts.SE17);

            yield return new ParagraphModel(PrayTexts.SE17B);

            yield return new ParagraphModel(PrayTexts.Modim);

            yield return new ParagraphModel(PrayTexts.ModimDeRabanan) { Title2 = AppResources.ModimDeRabananTitle };

            if (yomTovIndex == JewishCalendar.CHANUKAH)
            {
                yield return new ParagraphModel(PrayTexts.AlHanissimHannukah) { Title2 = AppResources.AlHanissimHannukahTitle };
            }

            yield return new ParagraphModel(string.Format(PrayTexts.SE18, string.Empty));

            ///Birkat Cohanim:
            string title = AppResources.BirkatCohanimTitle;

            yield return new ParagraphModel(PrayTexts.BirkatCohanim) { Title2 = title, IsCollapsible = true };

            yield return new ParagraphModel(string.Format(PrayTexts.SE19, string.Empty));

            yield return new ParagraphModel(PrayTexts.Yhiu);
            yield return new ParagraphModel(PrayTexts.ElokayNezor);

            yield return new ParagraphModel(string.Format(PrayTexts.OseShalom, PrayTexts.OseShalomRegular));

            yield return new ParagraphModel(PrayTexts.SE_End) { Title = nusach == Nusach.EdotMizrach ? PrayTexts.ResourceManager.GetString("AfterHazarahTitle") : null };
        }

        public static void AddAvinuMalkenu(List<TextsModel> texts, Nusach nusach, bool isAYT)
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

















        class StubLocationService : ILocationService
        {
            public bool IsActive
            {
                get
                {
                    return true;
                }
            }

            public Task<Geoposition> GetCurrentPositionAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new Geoposition(-122.084, 37.4219983333333, 5));
            }

            public Task<bool> IsInIsraelAsync()
            {
                return Task.FromResult(true);
            }
        }

        class StubTimeService : ITimeService
        {
            TimeService _realTimeService;
            DateTime _fakeTime;

            public StubTimeService(TimeService realTimeService)
            {
                _realTimeService = realTimeService;
            }

            public void SetFakeTime(DateTime fakeTime)
            {
                _fakeTime = fakeTime;
            }

            public Task<PrayerInfo> GetArvitInfoAsync(Geoposition location = null, DateTime? relativeToDate = null)
            {
                throw new NotImplementedException();
            }

            public Task<ComplexZmanimCalendar> GetCurrentZmanimCalendarAsync(DateTime? date = null, Geoposition position = null)
            {
                throw new NotImplementedException();
            }

            public async Task<DayJewishInfo> GetDayInfoAsync(Geoposition location = null, DateTime? relativeToDate = null, bool dayCritical = false)
            {
                return await _realTimeService.GetDayInfoAsync(location, relativeToDate ?? _fakeTime, dayCritical);
            }

            public Task<DateTime?> GetKnissatShabbatAsync(Geoposition position, DateTime? date)
            {
                throw new NotImplementedException();
            }

            public Task<PrayerInfo> GetMinchaInfoAsync(Geoposition location = null, DateTime? relativeToDate = null)
            {
                throw new NotImplementedException();
            }

            public Task<DateTime?> GetNightChatzotAsync(bool fallBackToCivilMidnight, Geoposition position = null, DateTime? date = null)
            {
                throw new NotImplementedException();
            }

            public Task<PrayerInfo> GetShacharitInfoAsync(Geoposition location = null, DateTime? relativeToDate = null)
            {
                throw new NotImplementedException();
            }

            public Task<DateTime?> GetSunsetAsync(Geoposition position = null)
            {
                throw new NotImplementedException();
            }
        }

        class DummyLogger : ILogger, IDisposable
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return this;
            }

            public void Dispose()
            {

            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return false;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {

            }
        }
    }
}
