using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal.Common
{
    public interface ISettingsProvider
    {
        bool GetValue(string key, bool defaultValue);

        string GetValue(string key, string defaultValue);

        int GetValue(string key, int defaultValue);

        double GetValue(string key, double defaultValue);

        long GetValue(string key, long defaultValue);

        void SetValue(string key, bool value);

        void SetValue(string key, string value);

        void SetValue(string key, int value);

        void SetValue(string key, double value);

        void SetValue(string key, long value);
    }
}
