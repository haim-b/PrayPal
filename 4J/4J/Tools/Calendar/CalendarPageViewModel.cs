using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Resources;
using PrayPal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        private HebrewCalendarDayViewModel _selectedDay;
        private int _month;
        private int _year;
        private string _DateString;
        private bool _isInitialized;

        public CalendarPageViewModel(ITimeService timeService, IErrorReportingService errorReportingService)
            : base(AppResources.CalendarTitle, errorReportingService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

            IncreaseMonthCommand = new Command(IncreaseMonth);
            DecreaseMonthCommand = new Command(DecreaseMonth);
        }

        public ObservableCollection<HebrewCalendarDayViewModel> Days
        {
            get { return _days; }
            private set { SetProperty(ref _days, value); }
        }

        public int Month
        {
            get { return _month; }
            set
            {
                SetProperty(ref _month, value);
                BuildItems();
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                SetProperty(ref _year, value);
                BuildItems();
            }
        }

        public string CalendarPageTitle
        {
            get { return _DateString; }
            set { SetProperty(ref _DateString, value); }
        }

        public HebrewCalendarDayViewModel SelectedDay
        {
            get { return _selectedDay; }
            set { SetProperty(ref _selectedDay, value); }
        }

        public Command IncreaseMonthCommand { get; }

        public Command DecreaseMonthCommand { get; }

        public async Task GenerateContentAsync()
        {
            var today = await _timeService.GetDayInfoAsync();

            Month = today.JewishCalendar.JewishMonth;
            Year = today.JewishCalendar.JewishYear;

            _isInitialized = true;

            BuildItems();
        }

        public void IncreaseMonth()
        {
            JewishCalendar jc = new JewishCalendar(Year, Month, 1);
            jc.JewishDayOfMonth = jc.DaysInJewishMonth;
            jc.forward();

            BeginInit();
            Month = jc.JewishMonth;
            Year = jc.JewishYear;
            EndInit();
        }

        public void DecreaseMonth()
        {
            JewishCalendar jc = new JewishCalendar(Year, Month, 1);
            jc.back();

            BeginInit();
            Month = jc.JewishMonth;
            Year = jc.JewishYear;
            EndInit();
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

            JewishCalendar selectedValue = _selectedDay?.JewishCalendar;

            int month = Month;
            HebrewDateFormatter formatter = new HebrewDateFormatter { UseGershGershayim = false, HebrewFormat = true, UseEndLetters = false };
            JewishCalendar jc = new JewishCalendar(Year, month, 1) { InIsrael = Settings.IsInIsrael, UseModernHolidays = true };

            int row = 1;
            bool selectedValueFound = false;

            ObservableCollection<HebrewCalendarDayViewModel> days = new ObservableCollection<HebrewCalendarDayViewModel>();

            while (jc.JewishMonth == month)
            {
                HebrewCalendarDayViewModel item = new HebrewCalendarDayViewModel(jc, row);
                //item.Content = formatter.formatHebrewNumber(jc.JewishDayOfMonth);
                days.Add(item);


                if (jc.DayOfWeek == 7)
                {
                    row++;
                }

                if (selectedValue != null && selectedValue.Equals(jc))
                {
                    //item.IsSelected = true;
                    selectedValueFound = true;
                }

                jc = HebDateHelper.Clone(jc);
                jc.forward();
            }

            // Go back to the right month:
            jc.back();

            Days = days;

            formatter.UseGershGershayim = true;
            CalendarPageTitle = formatter.formatMonth(jc) + " " + formatter.formatHebrewNumber(jc.JewishYear);

            if (selectedValueFound)
            {
                //SelectedDay = selectedValue;
            }
        }

    }
}
