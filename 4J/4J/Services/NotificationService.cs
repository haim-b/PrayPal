﻿using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrayPal.Services
{
    class NotificationService : INotificationService
    {
        public async Task ShowErrorMessageAsync(string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () => await App.Current.MainPage.DisplayAlert(AppShell.Current.Title, message, AppResources.MessageBoxOK));
        }

        public async Task ShowWarningAsync(string message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () => await App.Current.MainPage.DisplayAlert(AppShell.Current.Title, message, AppResources.MessageBoxOK));
        }
    }

    public interface INotificationService
    {
        Task ShowErrorMessageAsync(string message);

        Task ShowWarningAsync(string message);
    }
}
