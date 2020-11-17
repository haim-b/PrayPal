using Microsoft.Extensions.Logging;
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
        public static async Task TestPrayerAsync(Func<JewishCalendar, IEnumerable<string>> legacyPrayerFactory, Func<IPrayer> newPrayerFactory, DateTime? fromTime = null, DateTime? toTime = null)
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
                IEnumerable<string> legacyText = legacyPrayerFactory(dayJewishInfo.JewishCalendar);

                var t = newPrayerFactory();
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
