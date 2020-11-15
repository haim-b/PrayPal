using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Zmanim.HebrewCalendar;

namespace PrayPal.Tools.Calendar
{
    public class HebrewCalendarDayViewModel : BindableBase
    {
        private static readonly HebrewDateFormatter _hebrewDateFormatter = new HebrewDateFormatter() { UseGershGershayim = false, HebrewFormat = true, UseEndLetters = false };

        public HebrewCalendarDayViewModel(JewishCalendar jewishCalendar, int gridRow)
        {
            JewishCalendar = jewishCalendar ?? throw new ArgumentNullException(nameof(jewishCalendar));
            DayTitle = _hebrewDateFormatter.formatHebrewNumber(jewishCalendar.JewishDayOfMonth);
            HasEvent = jewishCalendar.YomTovIndex >= 0;
            GridColumn = jewishCalendar.DayOfWeek - 1;
            GridRow = gridRow;
        }

        public JewishCalendar JewishCalendar { get; }

        public string DayTitle { get; }

        public bool HasEvent { get; }

        public int GridColumn { get; }

        public int GridRow { get; }
    }
}
