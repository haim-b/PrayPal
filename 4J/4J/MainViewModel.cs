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
        }

        public DayTimesViewModel DayTimes { get; }
    }
}
