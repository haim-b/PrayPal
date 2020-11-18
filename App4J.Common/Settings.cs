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
        private static ISettingsProvider _settingsProvider;

        static Settings()
        {
            //string langauge = AppSettings.GetValue(LanguageKey, null);

            //if (langauge == null)
            //{
            //    langauge = "he";
            //}

            //Language = new CultureInfo(langauge);
        }

        private static ISettingsProvider AppSettings
        {
            get
            {
                if (_settingsProvider == null)
                {
                    throw new InvalidOperationException("A settings provider was not set.");
                }

                return _settingsProvider;
            }
        }

        public static void SetSettingsProvider(ISettingsProvider settingsProvider)
        {
            if (settingsProvider is null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            _settingsProvider = settingsProvider;
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
        //        AppSettings.SetValue(LanguageKey, value?.Name ?? "he");
        //    }
        //}

        public static string Language
        {
            get
            {
                return AppSettings.GetValue(LanguageKey, "he");
            }
            set
            {
                AppSettings.SetValue(LanguageKey, value ?? "he");
                OnSettingChanged();
            }
        }

        public static Nusach Nusach
        {
            get
            {
                return (Nusach)AppSettings.GetValue(NusachKey, (int)Nusach.Sfard);
            }
            set
            {
                AppSettings.SetValue(NusachKey, (int)value);
                OnSettingChanged();
            }
        }

        public static bool IsInIsrael
        {
            get
            {
                return AppSettings.GetValue(IsInIsraelKey, true);
            }
            set
            {
                AppSettings.SetValue(IsInIsraelKey, value);
                OnSettingChanged();
            }
        }

        public static bool UseLocation
        {
            get
            {
                return AppSettings.GetValue(UseLocationKey, false);
            }
            set
            {
                AppSettings.SetValue(UseLocationKey, value);
                OnSettingChanged();
            }
        }

        public static bool UseLightBackground
        {
            get
            {
                return AppSettings.GetValue(UseLightBackgroundKey, false);
            }
            set
            {
                AppSettings.SetValue(UseLightBackgroundKey, value);
                OnSettingChanged();
            }
        }

        public static bool UseLargeFont
        {
            get
            {
                return AppSettings.GetValue(UseLargeFontKey, false);
            }
            set
            {
                AppSettings.SetValue(UseLargeFontKey, value);
                OnSettingChanged();
            }
        }

        public static bool ShowVeanenu
        {
            get
            {
                return AppSettings.GetValue(ShowVeanenuKey, false);
            }
            set
            {
                AppSettings.SetValue(ShowVeanenuKey, value);
                OnSettingChanged();
            }
        }

        public static TimeCalcMethod TimeCalcMethod
        {
            get
            {
                return (TimeCalcMethod)AppSettings.GetValue(TimeCalcMethodKey, (int)TimeCalcMethod.Gra);
            }
            set
            {
                AppSettings.SetValue(TimeCalcMethodKey, (int)value);
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
