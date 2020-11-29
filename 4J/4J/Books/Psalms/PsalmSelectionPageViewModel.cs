using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Content;
using PrayPal.Resources;
using PrayPal.Services;
using PrayPal.TextPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zmanim.HebrewCalendar;

namespace PrayPal.Books.Psalms
{
    public class PsalmSelectionPageViewModel : PageViewModelBase
    {
        private readonly ITimeService _timeService;
        private readonly INavigationService _navigationService;

        private bool _isBeforeTextExpanded;

        private bool _isAfterTextExpanded;

        private readonly Command _showVerseByWeekDayCommand;
        private readonly Command _showVerseByMonthCommand;
        private readonly Command _showByVerseCommand;

        private int _selectedVerseByWeekDayIndex;
        private int _selectedVerseByMonthDayIndex;
        private List<string> _versesByWeekDay;
        private List<Tuple<string, string>> _versesByMonthDay;

        public PsalmSelectionPageViewModel(ITimeService timeService, INavigationService navigationService, INotificationService notificationService, IErrorReportingService errorReportingService, ILogger<PsalmSelectionPageViewModel> logger)
            : base(AppResources.GoToPsalmVerseTitle, notificationService, errorReportingService, logger)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            _showVerseByWeekDayCommand = new Command(ExecuteShowVerseByDayCommand);
            _showVerseByMonthCommand = new Command(ExecuteShowVerseByMonthCommand);
            _showByVerseCommand = new Command(ExecuteShowByVerseCommand);

            BeforePsalmText = Resources.Psalms.BeforeReading1 + Environment.NewLine + Resources.Psalms.BeforeReading2;
            AfterPsalmText = Resources.Psalms.AfterReading;

            FillItemAsync();
        }

        public List<string> VersesByWeekDay
        {
            get { return _versesByWeekDay; }
            set { SetProperty(ref _versesByWeekDay, value); }
        }

        public List<Tuple<string, string>> VersesByMonthDay
        {
            get { return _versesByMonthDay; }
            set { SetProperty(ref _versesByMonthDay, value); }
        }


        public bool IsBeforeTextExpanded
        {
            get { return _isBeforeTextExpanded; }
            set
            {
                SetProperty(ref _isBeforeTextExpanded, value);

                if (IsBeforeTextExpanded)
                {
                    IsAfterTextExpanded = false;
                }
            }
        }
        public bool IsAfterTextExpanded
        {
            get { return _isAfterTextExpanded; }
            set
            {
                SetProperty(ref _isAfterTextExpanded, value);

                if (IsAfterTextExpanded)
                {
                    IsBeforeTextExpanded = false;
                }
            }
        }


        public string BeforePsalmText { get; private set; }

        public string AfterPsalmText { get; private set; }

        public string PsalmVerse { get; set; }

