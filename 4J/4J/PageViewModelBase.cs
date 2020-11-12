using PrayPal.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    public class PageViewModelBase : BindableBase
    {
        protected readonly IErrorReportingService _errorReportingService;

        public PageViewModelBase(string title, IErrorReportingService errorReportingService)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
            }

            if (errorReportingService == null)
            {
                throw new ArgumentNullException(nameof(errorReportingService));
            }

            Title = title;
            _errorReportingService = errorReportingService;
            ReportErrorCommand = new Command(ExecuteReportErrorCommand);
        }

        public string Title { get; }

        public Command ReportErrorCommand { get; }

        protected virtual void ExecuteReportErrorCommand()
        {
            _errorReportingService.ReportIssueAsync(Title);
        }
    }
}
