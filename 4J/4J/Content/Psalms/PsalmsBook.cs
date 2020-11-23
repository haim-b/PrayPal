using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Models;

namespace PrayPal.Content
{
    [TextName(BookNames.Psalms)]
    public class PsalmsBook : SpansPrayerBase, ICustomizedPrayer
    {
        public const string TodayInWeekVersesSentinel = "PsalmsTodayInWeekVerses";
        public const string TodayInMonthVersesSentinel = "PsalmsTodayInMonthVerses";
        public const string VersesSentinel = "Verse";

        private Func<DocumentModel<SpanModel>> _documentFactory;
        private int _dayOrVerse;
        private string _sentinel;
        private string _contentGenerationParameter;

        public string ContentGenerationParameter
        {
            get { return _contentGenerationParameter; }
            set
            {
                _contentGenerationParameter = value;
                GenerateModel(value as string);
            }
        }

        private void GenerateModel(string value)
        {
            var parameters = ParseParameter(value);
            _documentFactory = parameters.documentFactory;
            _sentinel = parameters.sentinel;
            _dayOrVerse = parameters.dayOrVerse;
        }

        protected override async Task CreateOverrideAsync()
        {
            //await Task.Run(new Action(CreateContent));
            CreateContent();
        }

        private void CreateContent()
        {
            var doc = _documentFactory.Invoke();
            Title = doc.Title;

            foreach (SpanModel span in doc.Texts)
            {
                _items.Add(span);
            }
        }

        private (Func<DocumentModel<SpanModel>> documentFactory, string sentinel, int dayOrVerse) ParseParameter(string parameter)
        {
            string sentinel;
            int dayOrVerse;

            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.StartsWith(TodayInWeekVersesSentinel))
            {
                sentinel = TodayInWeekVersesSentinel;

                if (!int.TryParse(parameter.Replace(TodayInWeekVersesSentinel, ""), out dayOrVerse))
                {
                    dayOrVerse = DayInfo.JewishCalendar.DayOfWeek;
                }

                return (() => PsalmsTextGenerator.GetVersesTodayForWeek(dayOrVerse), sentinel, dayOrVerse);
            }
            else if (parameter.StartsWith(TodayInMonthVersesSentinel))
            {
                sentinel = TodayInMonthVersesSentinel;

                if (!int.TryParse(parameter.Replace(TodayInMonthVersesSentinel, ""), out dayOrVerse))
                {
                    throw new InvalidOperationException("Parameter has no valid day.");
                    //JewishCalendar jc = HebDateHelper.GetCalendarToday();
                    //dayOrVerse = jc.JewishDayOfMonth;
                }

                return (() => PsalmsTextGenerator.GetVersesTodayForMonth(DayInfo.JewishCalendar, dayOrVerse), sentinel, dayOrVerse);
            }
            else if (parameter.StartsWith(VersesSentinel))
            {
                sentinel = VersesSentinel;

                if (!int.TryParse(parameter.Replace(VersesSentinel, ""), out dayOrVerse))
                {
                    dayOrVerse = 1;
                }

                return (() => PsalmsTextGenerator.GetAllVerses(), sentinel, dayOrVerse);
            }

            throw new NotSupportedException("Content generation parameter value is invalid.");
        }

        protected override string GetTitle()
        {
            return Title;
        }

        public override bool UseCompactZoomedOutItems
        {
            get
            {
                return true;
            }
        }
    }
}
