using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Zmanim.HebrewCalendar;

namespace PrayPal.Tools.Calendar
{
    public class HebrewCalendarDayViewModel : BindableBase
    {
        private static readonly HebrewDateFormatter _hebrewDateFormatter = new HebrewDateFormatter() { UseGershGershayim = false, HebrewFormat = true, UseEndLetters = false };
        private static readonly CultureInfo _hebrewCulture = new CultureInfo("he");

        private bool _isSelected;


        public HebrewCalendarDayViewModel(JewishCalendar jewishCalendar, int gridRow)
        {
            JewishCalendar = jewishCalendar ?? throw new ArgumentNullException(nameof(jewishCalendar));
            DayTitle = _hebrewDateFormatter.formatHebrewNumber(jewishCalendar.JewishDayOfMonth);
            HasEvent = jewishCalendar.YomTovIndex >= 0;
            GridColumn = jewishCalendar.DayOfWeek - 1;
            GridRow = gridRow;

            // Show month on first square or for squares of the first civil month:
            bool showCivilMonth = jewishCalendar.Time.Day == 1 || jewishCalendar.JewishDayOfMonth == 1;

            CivilTitle = showCivilMonth ? jewishCalendar.Time.ToString("d MMM", _hebrewCulture) : jewishCalendar.Time.Day.ToString();
        }

        public JewishCalendar JewishCalendar { get; }

        public string DayTitle { get; }

        public string CivilTitle { get; }

        public bool HasEvent { get; }

        public int GridColumn { get; }

        public int GridRow { get; }
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

    }
}
