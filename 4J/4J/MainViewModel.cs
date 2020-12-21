using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using PrayPal.DayTimes;
using PrayPal.Prayers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PrayPal.AppSettings;
using PrayPal.Tools;
using PrayPal.Books;
using Xamarin.Essentials;
using PrayPal.SummaryView;
using Xamarin.Forms;
using PrayPal.About;

namespace PrayPal
{
    public class MainViewModel : BindableBase, IContentPage
    {
        private const string AppAlreadyRanKey = "AppAlreadyRan";
        private readonly INavigationService _navigationService;
        private readonly ILogger _logger;

        public MainViewModel(SummaryPageViewModel summary, PrayersViewModel prayers, BooksViewModel books, ToolsPageViewModel toolsPageViewModel, SettingsViewModel settings, INavigationService navigationService, ILogger<MainViewModel> logger)
        {
            Summary = summary ?? throw new ArgumentNullException(nameof(summary));
            Prayers = prayers ?? throw new ArgumentNullException(nameof(prayers));
            Books = books ?? throw new ArgumentNullException(nameof(books));
            Tools = toolsPageViewModel ?? throw new ArgumentNullException(nameof(toolsPageViewModel));
            Settings = settings;
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ShowAboutCommand = new Command(ShowAboutView);

            CurrentView = Summary;

            if (IsFirstTime)
            {
                CurrentView = Settings;
                IsFirstTime = false;
            }
        }

        public SummaryPageViewModel Summary { get; }

        public PrayersViewModel Prayers { get; }

        public BooksViewModel Books { get; }

        public ToolsPageViewModel Tools { get; }

        public SettingsViewModel Settings { get; }

        public object CurrentView { get; set; }

        public Command ShowAboutCommand { get; }

        private bool IsFirstTime
        {
            get { return !Preferences.Get(AppAlreadyRanKey, false); }
            set { Preferences.Set(AppAlreadyRanKey, !value); }
        }

        public async Task GenerateContentAsync()
        {
            await SafeInitializeViewModelAsync(Summary);
            await SafeInitializeViewModelAsync(Prayers);
            await SafeInitializeViewModelAsync(Books);
            await SafeInitializeViewModelAsync(Tools);
        }

        private async void ShowAboutView()
        {
            try
            {
                await _navigationService.NavigateToAsync(nameof(AboutViewModel));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to show the About view.");
            }
        }

        private async Task SafeInitializeViewModelAsync(IContentPage contentPage)
        {
            try
            {
                await contentPage.GenerateContentAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to initialize '{contentPage.GetType().Name}' view model.");
            }
        }
    }
}
