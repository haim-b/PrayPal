using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Content;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal
{
    public class PsalmsTextGenerator
    {
        private static readonly HebrewDateFormatter _verseFormatter = new HebrewDateFormatter() { UseGershGershayim = false, UseEndLetters = false };
        private static readonly HebrewDateFormatter _dayFormatter = new HebrewDateFormatter() { UseGershGershayim = true, UseEndLetters = false };

        //public static DocumentModel GetVerses(string typeName)
        //{
        //    if (string.IsNullOrEmpty(typeName))
        //    {
        //        return null;
        //    }

        //    if (typeName.StartsWith(PsalmsBook.TodayInWeekVersesSentinel))
        //    {
        //        int dayOfWeek;

        //        if (int.TryParse(typeName.Replace(PsalmsBook.TodayInWeekVersesSentinel, ""), out dayOfWeek))
        //        {
        //            return GetVersesTodayForWeek(dayOfWeek);
        //        }
        //        else
        //        {
        //            return GetVersesTodayForWeek();
        //        }
        //    }
        //    else if (typeName.StartsWith(PsalmsBook.TodayInMonthVersesSentinel))
        //    {
        //        int dayOfMonth;

        //        if (int.TryParse(typeName.Replace(PsalmsBook.TodayInMonthVersesSentinel, ""), out dayOfMonth))
        //        {
        //            return GetVersesTodayForMonth(dayOfMonth);
        //        }
        //        else
        //        {
        //            return GetVersesTodayForMonth();
        //        }
        //    }
        //    else if (typeName.StartsWith(PsalmsBook.VersesSentinel))
        //    {
        //        return GetAllVerses();
        //    }

        //    return null;
        //}

        public static Tuple<int, int> GetVerseRangeForWeekDay(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return new Tuple<int, int>(1, 29);
                case 2:
                    return new Tuple<int, int>(30, 50);
                case 3:
                    return new Tuple<int, int>(51, 72);
                case 4:
                    return new Tuple<int, int>(73, 89);
                case 5:
                    return new Tuple<int, int>(90, 106);
                case 6:
                    return new Tuple<int, int>(107, 119);
                case 7:
                    return new Tuple<int, int>(120, 150);
            }

            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }

        //public static Tuple<int, int> GetVerseRangeForMonthDay(int dayOfMonth)
        //{
        //    Tuple<int, int> result;

        //    if (_monthDayFromTo.TryGetValue((int)dayOfMonth, out result))
        //    {
        //        return result;
        //    }
        //    else if (dayOfMonth == 25)
        //    {
        //        result.Title = string.Format(AppResources.PsalmForMonthDayTitleFormat, _formatter.formatHebrewNumber(25));

        //        string title = string.Format(AppResources.PsalmVerseTitleFormat + ": {1}-{2}", _formatter.formatHebrewNumber(119), _formatter.formatHebrewNumber(1), _formatter.formatHebrewNumber(96));

        //        SpanModel p119 = new SpanModel(title, PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_1"));
        //        p.Add(p119);

        //        for (int i = 2; i <= 6; i++)
        //        {
        //            p119.Add(PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_" + i));
        //        }
        //    }
        //    else if (dayOfMonth == 26)
        //    {
        //        result.Title = string.Format(AppResources.PsalmForMonthDayTitleFormat, _formatter.formatHebrewNumber(26));

        //        string title = string.Format(AppResources.PsalmVerseTitleFormat + ": {1}-{2}", _formatter.formatHebrewNumber(119), _formatter.formatHebrewNumber(97), _formatter.formatHebrewNumber(176));

        //        SpanModel p119 = new SpanModel(title, PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_1"));
        //        p.Add(p119);

        //        for (int i = 7; i <= 11; i++)
        //        {
        //            p119.Add(PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_" + i));
        //        }
        //    }

        //    throw new ArgumentOutOfRangeException(nameof(dayOfMonth));
        //}

        public static DocumentModel<SpanModel> GetVersesTodayForWeek(int dayOfWeek)
        {
            DocumentModel<SpanModel> result = new DocumentModel<SpanModel>("PsalmToday");
            DateTimeFormatInfo fi = new CultureInfo("he-IL").DateTimeFormat;

            result.TrackContentGenerationTime = false;

            ICollection<SpanModel> p = new LinkedList<SpanModel>();

            switch (dayOfWeek)
            {
                case 1:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[0]);

                    for (int i = 1; i <= 29; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    break;
                case 2:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[1]);

                    for (int i = 30; i <= 50; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    break;
                case 3:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[2]);

                    for (int i = 51; i <= 72; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    break;
                case 4:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[3]);

                    for (int i = 73; i <= 89; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    break;
                case 5:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[4]);

                    for (int i = 90; i <= 106; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    break;
                case 6:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[5]);

                    for (int i = 107; i <= 118; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    SpanModel p119 = new SpanModel(string.Format(AppResources.PsalmVerseTitleFormat, _verseFormatter.formatHebrewNumber(119)), PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_1"));
                    p.Add(p119);

                    for (int i = 2; i <= 11; i++)
                    {
                        p119.Add(PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_" + i));
                    }

                    break;
                case 7:
                    result.Title = string.Format(AppResources.PsalmForTitleFormat, fi.DayNames[6]);

                    for (int i = 120; i <= 150; i++)
                    {
                        p.Add(GetPsalm(i));
                    }

                    break;
                default:
                    break;
            }

            result.SetTextsFactory(() => p.ToArray());
            return result;
        }

        public static DocumentModel<SpanModel> GetVersesTodayForMonth(JewishCalendar jewishMonth, int? dayOfMonth = null)
        {
            JewishCalendar jc = jewishMonth;

            if (dayOfMonth == null)
            {
                dayOfMonth = jc.JewishDayOfMonth;
            }

            Tuple<int, int> fromTo;

            DocumentModel<SpanModel> result = new DocumentModel<SpanModel>("PsalmTodayForMonth");

            result.TrackContentGenerationTime = false;

            ICollection<SpanModel> p = new List<SpanModel>(10);

            if (_monthDayFromTo.TryGetValue((int)dayOfMonth, out fromTo))
            {
                result.Title = string.Format(AppResources.PsalmForMonthDayTitleFormat, _dayFormatter.formatHebrewNumber((int)dayOfMonth));

                for (int i = fromTo.Item1; i <= fromTo.Item2; i++)
                {
                    p.Add(GetPsalm(i));
                }

                if (jc.DaysInJewishMonth == 29 && dayOfMonth == 29)
                {
                    fromTo = _monthDayFromTo[30];

                    for (int i = fromTo.Item1; i < fromTo.Item2; i++)
                    {
                        p.Add(GetPsalm(i));
                    }
                }
            }
            else if (dayOfMonth == 25)
            {
                result.Title = string.Format(AppResources.PsalmForMonthDayTitleFormat, _dayFormatter.formatHebrewNumber(25));

                string title = string.Format(AppResources.PsalmVerseTitleFormat + ": {1}-{2}", _verseFormatter.formatHebrewNumber(119), _verseFormatter.formatHebrewNumber(1), _verseFormatter.formatHebrewNumber(96));

                SpanModel p119 = new SpanModel(title, PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_1"));
                p.Add(p119);

                for (int i = 2; i <= 6; i++)
                {
                    p119.Add(PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_" + i));
                }
            }
            else if (dayOfMonth == 26)
            {
                result.Title = string.Format(AppResources.PsalmForMonthDayTitleFormat, _dayFormatter.formatHebrewNumber(26));

                string title = string.Format(AppResources.PsalmVerseTitleFormat + ": {1}-{2}", _verseFormatter.formatHebrewNumber(119), _verseFormatter.formatHebrewNumber(97), _verseFormatter.formatHebrewNumber(176));

                SpanModel p119 = new SpanModel(title, PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_1"));
                p.Add(p119);

                for (int i = 7; i <= 11; i++)
                {
                    p119.Add(PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_" + i));
                }
            }

            result.SetTextsFactory(() => p);

            return result;
        }



        public static DocumentModel<SpanModel> GetAllVerses()
        {
            DocumentModel<SpanModel> result = new DocumentModel<SpanModel>("PsalmToday");

            result.Title = AppResources.TehillimTitle;
            result.TrackContentGenerationTime = false;
            ICollection<SpanModel> p = new List<SpanModel>(170);

            for (int i = 1; i <= 150; i++)
            {
                if (i == 119)
                {
                    SpanModel p119 = new SpanModel(string.Format(AppResources.PsalmVerseTitleFormat, _verseFormatter.formatHebrewNumber(119)), PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_1"));
                    p.Add(p119);

                    for (int j = 2; j <= 11; j++)
                    {
                        p119.Add(PrayPal.Resources.Psalms.ResourceManager.GetString("Psalm119_" + j));
                    }
                }
                else
                {
                    p.Add(GetPsalm(i));
                }
            }

            result.SetTextsFactory(() => p.ToArray());
            return result;
        }

        private static SpanModel GetPsalm(int number)
        {
            return PrayersHelper.GetPsalm(number);
        }

        private static Dictionary<int, Tuple<int, int>> _monthDayFromTo = new Dictionary<int, Tuple<int, int>>
        {
            {1, new Tuple<int,int>(1, 9)},
            {2, new Tuple<int,int>(10, 17)},
            {3, new Tuple<int,int>(18, 22)},
            {4, new Tuple<int,int>(23, 28)},
            {5, new Tuple<int,int>(29, 34)},
            {6, new Tuple<int,int>(35, 38)},
            {7, new Tuple<int,int>(39, 43)},
            {8, new Tuple<int,int>(44, 48)},
            {9, new Tuple<int,int>(49, 54)},
            {10, new Tuple<int,int>(55, 59)},
            {11, new Tuple<int,int>(60, 65)},
            {12, new Tuple<int,int>(66, 68)},
            {13, new Tuple<int,int>(69, 71)},
            {14, new Tuple<int,int>(72, 76)},
            {15, new Tuple<int,int>(77, 78)},
            {16, new Tuple<int,int>(79, 82)},
            {17, new Tuple<int,int>(83, 87)},
            {18, new Tuple<int,int>(88, 89)},
            {19, new Tuple<int,int>(90, 96)},
            {20, new Tuple<int,int>(97, 103)},
            {21, new Tuple<int,int>(104, 105)},
            {22, new Tuple<int,int>(106, 107)},
            {23, new Tuple<int,int>(108, 112)},
            {24, new Tuple<int,int>(113, 118)},
            {27, new Tuple<int,int>(120, 134)},
            {28, new Tuple<int,int>(135, 139)},
            {29, new Tuple<int,int>(140, 144)},
            {30, new Tuple<int,int>(145, 150)}
        };
    }
}