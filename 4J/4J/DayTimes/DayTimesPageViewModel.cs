using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.DayTimes
{
    public class DayTimesPageViewModel : ScreenPageViewModelBase, IContentPage
    {
        public DayTimesPageViewModel(ILocationService locationService, ITimeService timeService, ILogger<DayTimesViewModel> baseLogger, ILogger<DayTimesPageViewModel> logger)
            : base(AppResources.ZmaneyHayom, logger)
        {
            DayTimesViewModel = new DayTimesViewModel(locationService, timeService, baseLogger)
            {
                ShowPrayersTime = true,
                ShowRelativePrayers = true,
                IncludeIsruChag = true
            };
        }

        protected override async Task OnSettingsChangedAsync(string settingsName)
        {
            if (!Settings.TimeAffecingSettings.Contains(settingsName))
            {
                return;
            }

            await GenerateContentAsync();
        }

        public async Task GenerateContentAsync()
        {
            await DayTimesViewModel.GenerateZmanim(null, null, null);
        }

        public DayTimesViewModel DayTimesViewModel { get; }
    }
}
