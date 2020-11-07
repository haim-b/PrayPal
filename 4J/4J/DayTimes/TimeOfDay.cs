using System;
using System.Collections.Generic;
using System.Text;

namespace Yahadut.DayTimes
{
    public class TimeOfDay
    {
        public TimeOfDay(string title, DateTime time)
        {
            Time = time.ToString("t");
            Title = string.Format("{0}: {1}", title, Time);
        }

        public TimeOfDay(string title)
        {
            Title = title;
        }

        public string Title { get; set; }

        public string Time { get; set; }
    }
}
