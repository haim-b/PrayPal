using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zmanim.HebrewCalendar;

namespace PrayPal.Tools.Calendar
{
    [ViewFor(typeof(CalendarPageViewModel))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentPage
    {
        public static readonly BindableProperty DaysProperty = BindableProperty.Create(nameof(Days), typeof(IEnumerable<HebrewCalendarDayViewModel>), typeof(CalendarView), propertyChanged: OnDaysChanged);

        public CalendarView()
        {
            InitializeComponent();
        }

        public IEnumerable<HebrewCalendarDayViewModel> Days
        {
            get { return (IEnumerable<HebrewCalendarDayViewModel>)GetValue(DaysProperty); }
            set { SetValue(DaysProperty, value); }
        }


        private static void OnDaysChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CalendarView cv = (CalendarView)bindable;

            cv.OnDaysChanged((IEnumerable<HebrewCalendarDayViewModel>)newValue);
        }

        private void OnDaysChanged(IEnumerable<HebrewCalendarDayViewModel> days)
        {
            //calendar.Children.Clear();

            //DateTimeFormatInfo fi = new CultureInfo("he-IL").DateTimeFormat;

            //for (int i = 0; i < 7; i++)
            //{
            //    Label item = new Label();
            //    item.Text = fi.AbbreviatedDayNames[i];
            //    Grid.SetColumn(item, i);
            //    calendar.Children.Add(item);
            //}

            //if (days == null)
            //{
            //    return;
            //}

            //int row = 1;

            //foreach (HebrewCalendarDayViewModel day in days)
            //{
            //    ContentView item = new ContentView();
            //    item.Content = new Label { Text = _hebrewDateFormatter.formatHebrewNumber(day.JewishCalendar.JewishDayOfMonth) };
            //    Grid.SetColumn(item, day.JewishCalendar.DayOfWeek - 1);
            //    Grid.SetRow(item, row);


            //    if (day.JewishCalendar.DayOfWeek == 7)
            //    {
            //        row++;
            //    }

            //    calendar.Children.Add(item);
            //}
        }
    }
}