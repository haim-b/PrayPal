using CodeMill.VMFirstNav;
using Microsoft.Extensions.Logging;
using PrayPal.Common.Services;
using PrayPal.Content;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrayPal.TextPresenter
{
    [QueryProperty(nameof(TextName), "textName")]
    public class TextPresenterViewModel : BindableBase, IContentPage
    {
        private ITextDocument _textDocument;
        private string _textName;
        private readonly IEnumerable<Lazy<ITextDocument, PrayerMetadata>> _texts;
        private readonly ITimeService _timeService;
        private readonly ILogger _logger;

        public TextPresenterViewModel(IEnumerable<Lazy<ITextDocument, PrayerMetadata>> texts, ITimeService timeService, ILogger<TextPresenterViewModel> logger)
        {
            _texts = texts ?? throw new ArgumentNullException(nameof(texts));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string TextName
        {
            get { return _textName; }
            set { _textName = Uri.UnescapeDataString(value); }
        }

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
            var text = _texts.FirstOrDefault(t => t.Metadata.Name == _textName);

            if (text == null)
            {
                _logger.LogError("No text with name '{0}'.", _textName);
                return;
            }

            TextDocument = text.Value;

            await _textDocument.CreateAsync(await _timeService.GetDayInfoAsync(null, null, true), _logger);
        }
    }
}
