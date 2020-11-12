using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PrayPal.Resources;

namespace PrayPal
{
    [ContentProperty(nameof(ResourceKey))]
    public class CustomResource : IMarkupExtension<string>
    {
        public string ResourceKey { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            return AppResources.ResourceManager.GetString(ResourceKey, new System.Globalization.CultureInfo(PrayPal.Common.Settings.Language));
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return this.ProvideValue(serviceProvider);
        }
    }
}
