using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Globalization;

namespace PrayPal.Common
{
    public class Settings
    {
        // static CultureInfo _language;

        static Settings()
        {
            //string langauge = AppSettings.GetValueOrDefault(LanguageKey, null);

            //if (langauge == null)
            //{
            //    langauge = "he";
            //}

            //Language = new CultureInfo(langauge);
        }

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string LanguageKey = "Language";
        private const string NusachKey = "Nusach";
        private const string IsInIsraelKey = "IsInIsrael";
        private const string TimeCalcMethodKey = "TimeCalcMethod";
        private const string UseLocationKey = "UseLocation";
        private const string ShowVeanenuKey = "ShowVeanenu";

        #endregion


        //public static string Language
        //{
        //    get
        //    {
        //        return _language;
        //    }
        //    set
        //    {
        //        _language = value;
        //        AppSettings.AddOrUpdateValue(LanguageKey, value?.Name ?? "he");
        //    }
        //}

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(LanguageKey, "he");
            }
            set
            {
                AppSettings.AddOrUpdateValue(LanguageKey, value ?? "he");
            }
        }

        public static Nusach Nusach
        {
            get
            {
                return (Nusach)AppSettings.GetValueOrDefault(NusachKey, (int)Nusach.Sfard);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NusachKey, (int)value);
            }
        }

        public static bool IsInIsrael
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsInIsraelKey, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsInIsraelKey, value);
            }
        }

        public static bool UseLocation
        {
            get
            {
                return AppSettings.GetValueOrDefault(UseLocationKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UseLocationKey, value);
            }
        }

        public static bool ShowVeanenu
        {
            get
            {
                return AppSettings.GetValueOrDefault(ShowVeanenuKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ShowVeanenuKey, value);
            }
        }

        public static string TimeCalcMethod
        {
            get
            {
                return AppSettings.GetValueOrDefault(TimeCalcMethodKey, "Gra");
            }
            set
            {
                AppSettings.AddOrUpdateValue(TimeCalcMethodKey, value);
            }
        }
    }
}
