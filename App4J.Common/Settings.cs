using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Globalization;

namespace App4J.Common
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
    }
}
