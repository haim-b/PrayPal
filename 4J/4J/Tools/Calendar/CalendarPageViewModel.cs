using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.DayTimes;
using PrayPal.Resources;
using PrayPal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zmanim.HebrewCalendar;

namespace PrayPal.Tools.Calendar
{
    public class CalendarPageViewModel : PageViewModelBase, IContentPage
    {
        private readonly ITimeService _timeService;
        private ObservableCollection<HebrewCalendarDayViewModel> _days;
        private JewishCalendar _selectedDay;
        private DayTimesViewModel _selectedDayInfo;
        private int _monthFromNissan;
        private int _year;
        //private string _monthString;
        //private string _yearString;
        private bool _isInitialized;

        private readonly DayTimesViewModel _dayTimesViewModel;

        private static readonly ReadOnlyCollection<string> _years;
        private static readonly string[] _monthNames;
        private string _todayDayTitle;
        private const int StartYear = 5700;

        static CalendarPageViewModel()
        {
            HebrewDateFormatter formatter = new HebrewDateFormatter { UseGershGershayim = true, HebrewFormat = true, UseEndLetters = true };

            List<string> years = new List<string>(6000);

            years.AddRange(Enumerable.Range(StartYear, 5800 - StartYear).Select(i => formatter.formatHebrewNumber(i)));

            _years = new ReadOnlyCollection<string>(years);

            _monthNames = CommonResources.HebMonths.Split('|');

            if (_monthNames.Length != 14)
            {
                throw new InvalidOperationException("There must be 14 registered months, including Regular Adar, Adar 1 and Adar 2.");
            }
        }

        public CalendarPageViewModel(ITimeService timeService, ILocationService locationService, INotificationService notificationService, IErrorReportingService errorReportingService, ILogger<DayTimesViewModel> dayTimesLogger, ILogger<CalendarPageViewModel> logger)
            : base(AppResources.CalendarTitle, notificationService, errorReportingService, logger)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _dayTimesViewModel = new DayTimesViewModel(locationService, timeService, dayTimesLogger) { ShowPrayersTime = true, ShowRelativePrayers = false, IncludeIsruChag = false, ShowGregorianDate = true };

            IncreaseMonthCommand = new Command(IncreaseMonth, CanIncreaseMonth);
            DecreaseMonthCommand = new Command(DecreaseMonth, CanDecreaseMonth);
            SelectDayCommand = new Command<HebrewCalendarDayViewModel>(ExecuteSelectDay);
            GoToTodayCommand = new Command(ExecuteGoToTodayCommand);
            Months = new ObservableCollection<string>();

            // Add Tishrei to Adar:
            for (int i = 0; i <= 5; i++)
            {
                Months.Add(_monthNames[i]);
            }

            // Add Nissan to Ellul:
            for (int i = 8; i <= 13; i++)
            {
                Months.Add(_monthNames[i]);
            }

            // Supply initial values before the location is resolved:
            JewishCalendar jc = new JewishCalendar(DateTime.Now, Settings.IsInIsrael);
            MonthFromNissan = jc.JewishMonth;
            Year = jc.JewishYear;
            _selectedDay = jc;


            ExecuteGoToTodayCommand();
        }

        public ObservableCollection<HebrewCalendarDayViewModel> Days
        {
            get { return _days; }
            private set { SetProperty(ref _days, value); }
        }

        public string TodayDayTitle
        {
            get { return _todayDayTitle; }
            set { SetProperty(ref _todayDayTitle, value); }
        }

