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

namespace PrayPal
{
    public class MainViewModel : BindableBase, IContentPage
    {
        private const string AppAlreadyRanKey = "AppAlreadyRan";

        private readonly ILogger _logger;

        public MainViewModel(DayTimesPageViewModel dayTimes, PrayersViewModel prayers, BooksViewModel books, ToolsPageViewModel toolsPageViewModel, SettingsViewModel settings, ILogger<MainViewModel> logger)
        {
            DayTimes = dayTimes ?? throw new ArgumentNullException(nameof(dayTimes));
            Prayers = prayers ?? throw new ArgumentNullException(nameof(prayers));
            Books = books ?? throw new ArgumentNullException(nameof(books));
            Tools = toolsPageViewModel ?? throw new ArgumentNullException(nameof(toolsPageViewModel));
            Settings = settings;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (IsFirstTime)
            {
                CurrentView = Settings;
                IsFirstTime = false;
            }
        }

        public DayTimesPageViewModel DayTimes { get; }

        public PrayersViewModel Prayers { get; }

        public BooksViewModel Books { get; }

        public ToolsPageViewModel Tools { get; }

        public SettingsViewModel Settings { get; }

        public object CurrentView { get; set; }

        private bool IsFirstTime
        {
            get { return !Preferences.Get(AppAlreadyRanKey, false); }
            set { Preferences.Set(AppAlreadyRanKey, !value); }
        }

        public async Task GenerateContentAsync()
        {
            await SafeInitializeViewModelAsync(DayTimes);
            await SafeInitializeViewModelAsync(Prayers);
            await SafeInitializeViewModelAsync(Books);
            await SafeInitializeViewModelAsync(Tools);
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
