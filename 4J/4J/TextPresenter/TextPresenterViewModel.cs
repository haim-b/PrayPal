using CodeMill.VMFirstNav;
using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Content;
using PrayPal.Models;
using PrayPal.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrayPal.TextPresenter
{
    [QueryProperty(nameof(TextName), TextNameParam)]
    [QueryProperty(nameof(TextParam), TextParamParam)]
    [QueryProperty(nameof(StartFromParagraphIndexValue), StartFromParagraphIndexParam)]
    public class TextPresenterViewModel : PageViewModelBase, IContentPage
    {
        public const string TextNameParam = "textName";
        public const string TextParamParam = "textParam";
        public const string StartFromParagraphIndexParam = "startFromParagraph";

        private ITextDocument _textDocument;
        private string _textName;
        private string _textParam;
        private readonly IEnumerable<Lazy<ITextDocument, PrayerMetadata>> _texts;
        private readonly ITimeService _timeService;
        private readonly ILogger _logger;

        public TextPresenterViewModel(IEnumerable<Lazy<ITextDocument, PrayerMetadata>> texts, ITimeService timeService, INotificationService notificationService, IErrorReportingService errorReportingService, ILogger<TextPresenterViewModel> logger)
            : base("a", notificationService, errorReportingService, logger)
        {
            _texts = texts ?? throw new ArgumentNullException(nameof(texts));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string TextName
        {
            get { return _textName; }
            set
            {
                _textName = Uri.UnescapeDataString(value);
            }
        }

        public string TextParam
        {
            get { return _textParam; }
            set
            {
                _textParam = Uri.UnescapeDataString(value);
            }
        }

        public string StartFromParagraphIndexValue
        {
            get { return StartFromParagraphIndex.ToString(); }
            set
            {
                StartFromParagraphIndex = int.Parse(value);
            }
        }

        public int StartFromParagraphIndex { get; private set; }

        public ITextDocument TextDocument
        {
            get { return _textDocument; }
            set
            {
                SetProperty(ref _textDocument, value);
            }
        }

        public async Task GenerateContentAsync()
        {
            var texts = _texts.Where(t => t.Metadata.Name == _textName && t.Metadata.Nusach?.Contains(Settings.Nusach) != false).ToList();

            if (texts.Count == 0)
            {
                _logger.LogError("No text with name '{0}'.", _textName);
                return;
            }
            else if (texts.Count > 1)
            {
                _logger.LogError("Found multiple texts for name '{0}' and nusach '{1}': {2}.", _textName, Settings.Nusach, string.Join(", ", texts.Select(t => t.Value.GetType().Name)));
                return;
            }

            var dayInfo = await _timeService.GetDayInfoAsync(null, null, true);

            Trace.WriteLine($"Day info: {dayInfo?.JewishCalendar?.JewishMonth.ToString() ?? "null"}.");

            var text = texts[0];

            if (text.Value is ICustomizedPrayer cp)
            {
                if (TextParam == null)
                {
                    // If the VM is a custom prayer, it needs a text param.
                    // If the text param wasn't set yet, don't generate the content yet:
                    return;
                }

                cp.ContentGenerationParameter = TextParam;
            }

            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    await text.Value.CreateAsync(dayInfo, _logger);
                }
                catch (NotificationException ne)
                {
                    await NotificationService.ShowWarningAsync(ne.Message);
                }
            }).Unwrap();

            Trace.WriteLine($"Number of spans: {(text.Value as PrayerBase<SpanModel>)?.Items?.Count.ToString() ?? "N/A"}.");

            TextDocument = text.Value;
        }
    }
}
