using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.Logging;
using PrayPal.Resources;
using PrayPal.Tools.Calendar;
using PrayPal.Tools.PrayerDirection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrayPal.Tools
{
    public class ToolsPageViewModel : ScreenPageViewModelBase, IContentPage
    {
        private readonly INavigationService _navigationService;

        public ToolsPageViewModel(INavigationService navigationService, ILogger<ToolsPageViewModel> logger)
            : base(AppResources.ToolsTitle, logger)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            ItemTappedCommand = new Command<ToolItemViewModel>(OnItemTappedExecuted);

            Items = new ObservableCollection<ToolItemViewModel>();
        }

        public ObservableCollection<ToolItemViewModel> Items { get; }

        public Command<ToolItemViewModel> ItemTappedCommand { get; }

        public Task GenerateContentAsync()
        {
            Items.Clear();
            Items.Add(new ToolItemViewModel("Calendar", AppResources.CalendarTitle, typeof(CalendarPageViewModel)));
            Items.Add(new ToolItemViewModel("PrayerDirection", AppResources.PrayingDirectionTitle, typeof(PrayerDirectionViewModel)));

            return Task.CompletedTask;
        }

        private async void OnItemTappedExecuted(ToolItemViewModel item)
        {
            if (item == null)
            {
                return;
            }

            Logger.LogInformation("Opened " + item.Name);
            Analytics.TrackEvent("Opened tool", Utils.AnalyticsProperty("tool", item.Name));

            await _navigationService.NavigateToAsync(item.ViewModelType.Name);
        }

    }
}
