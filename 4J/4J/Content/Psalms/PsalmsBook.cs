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
    [PrayerName(BookNames.Psalms)]
    public class PsalmsBook : SpansPrayerBase, ICustomizedPrayer
    {
        public const string TodayInWeekVersesSentinel = "PsalmsTodayInWeekVerses";
        public const string TodayInMonthVersesSentinel = "PsalmsTodayInMonthVerses";
        public const string VersesSentinel = "Verse";

        private readonly ITimeService _timeService;
        private readonly ILogger _logger;

        private DocumentModel _model;
        private int _dayOrVerse;
        private string _sentinel;
        private object _contentGenerationParameter;

        public PsalmsBook(ITimeService timeService, ILogger<PsalmsBook> logger)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public object ContentGenerationParameter
        {
            get { return _contentGenerationParameter; }
            set
            {
                _contentGenerationParameter = value;
                GenerateModel(value as string);
            }
        }

        private async void GenerateModel(string value)
        {
            var parameters = await ParseParameterAsync(value);
            _model = parameters.document;
            _sentinel = parameters.sentinel;
            _dayOrVerse = parameters.dayOrVerse;
            Title = _model.Title;
        }

        protected override Task CreateOverrideAsync()
        {
            return Task.Run(new Action(CreateContent));
        }

        private void CreateContent()
        {
            foreach (SpanModel span in _model.Texts)
            {
                Add(span.Title, span.ToArray());
            }
        }

        private async Task<(DocumentModel document, string sentinel, int dayOrVerse)> ParseParameterAsync(string parameter)
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
                    //JewishCalendar jc = HebDateHelper.GetCalendarToday();
                    dayOrVerse = (int)DateTime.Now.DayOfWeek + 1; //jc.DayOfWeek;
                }

                return (PsalmsTextGenerator.GetVersesTodayForWeek(dayOrVerse), sentinel, dayOrVerse);
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

                return (await PsalmsTextGenerator.GetVersesTodayForMonthAsync(_timeService, dayOrVerse), sentinel, dayOrVerse);
            }
            else if (parameter.StartsWith(VersesSentinel))
            {
                sentinel = VersesSentinel;

                if (!int.TryParse(parameter.Replace(VersesSentinel, ""), out dayOrVerse))
                {
                    dayOrVerse = 1;
                }

                return (PsalmsTextGenerator.GetAllVerses(), sentinel, dayOrVerse);
            }

            throw new NotSupportedException("Content generation parameter value is invalid.");
        }

        protected override string GetTitle()
        {
            return _model?.Title;
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
