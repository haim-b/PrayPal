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
            Items.Add(new PrayerItemViewModel(BookNames.Psalms, AppResources.TehillimTitle));
            Items.Add(new PrayerItemViewModel(BookNames.IggerretHaramban, AppResources.IgerretHarambanTitle));

            return Task.CompletedTask;
        }


        private async void OnItemTappedExecuted(PrayerItemViewModel item)
        {
            if (item.ViewModelType != null)
            {
                await _navigationService.NavigateToAsync(item.ViewModelType.Name);
                return;
            }

            await _navigationService.NavigateToAsync(nameof(TextPresenterViewModel), "textName", item.PageName);
        }

    }
}
