using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrayPal.Services
{
    public interface IErrorReportingService
    {
        Task ReportIssueAsync(string subject);
    }

    public class ErrorReportingService : IErrorReportingService
    {
        public async Task ReportIssueAsync(string subject)
        {
            EmailMessage message = new EmailMessage();
            message.Subject = subject;
            message.To.Add(AppResources.EmailAddress);
            //message.Attachments.Add(new EmailAttachment());
            //await message.sem EmailManager.ShowComposeNewEmailAsync(message);
        }
    }
}
