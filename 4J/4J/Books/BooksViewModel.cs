using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Prayers;
using PrayPal.Resources;
using PrayPal.TextPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrayPal.Books
{
    public class BooksViewModel : ScreenPageViewModelBase, IContentPage
    {
        private readonly INavigationService _navigationService;

        public BooksViewModel(INavigationService navigationService, ILogger<BooksViewModel> logger)
            : base(AppResources.BooksTitle, logger)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            Items = new ObservableCollection<PrayerItemViewModel>();

            ItemTappedCommand = new Command<PrayerItemViewModel>(OnItemTappedExecuted);
        }

        public ObservableCollection<PrayerItemViewModel> Items { get; }

        public Command<PrayerItemViewModel> ItemTappedCommand { get; }

        public Task GenerateContentAsync()
        {
            Items.Clear();
            Items.Add(new PrayerItemViewModel(BookNames.Psalms, AppResources.TehillimTitle, typeof(Psalms.PsalmSelectionPageViewModel)));
            Items.Add(new PrayerItemViewModel(BookNames.IggerretHaramban, AppResources.IgerretHarambanTitle));

            return Task.CompletedTask;
        }


        private async void OnItemTappedExecuted(PrayerItemViewModel item)
        {
            if (item == null)
            {
                return;
            }

            Logger.LogInformation($"Opened " + item.PageName);
            Analytics.TrackEvent("Opened book", Utils.AnalyticsProperty("book", item.PageName));

            if (item.ViewModelType != null)
            {
                await _navigationService.NavigateToAsync(item.ViewModelType.Name);
                return;
            }

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel), TextPresenterViewModel.TextNameParam, item.PageName);
        }

    }
}
