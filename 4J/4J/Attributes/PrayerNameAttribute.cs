using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PrayerNameAttribute : Attribute
    {
        public PrayerNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Name = name;
        }

        public string Name { get; set; }
    }
}
