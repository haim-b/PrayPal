using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    public static class PrayersHelper
    {
        private static readonly HebrewDateFormatter _psalmFormatter = new HebrewDateFormatter { UseGershGershayim = false, UseEndLetters = false };

        public static ParagraphModel GetYaalehVeYavo(DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            if (dayInfo.JewishCalendar.RoshChodesh)
            {
                return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.YaalehVeyavo, CommonPrayerTextProvider.Current.YaalehVeyavoRoshHodesh, false, true, AppResources.YaalehVeyavoRoshHodeshTitle, false);
            }

            int yomTov = dayInfo.YomTov;

            if (yomTov == JewishCalendar.SUCCOS || yomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || yomTov == JewishCalendar.HOSHANA_RABBA)
            {
                return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.YaalehVeyavo, CommonPrayerTextProvider.Current.YaalehVeyavoSukkot, false, true, AppResources.YaalehVeyavoSukkotTitle, false);
            }

            if (yomTov == JewishCalendar.PESACH || yomTov == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.YaalehVeyavo, CommonPrayerTextProvider.Current.YaalehVeyavoPesach, false, true, AppResources.YaalehVeyavoPesachTitle, false);
            }

            return null;
        }

        public static ParagraphModel CreateParagraphForStringFormat(string text, string argument, bool isHighlighted, bool isBold, string title, bool isCollapsible)
        {
            string[] texts = string.Format(text, "|").Split('|');

            if (texts.Length == 0)
            {
                return null;
            }

            ParagraphModel p = new ParagraphModel(title, texts[0]);
            p.IsCollapsible = isCollapsible;

            if (texts.Length == 1)
            {
                return p;
            }

            if (!string.IsNullOrEmpty(argument))
            {
                p.Add(new RunModel(argument, isHighlighted, isBold));
            }

            if (texts.Length > 1 && !string.IsNullOrEmpty(texts[1]))
            {
                p.Add(texts[1]);
            }
            return p;
        }

        public static ParagraphModel CreateParagraphForStringFormat(string format, params RunModel[] args)
        {
            return CreateParagraphForStringFormat(format, null, false, args);
        }

        public static ParagraphModel CreateParagraphForStringFormat(string format, string title, bool isCollapsible, params RunModel[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException($"'{nameof(format)}' cannot be null or empty", nameof(format));
            }

            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            List<object> texts = string.Format(format, "|").Split('|').ToList<object>();

            if (texts.Count == 0)
            {
                return null;
            }

            // Add arg placeholders:

            for (int i = 0; i < args.Length; i++)
            {
                texts.Insert(i + 1, args[i]);
            }

            return new ParagraphModel(title, GetRuns()) { IsCollapsible = isCollapsible };

            IEnumerable<RunModel> GetRuns()
            {
                foreach (object o in texts)
                {
                    if (o is null)
                    {
                        continue;
                    }
                    else if (o is RunModel r)
                    {
                        yield return r;
                        continue;
                    }

                    string s = (string)o;

                    if (!string.IsNullOrEmpty(s))
                    {
                        yield return new RunModel(s);
                    }
                }
            }
        }

        public static ParagraphModel GetOseShalom(DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.OseShalom, dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.OseShalomAseretYamim : CommonPrayerTextProvider.Current.OseShalomRegular, false, dayInfo.AseretYameyTshuva, null, false);
        }

        //public static IPrayer GetPrayer(string prayerName, Nusach nusach)
        //{
        //    if (string.IsNullOrEmpty(prayerName))
        //    {
        //        throw prayerName == null ? new ArgumentNullException("prayerName") : new ArgumentException("Value cannot be empty.", "prayerName");
        //    }

        //    Type iPrayerType = typeof(IPrayer);
        //    // Get all classes that implement the selected interface  
        //    Assembly asm = iPrayerType.GetTypeInfo().Assembly;

        //    IEnumerable<TypeInfo> subTypes = asm.DefinedTypes.Where(t => t.IsClass && t.ImplementedInterfaces.Any(i => i == iPrayerType));

        //    TypeInfo candidate = subTypes.Where(st => st.GetCustomAttributes<PrayerNameAttribute>().Any(att => att.PrayerName == prayerName) && st.GetCustomAttributes<NusachAttribute>().Any(att => att.Nusach == nusach)).FirstOrDefault();

        //    if (candidate != null)
        //    {
        //        return Activator.CreateInstance(candidate.AsType()) as IPrayer;
        //    }

        //    return null;
        //}

        public static void SetPrayerTextProvider(Nusach nusace)
        {
            CommonPrayerTextProvider textProvider;

            switch (nusace)
            {
                case Nusach.Ashkenaz:
                    textProvider = AshkenazPrayerTextProvider.Instance;
                    break;
                case Nusach.EdotMizrach:
                    textProvider = EdotHaMizrachPrayerTextProvider.Instance;
                    break;
                case Nusach.Baladi:
                    textProvider = new BaladiPrayerTextProvider();
                    break;
                case Nusach.Sfard:
                default:
                    textProvider = new SfardPrayerTextProvider();
                    break;
            }

            CommonPrayerTextProvider.Current = textProvider;
        }

        public static SpanModel GetPsalm(int number)
        {
            if (number <= 0 || number > 150)
            {
                throw new ArgumentOutOfRangeException("Number " + number + " is not valid for psalm verse.");
            }

            string hebrewNumber = _psalmFormatter.formatHebrewNumber(number);

            SpanModel span = new SpanModel(string.Format(AppResources.PsalmTitle, hebrewNumber));
            span.ShortTitle = hebrewNumber;
            span.Add(new ParagraphModel(Psalms.ResourceManager.GetString("Psalm" + number)));
            return span;
        }

        public static ParagraphModel GetHalfKadish(DayJewishInfo dayInfo, bool includeTitle = true, bool isCollapsible = true, bool isExpanded = false)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            ParagraphModel p = CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.HaziKadish, dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.KadishLeelaAYT : CommonPrayerTextProvider.Current.KadishLeelaRegular, false, dayInfo.AseretYameyTshuva, includeTitle ? AppResources.HaziKadishTitle : null, isCollapsible);
            p.IsExpanded = isExpanded;
            return p;
        }

        public static IEnumerable<ParagraphModel> GetFullKadish(DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            yield return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.KadishShalem, dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.KadishLeelaAYT : CommonPrayerTextProvider.Current.KadishLeelaRegular, false, dayInfo.AseretYameyTshuva, null, false);

            yield return GetOseShalom(dayInfo);
        }

        public static IEnumerable<ParagraphModel> GetKadishYatom(DayJewishInfo dayInfo, bool includeTitle = true)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            yield return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.KadishYatom, dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.KadishLeelaAYT : CommonPrayerTextProvider.Current.KadishLeelaRegular, false, dayInfo.AseretYameyTshuva, includeTitle ? AppResources.KadishYatomTitle : null, false);

            yield return GetOseShalom(dayInfo);
        }

        public static IEnumerable<ParagraphModel> GetKadishDerabanan(DayJewishInfo dayInfo, bool includeTitle = true)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            yield return CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.KadishDerabanan, dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.KadishLeelaAYT : CommonPrayerTextProvider.Current.KadishLeelaRegular, false, dayInfo.AseretYameyTshuva, includeTitle ? AppResources.KadishDerabananTitle : null, false);

            yield return GetOseShalom(dayInfo);
        }

        public static string GetNefilatApayim(Nusach nushach)
        {
            if (nushach == Nusach.Sfard || nushach == Nusach.Ashkenaz)
            {
                return CommonPrayerTextProvider.Current.NefilatApaim;
            }
            else if (nushach == Nusach.EdotMizrach)
            {
                return Psalms.Psalm25 + EdotHaMizrachPrayerTextProvider.Instance.NefilatApayimEnding;
            }

            return string.Empty;
        }

        public static bool ShouldSayLeDavid(this DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            return dayInfo.JewishCalendar.JewishMonth == 6 || (dayInfo.JewishCalendar.JewishMonth == 7 && dayInfo.JewishCalendar.JewishDayOfMonth <= 21);
        }

        public static bool IsTfillinTime(this DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            return /*!dayInfo.JewishCalendar.YomTov */ !dayInfo.JewishCalendar.CholHamoed && dayInfo.YomTov != JewishCalendar.HOSHANA_RABBA;
        }

        public static bool IsMussafDay(this DayJewishInfo dayInfo)
        {
            if (dayInfo == null)
            {
                throw new ArgumentNullException("dayInfo");
            }

            return dayInfo.JewishCalendar.RoshChodesh || dayInfo.JewishCalendar.CholHamoed || dayInfo.YomTov == JewishCalendar.HOSHANA_RABBA;
        }

        public static bool IsAfterYomKippur(this DayJewishInfo dayInfo)
        {
            JewishCalendar yesterday = dayInfo.JewishCalendar.CloneEx();
            yesterday.back();

            return yesterday.YomTovIndex == JewishCalendar.YOM_KIPPUR;
        }

    }
}