        public int MonthFromNissan
        {
            get { return _monthFromNissan; }
            set
            {
                SetProperty(ref _monthFromNissan, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MonthIndex)));
                BuildItems();
                RefreshCommands();
            }
        }

        public int MonthIndex
        {
            // The getter's formula is this: Shift 6 (or 7 if leap) months forward (for example, Nissan is 1 but is actually the 7th or 8th).
            // Then we subtract 1 to make is zero-based. Then we mod by 12/13 to make the negative value cyclic. Mod is last because it provides zero-based values.
            // Setter is the opposite.
            get
            {
                int leapYear = JewishCalendar.IsJewishLeapYear(_year) ? 1 : 0;
                return Mod(MonthFromNissan + 6 + leapYear - 1, 12 + leapYear);
            }
            set
            {
                if (value == -1)
                {
                    return;
                }

                int leapYear = JewishCalendar.IsJewishLeapYear(_year) ? 1 : 0;
                MonthFromNissan = Mod(value - 6 - leapYear, 12 + leapYear) + 1;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MonthIndex)));
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                int oldValue = _year;
                SetProperty(ref _year, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(YearIndex)));

                UpdateMonthsList(oldValue);
                BuildItems();
                RefreshCommands();
            }
        }

        public int YearIndex
        {
            get { return Year - StartYear; }
            set
            {
                Year = value + StartYear;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(YearIndex)));
            }
        }

        public JewishCalendar SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                SetProperty(ref _selectedDay, value);

                if (value == null)
                {
                    SelectedDayInfo = null;
                }
                else
                {
                    _dayTimesViewModel.JewishDate = value;
                    SelectedDayInfo = _dayTimesViewModel;
                }
            }
        }

        public DayTimesViewModel SelectedDayInfo
        {
            get { return _selectedDayInfo; }
            set { SetProperty(ref _selectedDayInfo, value); }
        }

        public ObservableCollection<string> Months { get; }

        public ReadOnlyCollection<string> Years
        {
            get { return _years; }
        }

        public Command IncreaseMonthCommand { get; }

        public Command DecreaseMonthCommand { get; }

        public Command SelectDayCommand { get; }



        public Command GoToTodayCommand { get; }

        private async Task SelectTodayAsync()
        {
            DayJewishInfo dayInfo = await _timeService.GetDayInfoAsync().ConfigureAwait(true);

            if (dayInfo != null)
            {
                BeginInit();
                Year = dayInfo.JewishCalendar.JewishYear;
                MonthFromNissan = dayInfo.JewishCalendar.JewishMonth;
                EndInit();
                SelectedDay = dayInfo.JewishCalendar;
            }

            if (_todayDayTitle == null)
            {
                HebrewDateFormatter formatter = new HebrewDateFormatter { UseGershGershayim = false, HebrewFormat = true, UseEndLetters = false };

                TodayDayTitle = formatter.formatHebrewNumber(dayInfo.JewishCalendar.JewishDayOfMonth);
            }
        }

        private async void ExecuteGoToTodayCommand()
        {
            try
            {
                await SelectTodayAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to select today.");
            }
        }


        private void UpdateMonthsList(int oldYear)
        {
            bool isOldYearLeap = oldYear != 0 && JewishCalendar.IsJewishLeapYear(oldYear);
            bool isNewYearLeap = JewishCalendar.IsJewishLeapYear(_year);

            if (isOldYearLeap == isNewYearLeap)
            {
                return;
            }

            if (isNewYearLeap)
            {
                Months.RemoveAt(5);
                Months.Insert(5, _monthNames[6]);
                Months.Insert(6, _monthNames[7]);

                if (MonthIndex > 5)
                {
                    MonthIndex++;
                }
            }
            else
            {
                Months.RemoveAt(6); // We remove Adar 2 first and then Adar 1 to keep the list stable. We could have also remove at (5) twice.
                Months.RemoveAt(5);
                Months.Insert(5, _monthNames[5]);

                if (MonthIndex > 5)
                {
                    MonthIndex--;
                }
            }

            if (Months.Count < _monthFromNissan)
            {
                MonthFromNissan = Months.Count - 1;
            }
        }

        private static int Mod(int a, int b)
        {
            return (int)(a - b * Math.Floor((double)a / b));
        }

        public async Task GenerateContentAsync()
        {
            var today = await _timeService.GetDayInfoAsync();

            MonthFromNissan = today.JewishCalendar.JewishMonth;
            Year = today.JewishCalendar.JewishYear;

            _isInitialized = true;

            BuildItems();
        }

        private void IncreaseMonth()
        {
            JewishCalendar jc = new JewishCalendar(Year, MonthFromNissan, 1, Settings.IsInIsrael);

            // Go to the last day of the month:
            jc.JewishDayOfMonth = jc.DaysInJewishMonth;
            jc.forward();

            BeginInit();
            // Set the year first so that the months list will update before we set the month index:
            Year = jc.JewishYear;
            MonthFromNissan = jc.JewishMonth;
            EndInit();
        }


        private bool CanIncreaseMonth()
        {
            return !(MonthIndex == Months.Count - 1 && YearIndex == _years.Count - 1);
        }


        public void DecreaseMonth()
        {
            JewishCalendar jc = new JewishCalendar(Year, MonthFromNissan, 1, Settings.IsInIsrael);
            jc.back();

            BeginInit();
            // Set the year first so that the months list will update before we set the month index:
            Year = jc.JewishYear;
            MonthFromNissan = jc.JewishMonth;
            EndInit(); ;
        }

        private bool CanDecreaseMonth()
        {
            return !(MonthIndex == 0 && YearIndex == 0);
        }

        private void RefreshCommands()
        {
            IncreaseMonthCommand.ChangeCanExecute();
            DecreaseMonthCommand.ChangeCanExecute();
        }

        private void ExecuteSelectDay(HebrewCalendarDayViewModel day)
        {
            foreach (var item in Days)
            {
                if (item.JewishCalendar == day?.JewishCalendar)
                {
                    item.IsSelected = true;
                    continue;
                }

                item.IsSelected = false;
            }

            SelectedDay = day.JewishCalendar;
        }

        private void BeginInit()
        {
            _isInitialized = false;
        }

        private void EndInit()
        {
            _isInitialized = true;
            BuildItems();
        }

        private void BuildItems()
        {
            if (!_isInitialized)
            {
                return;
            }

            JewishCalendar selectedValue = _selectedDay;

            int month = MonthFromNissan;

            JewishCalendar jc = new JewishCalendar(Year, month, 1, Settings.IsInIsrael) { UseModernHolidays = true };

            int row = 1;
            bool selectedValueFound = false;

            ObservableCollection<HebrewCalendarDayViewModel> days = new ObservableCollection<HebrewCalendarDayViewModel>();

            while (jc.JewishMonth == month)
            {
                HebrewCalendarDayViewModel item = new HebrewCalendarDayViewModel(jc, row);

                days.Add(item);


                if (jc.DayOfWeek == 7)
                {
                    row++;
                }

                if (selectedValue != null && selectedValue.Equals(jc))
                {
                    item.IsSelected = true;
                    selectedValueFound = true;
                }

                jc = jc.CloneEx();
                jc.forward();
            }

            // Go back to the right month:
            jc.back();

            Days = days;


            if (selectedValueFound)
            {
                SelectedDay = selectedValue;
            }
        }

    }
}
