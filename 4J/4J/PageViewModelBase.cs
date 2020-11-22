using Microsoft.Extensions.Logging;
using PrayPal.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PrayPal
{
    public class PageViewModelBase : BindableBase
    {
        public PageViewModelBase(string title, INotificationService notificationService, IErrorReportingService errorReportingService, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
            }

            Title = title;
            NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            ErrorReportingService = errorReportingService ?? throw new ArgumentNullException(nameof(errorReportingService));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ReportErrorCommand = new Command(ExecuteReportErrorCommand);
        }

        public string Title { get; }

        public Command ReportErrorCommand { get; }

        protected INotificationService NotificationService { get; }

        protected IErrorReportingService ErrorReportingService { get; }

        protected ILogger Logger { get; }

        protected virtual void ExecuteReportErrorCommand()
        {
            ErrorReportingService.ReportIssueAsync(Title);
        }
    }
}