        public int SelectedVerseByWeekDayIndex
        {
            get { return _selectedVerseByWeekDayIndex; }
            set
            {
                // We don't use SetProperty because we want the PropertyChanged event if the index remains 0,
                // otherwise the UI will not show the selected item if it's the first one:
                _selectedVerseByWeekDayIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedVerseByWeekDayIndex)));
            }
        }

        public int SelectedVerseByMonthDayIndex
        {
            get { return _selectedVerseByMonthDayIndex; }
            set
            {
                // We don't use SetProperty because we want the PropertyChanged event if the index remains 0,
                // otherwise the UI will not show the selected item if it's the first one:
                _selectedVerseByMonthDayIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedVerseByMonthDayIndex)));
            }
        }

        public Command ShowVerseByWeekDayCommand
        {
            get { return _showVerseByWeekDayCommand; }
        }

        public Command ShowVerseByMonthCommand
        {
            get { return _showVerseByMonthCommand; }
        }

        public Command ShowByVerseCommand
        {
            get { return _showByVerseCommand; }
        }

        private async Task FillItemAsync()
        {
            try
            {
                DayJewishInfo dayInfo = await _timeService.GetDayInfoAsync();

                if (dayInfo == null)
                {
                    Logger.LogError("Cannot find day info for psalm items.");
                    return;
                }

                SetVersesByWeekDay(dayInfo);
                SetVersesByMonthDay(dayInfo);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Cannot fill psalm items.");
            }
        }

        private void SetVersesByWeekDay(DayJewishInfo dayInfo)
        {
            string title = null;
            DateTimeFormatInfo fi = new CultureInfo("he-IL").DateTimeFormat;

            List<string> items = new List<string>(7);

            for (int i = 1; i <= 7; i++)
            {
                title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[i - 1]);

                items.Add(title);
            }

            VersesByWeekDay = items;
            SelectedVerseByWeekDayIndex = dayInfo.JewishCalendar.DayOfWeek - 1;
        }

        private void SetVersesByMonthDay(DayJewishInfo dayInfo)
        {
            HebrewDateFormatter formatter = new HebrewDateFormatter();
            formatter.UseGershGershayim = true;
            formatter.UseEndLetters = false;

            List<Tuple<string, string>> items = new List<Tuple<string, string>>(30);

            for (int i = 1; i <= dayInfo.JewishCalendar.DaysInJewishMonth; i++)
            {
                string title = string.Format(AppResources.PsalmForMonthDayTitleFormat, formatter.formatHebrewNumber(i));
                string titleShort = string.Format(AppResources.PsalmForMonthDayTitleShortFormat, formatter.formatHebrewNumber(i));
                items.Add(new Tuple<string, string>(title, titleShort));
            }

            VersesByMonthDay = items;
            SelectedVerseByMonthDayIndex = dayInfo.JewishCalendar.JewishDayOfMonth - 1;
        }

        private async void ExecuteShowVerseByDayCommand()
        {
            Logger.LogInformation("Opened Psalms day of week");
            Analytics.TrackEvent("Opened Psalm by week", Utils.AnalyticsProperty("day", (_selectedVerseByWeekDayIndex + 1).ToString()));

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel),
                TextPresenterViewModel.TextNameParam, BookNames.Psalms,
                TextPresenterViewModel.TextParamParam, PsalmsBook.TodayInWeekVersesSentinel + (_selectedVerseByWeekDayIndex + 1));
        }

        private async void ExecuteShowVerseByMonthCommand()
        {
            Logger.LogInformation("Opened Psalms by day of month");
            Analytics.TrackEvent("Opened Psalm by month", Utils.AnalyticsProperty("day", (_selectedVerseByMonthDayIndex + 1).ToString()));

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel),
                TextPresenterViewModel.TextNameParam, BookNames.Psalms,
                TextPresenterViewModel.TextParamParam, PsalmsBook.TodayInMonthVersesSentinel + (_selectedVerseByMonthDayIndex + 1));
        }

        private async void ExecuteShowByVerseCommand()
        {
            int verseIndex = GetVerseIndex(PsalmVerse ?? "1");

            Logger.LogInformation("Opened Psalms by verse");
            Analytics.TrackEvent("Opened Psalm by verse", Utils.AnalyticsProperty("verse", (verseIndex + 1).ToString()));

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel),
                TextPresenterViewModel.TextNameParam, BookNames.Psalms,
                TextPresenterViewModel.TextParamParam, PsalmsBook.VersesSentinel + PsalmVerse,
                TextPresenterViewModel.StartFromParagraphIndexParam, verseIndex.ToString());
        }

        private int GetVerseIndex(string verse)
        {
            int gematriya;

            if (!int.TryParse(verse, out gematriya))
            {
                gematriya = GetGematriyaValue(verse);
            }

            // Verse 119 is split into 11 parts (1 like every verse + 10 more):
            if (gematriya > 119)
            {
                gematriya += 10;
            }

            return gematriya - 1; // The index is zero-based.
        }

        private int GetGematriyaValue(string verse)
        {
            int result = 0;

            foreach (char c in verse)
            {
                switch (c)
                {
                    case 'א':
                        result += 1;
                        break;
                    case 'ב':
                        result += 2;
                        break;
                    case 'ג':
                        result += 3;
                        break;
                    case 'ד':
                        result += 4;
                        break;
                    case 'ה':
                        result += 5;
                        break;
                    case 'ו':
                        result += 6;
                        break;
                    case 'ז':
                        result += 7;
                        break;
                    case 'ח':
                        result += 8;
                        break;
                    case 'ט':
                        result += 9;
                        break;
                    case 'י':
                        result += 10;
                        break;
                    case 'כ':
                        result += 20;
                        break;
                    case 'ל':
                        result += 30;
                        break;
                    case 'מ':
                        result += 40;
                        break;
                    case 'נ':
                        result += 50;
                        break;
                    case 'ס':
                        result += 60;
                        break;
                    case 'ע':
                        result += 70;
                        break;
                    case 'פ':
                        result += 80;
                        break;
                    case 'צ':
                        result += 90;
                        break;
                    case 'ק':
                        result += 100;
                        break;
                    case 'ר':
                        result += 200;
                        break;
                    case 'ש':
                        result += 300;
                        break;
                    case 'ת':
                        result += 400;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

    }
}
