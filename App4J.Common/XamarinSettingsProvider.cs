using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace PrayPal.Common
{
    public class XamarinSettingsProvider : ISettingsProvider
    {
        public bool GetValue(string key, bool defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public string GetValue(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public int GetValue(string key, int defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public double GetValue(string key, double defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public long GetValue(string key, long defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public void SetValue(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public void SetValue(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public void SetValue(string key, int value)
        {
            Preferences.Set(key, value);
        }

        public void SetValue(string key, double value)
        {
            Preferences.Set(key, value);
        }

        public void SetValue(string key, long value)
        {
            Preferences.Set(key, value);
        }
    }
}
