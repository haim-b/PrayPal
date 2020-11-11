using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace PrayPal.Common
{
    public static class Settings
    {
        private static readonly List<WeakReference<ISettingsListener>> _listeners = new List<WeakReference<ISettingsListener>>();
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
        private const string UseLightBackgroundKey = "UseLightBackground";
        private const string UseLargeFontKey = "UseLargeFont";

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
                OnSettingChanged();
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
                OnSettingChanged();
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
                OnSettingChanged();
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
                OnSettingChanged();
            }
        }

        public static bool UseLightBackground
        {
            get
            {
                return AppSettings.GetValueOrDefault(UseLightBackgroundKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UseLightBackgroundKey, value);
                OnSettingChanged();
            }
        }

        public static bool UseLargeFont
        {
            get
            {
                return AppSettings.GetValueOrDefault(UseLargeFontKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UseLargeFontKey, value);
                OnSettingChanged();
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
                OnSettingChanged();
            }
        }

        public static TimeCalcMethod TimeCalcMethod
        {
            get
            {
                return (TimeCalcMethod)AppSettings.GetValueOrDefault(TimeCalcMethodKey, (int)TimeCalcMethod.Gra);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TimeCalcMethodKey, (int)value);
                OnSettingChanged();
            }
        }

        public static void RegisterListener(ISettingsListener listener)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.Add(new WeakReference<ISettingsListener>(listener));
        }

        public static void UnregisterListener(ISettingsListener listener)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.RemoveAll(w => w.TryGetTarget(out var x) && object.ReferenceEquals(x, listener));
        }

        private static void OnSettingChanged([CallerMemberName] string settingName = null)
        {
            foreach (var listener in _listeners)
            {
                try
                {
                    if (listener.TryGetTarget(out var l))
                    {
                        l.OnSettingsChanged(settingName);
                    }
                }
                catch { }
            }
        }
    }

    public interface ISettingsListener
    {
        void OnSettingsChanged(string settingName);
    }
}
