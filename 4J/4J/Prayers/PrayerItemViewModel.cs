using PrayPal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Prayers
{
    public class PrayerItemViewModel : ItemViewModel
    {
        public PrayerItemViewModel(string pageName, string title)//, object pageParameter = null)
            : base(title, null)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                throw new ArgumentException($"'{nameof(pageName)}' cannot be null or whitespace", nameof(pageName));
            }

            PageName = pageName;
            //PageParameter = pageParameter;
        }

        public PrayerItemViewModel(string pageName, string title, string subtitle)//, object pageParameter = null)
            : base(title, subtitle)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                throw new ArgumentException($"'{nameof(pageName)}' cannot be null or whitespace", nameof(pageName));
            }

            PageName = pageName;
            //PageParameter = pageParameter;
        }

        public PrayerItemViewModel(string pageName, string title, Type viewModelType)
            : this(pageName, title)
        {
            ViewModelType = viewModelType ?? throw new ArgumentNullException(nameof(viewModelType));
        }

        public string PageName { get; }

        //public object PageParameter { get; }

        public Type ViewModelType { get; }
    }
}
