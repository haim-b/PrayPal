using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Autofac;
using Xamarin.Forms;
using System.Threading.Tasks;
using PrayPal.Common.Services;
using PrayPal.Common;

namespace PrayPal
{
    internal static class Utils
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> WithCorrectMetadataFrom<TAttribute>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            Type attrType = typeof(TAttribute);
            IEnumerable<PropertyInfo> metadataProperties = from pi in RuntimeReflectionExtensions.GetRuntimeProperties(attrType)
                                                           where pi.CanRead && pi.Name != nameof(Attribute.TypeId)
                                                           select pi;

            return registration.WithMetadata(delegate (Type t)
            {
                List<TAttribute> list = t.GetCustomAttributes(inherit: true).OfType<TAttribute>().ToList();

                if (list.Count == 0)
                {
                    //throw new ArgumentException("No metadata attributes found.");
                    return metadataProperties.Select(p => new KeyValuePair<string, object>(p.Name, p.PropertyType.IsClass ? null : Activator.CreateInstance(p.PropertyType)));
                }
                if (list.Count != 1)
                {
                    throw new ArgumentException("More than one metadata attribute match.");
                }

                TAttribute attr = list[0];
                return metadataProperties.Select(p => GenerateMetadata(p, attr));
            });
        }

        private static KeyValuePair<string, object> GenerateMetadata<TAttribute>(PropertyInfo p, TAttribute attr)
        {
            object value = p.GetValue(attr, null);
            return new KeyValuePair<string, object>(p.Name, value);
        }

        public static double GetFontSize(bool largeFont)
        {
            //if (largeFont)
            //{
            //    return Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //}

            //return Device.GetNamedSize(NamedSize.Default, typeof(Label));

            if (App.Current == null)
            {
                return largeFont ? 22 : 18;
            }

            return largeFont ? (double)App.Current.Resources["TextLargeFontSize"] : (double)App.Current.Resources["TextNormalFontSize"];
        }

        public static double GetTitleFontSize(bool largeFont)
        {
            if (App.Current == null)
            {
                return largeFont ? 22 : 18;
            }

            if (largeFont)
            {
                return Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            }

            return Device.GetNamedSize(NamedSize.Caption, typeof(Label));
        }

        public static IDictionary<string, string> AnalyticsProperty(string name, string value)
        {
            return new Dictionary<string, string> { { name, value } };
        }


        public static async Task<PrayerInfo> GetNextPrayerAsync(this ITimeService timeService, Geoposition position = null, DateTime? relativeToDate = null)
        {
            if (timeService is null)
            {
                throw new ArgumentNullException(nameof(timeService));
            }

            PrayerInfo[] prayers = HebDateHelper.GetPrayersInfo(position, relativeToDate, relativeToDate != null);

            foreach (PrayerInfo prayer in prayers)
            {
                if (prayer.Start <= relativeToDate && relativeToDate <= prayer.End)
                {
                    return prayer;
                }
            }

            return null;

            //DateTime now = relativeToDate ?? DateTime.Now;

            //IEnumerable<Task<PrayerInfo>> prayers = IteratePrayers();

            //foreach (var prayerTask in prayers)
            //{
            //    var prayer = await prayerTask;

            //    if (prayer.Start <= now && now <= prayer.End)
            //    {
            //        return prayer;
            //    }
            //}

            //return null;

            //IEnumerable<Task<PrayerInfo>> IteratePrayers()
            //{
            //    yield return timeService.GetShacharitInfoAsync(position, relativeToDate);
            //    yield return timeService.GetMinchaInfoAsync();
            //    yield return timeService.GetArvitInfoAsync();
            //}
        }
    }
}
