using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using PrayPal.DayTimes;

namespace PrayPal
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel(DayTimesViewModel dayTimes)
        {
            DayTimes = dayTimes ?? throw new ArgumentNullException(nameof(dayTimes));
            DayTimes.ShowPrayersTime = true;
            DayTimes.ShowRelativePrayers = true;
            DayTimes.GenerateContentAsync();
        }

        public DayTimesViewModel DayTimes { get; }
    }
}
