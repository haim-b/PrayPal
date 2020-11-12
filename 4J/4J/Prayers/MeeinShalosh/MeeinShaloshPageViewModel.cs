using Plugin.Settings;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Content;
using PrayPal.Models;
using PrayPal.Resources;
using PrayPal.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zmanim.HebrewCalendar;

namespace PrayPal.Prayers.MeeinShalosh
{
    public class MeeinShaloshPageViewModel : PageViewModelBase, IContentPage
    {
        private string _basicTextFormat;


        private ParagraphModel _text;
        private readonly ITimeService _timeService;

        private string _andSeparator;

        private Task _contentGenerationTask;

        public MeeinShaloshPageViewModel(ITimeService timeService, IErrorReportingService errorReportingService)
            : base(AppResources.MeeinShaloshTitle, errorReportingService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

            _andSeparator = " " + AppResources.And;
        }

        public bool Mezonot
        {
            get { return GetValue(); }
            set
            {
                SetValue(value);
                CalculateBlessing();
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShowMezonotOfIsrael)));
            }
        }

        public bool MezonotIsrael
        {
            get { return GetValue(); }
            set
            {
                SetValue(value);
                CalculateBlessing();
            }
        }

        public bool ShowMezonotOfIsrael
        {
            get { return Settings.Nusach == Nusach.EdotMizrach && GetValue(nameof(Mezonot)); }
        }

        public bool Geffen
        {
            get { return GetValue(); }
            set
            {
                SetValue(value);
                CalculateBlessing();
            }
        }

        public bool GeffenIsrael
        {
            get { return GetValue(); }
            set
            {
                SetValue(value);
                CalculateBlessing();
            }
        }

        public bool Fruit
        {
            get { return GetValue(); }
            set
            {
                SetValue(value);
                CalculateBlessing();
            }
        }

        public bool FruitIsrael
        {
            get { return GetValue(); }
            set
            {
                SetValue(value);
                CalculateBlessing();
            }
        }

        public ParagraphModel Text1
        {
            get { return _text; }
            set
            {
                SetProperty(ref _text, value);
            }
        }

        private async void CalculateBlessing()
        {
            Task generationTask = _contentGenerationTask;

            if (generationTask != null)
            {
                await generationTask;
            }

            string part1 = string.Join(_andSeparator, GetPart1Texts());
            string part2 = string.Join(" ", GetPart2Texts());
            string part3 = Settings.Nusach == Nusach.EdotMizrach ? string.Join(" ", GetPart3Texts()) : null;

            if (string.IsNullOrEmpty(part1) && string.IsNullOrEmpty(part2))
            {
                Text1 = null;
            }
            else
            {
                Text1 = new ParagraphModel(string.Format(_basicTextFormat, part1, part2, part3));
            }
        }

        private IEnumerable<string> GetPart1Texts()
        {
            if (GetValue(nameof(Mezonot)))
            {
                yield return CommonPrayerTextProvider.Current.MeeinShaloshMichya1;
            }

            if (GetValue(nameof(Geffen)))
            {
                yield return CommonPrayerTextProvider.Current.MeeinShaloshGeffen1;
            }

            if (GetValue(nameof(Fruit)))
            {
                yield return CommonPrayerTextProvider.Current.MeeinShaloshFruit1;
            }
        }

        private IEnumerable<string> GetPart2Texts()
        {
            if (GetValue(nameof(Mezonot)))
            {
                yield return GetValue(nameof(MezonotIsrael)) ? CommonPrayerTextProvider.Current.MeeinShaloshMichya2Israel : CommonPrayerTextProvider.Current.MeeinShaloshMichya2;
            }

            if (GetValue(nameof(Geffen)))
            {
                yield return GetValue(nameof(GeffenIsrael)) ? CommonPrayerTextProvider.Current.MeeinShaloshGeffen2Israel : CommonPrayerTextProvider.Current.MeeinShaloshGeffen2;
            }

            if (GetValue(nameof(Fruit)))
            {
                yield return GetValue(nameof(FruitIsrael)) ? CommonPrayerTextProvider.Current.MeeinShaloshFruit2Israel : CommonPrayerTextProvider.Current.MeeinShaloshFruit2;
            }
        }

        private IEnumerable<string> GetPart3Texts()
        {
            if (GetValue(nameof(Mezonot)))
            {
                yield return GetValue(nameof(MezonotIsrael)) ? EdotHaMizrachPrayerTextProvider.Instance.MeeinShaloshMichya3Israel : EdotHaMizrachPrayerTextProvider.Instance.MeeinShaloshMichya3;
            }

            if (GetValue(nameof(Geffen)))
            {
                yield return GetValue(nameof(GeffenIsrael)) ? CommonPrayerTextProvider.Current.MeeinShaloshGeffen2Israel : CommonPrayerTextProvider.Current.MeeinShaloshGeffen2;
            }

            if (GetValue(nameof(Fruit)))
            {
                yield return GetValue(nameof(FruitIsrael)) ? CommonPrayerTextProvider.Current.MeeinShaloshFruit2Israel : CommonPrayerTextProvider.Current.MeeinShaloshFruit2;
            }
        }

        private bool GetValue([CallerMemberName] string propertyName = null)
        {
            return CrossSettings.Current.GetValueOrDefault(propertyName, false);
        }

        private void SetValue(bool value, [CallerMemberName] string propertyName = null)
        {
            CrossSettings.Current.AddOrUpdateValue(propertyName, value);
        }

        public async Task GenerateContentAsync()
        {
            Task existingTask = _contentGenerationTask;

            if (existingTask != null)
            {
                await existingTask;
            }

            _contentGenerationTask = Task.Run(async () =>
              {
                  JewishCalendar jc = (await _timeService.GetDayInfoAsync()).JewishCalendar;

                  int yomTovIndex = jc.YomTovIndex;

                  if (jc.RoshChodesh)
                  {
                      _basicTextFormat = string.Format(CommonPrayerTextProvider.Current.MeeinShalosh, "{0}", CommonPrayerTextProvider.Current.MeeinShaloshRoshChodesh, "{1}", "{2}");
                  }
                  else if (yomTovIndex == JewishCalendar.CHOL_HAMOED_PESACH)
                  {
                      _basicTextFormat = string.Format(CommonPrayerTextProvider.Current.MeeinShalosh, "{0}", CommonPrayerTextProvider.Current.MeeinShaloshPesach, "{1}");
                  }
                  else if (yomTovIndex == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTovIndex == JewishCalendar.HOSHANA_RABBA)
                  {
                      _basicTextFormat = string.Format(CommonPrayerTextProvider.Current.MeeinShalosh, "{0}", CommonPrayerTextProvider.Current.MeeinShaloshSukkot, "{1}");
                  }
                  else
                  {
                      _basicTextFormat = string.Format(CommonPrayerTextProvider.Current.MeeinShalosh, "{0}", string.Empty, "{1}", "{2}");
                  }

                  CalculateBlessing();

              });

            await _contentGenerationTask;
            _contentGenerationTask = null;
        }
    }
}
