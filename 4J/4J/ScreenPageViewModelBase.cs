using Microsoft.Extensions.Logging;
using PrayPal.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal
{
    public class ScreenPageViewModelBase : BindableBase, ISettingsListener
    {
        public ScreenPageViewModelBase(string title, ILogger logger)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Logger = logger;

            Settings.RegisterListener(this);
        }

        public string Title { get; }

        protected ILogger Logger { get; }

        async void ISettingsListener.OnSettingsChanged(string settingName)
        {
            try
            {
                await OnSettingsChangedAsync(settingName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed when refreshing after settings changed.");
            }
        }

        protected virtual Task OnSettingsChangedAsync(string settingsName)
        {
            return Task.CompletedTask;
        }
    }
}
