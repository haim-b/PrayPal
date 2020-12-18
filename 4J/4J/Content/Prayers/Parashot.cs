using Microsoft.Extensions.Logging;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    public static class Parashot
    {
        private static readonly List<ParashaMarks> _parashotToReadingMarks;

        private static readonly List<ParashaMarks> _cholHamoeldPesach;

        static Parashot()
        {
            _parashotToReadingMarks = new List<ParashaMarks>
            {
                new ParashaMarks(1,1,1,6,9,13), // Bereshit
                new ParashaMarks(1,6,9,17,20,22), // Noah
                new ParashaMarks(1,12,1,4,7,13), // Lech Lecha
                new ParashaMarks(1,18,1,6,9,14), // Vayera
                new ParashaMarks(1,23,1,6,11,16), // Chayey Sarah
                new ParashaMarks(1,25,19,23,27,5), // Toldot
                new ParashaMarks(1,28,10,13,18,22), // Vayetze
                new ParashaMarks(1,32,4,7,10,13), // Vayshlach
                new ParashaMarks(1,37,1,4,8,11), // Vayeshev
                new ParashaMarks(1,41,1,5,8,16), // Miketz
                new ParashaMarks(1,44,18,21,25,30), // Vaygash
                new ParashaMarks(1,47,28,1,4,9), // Vayechi

                new ParashaMarks(2,1,1,8,13,17), // Shmot
                new ParashaMarks(2,6,2,6,14,17), // Va'era
                new ParashaMarks(2,10,1,4,7,11), // Bo
                new ParashaMarks(2,13,17,1,5,8), // Beshalach
                new ParashaMarks(2,18,1,5,9,12), // Itro
                new ParashaMarks(2,21,1,4,7,19), // Mishpatim
                new ParashaMarks(2,25,1,6,10,15), // Truma
                new ParashaMarks(2,27,20,6,10,12), // Tetzaveh
                new ParashaMarks(2,30,11,14,17,21), // Ki Tisa
                new ParashaMarks(2,35,1,4,11,20), // Vayak'hel
                new ParashaMarks(2,38,21,24,28,1), // Pkudey

                new ParashaMarks(3,1,1,5,10,13), // Vaykra
                new ParashaMarks(3,6,1,4,7,11), // Tzav
                new ParashaMarks(3,9,1,7,11,16), // Shmini
                new ParashaMarks(3,12,1,5,1,5), // Tazria
                new ParashaMarks(3,14,1,6,10,12), // Metzora
                new ParashaMarks(3,16,1,7,12,17), // Achrey Mot
                new ParashaMarks(3,19,1,5,11,14), // Kdoshim
                new ParashaMarks(3,21,1,7,13,15), // Emor
                new ParashaMarks(3,25,1,4,8,13), // Behar
                new ParashaMarks(3,26,3,6,10,13), // Bechukotay

                new ParashaMarks(4,1,1,5,17,19), // Bamidbar
                new ParashaMarks(4,4,21,25,29,37), // Naso
                new ParashaMarks(4,8,1,5,10,14), // Beha'alotecha
                new ParashaMarks(4,13,1,4,17,20), // Shlach Lecha
                new ParashaMarks(4,16,1,4,8,13), // Korach
                new ParashaMarks(4,19,1,7,10,17), // Chukat
                new ParashaMarks(4,22,2,5,8,12), // Balak
                new ParashaMarks(4,25,10,13,16,4), // Pinchas
                new ParashaMarks(4,30,2,10,14,17), // Matot
                new ParashaMarks(4,33,1,4,7,10), // Masey
                
                new ParashaMarks(5,1,1,4,8,11), // Dvarim
                new ParashaMarks(5,3,23,26,5,8), // Va'etchanan
                new ParashaMarks(5,7,12,22,4,10), // Ekev
                new ParashaMarks(5,11,26,32,6,10), // Re'e
                new ParashaMarks(5,16,18,21,11,20), // Shoftim
                new ParashaMarks(5,21,10,15,18,7), // Ki Tetze
                new ParashaMarks(5,21,10,15,18,7), // Ki Tetze
                new ParashaMarks(5,21,10,15,18,7), // Ki Tetze
                new ParashaMarks(5,26,1,4,9,11), // Ki Tavo
                new ParashaMarks(5,29,9,12,15,28), // Nitzavim
                new ParashaMarks(5,31,1,4,7,13), // Vayelech
                new ParashaMarks(5,32,1,7,13,18), // Ha'azinu
                new ParashaMarks(5,33,1,8,13,17) // Vezot HaBracha
            };


            _cholHamoeldPesach = new List<ParashaMarks>
            {
                new ParashaMarks(3, 22, 26, 9, 15, 44),
                new ParashaMarks(2, 13, 1, 5, 11, 16),
                new ParashaMarks(2, 22, 24, 27, 6, 19),
                new ParashaMarks(2, 34, 1, 4, 18, 26),
                new ParashaMarks(4, 9, 1, 6, 9, 14),
            };
        }

        public static IEnumerable<ParagraphModel> GetParashaReadingForShacharit(JewishCalendar jc, Prayer prayer, ILogger logger)
        {
            if (jc is null)
            {
                throw new ArgumentNullException(nameof(jc));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (prayer != Prayer.Shacharit)
            {
                return Enumerable.Empty<ParagraphModel>();
            }

            if (jc.RoshChodesh)
            {
                var r = GetRoshChodesh();

                if (jc.Chanukah)
                {
                    var l = r.ToList();
                    l.Add(new ParagraphModel(string.Format(AppResources.InSecondBookAliyaTitle_F0, AppResources.FourthTorahReaderTitle), GetHanukkah(jc).SelectMany(p => p.Content)));
                    return l;
                }

                return r;
            }

            if (jc.YomTovIndex == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                return GetCholHaMoedPesach(jc, logger);
            }


            if (jc.YomTovIndex == JewishCalendar.CHOL_HAMOED_SUCCOS || jc.YomTovIndex == JewishCalendar.HOSHANA_RABBA)
            {
                return GetCholHaMoedSukkot(jc);
            }

            if (jc.Chanukah)
            {
                return GetHanukkah(jc);
            }

            return GetMondayAndThursday(jc);
        }

        private static IEnumerable<ParagraphModel> GetCholHaMoedPesach(JewishCalendar jc, ILogger logger)
        {
            ResourceManager book;

            int dayOfCholHaMoed = GetEffectiveCholHaMoedDay();

            if (dayOfCholHaMoed == -1)
            {
                logger.LogError($"Effective Chol HaMoed Pessach day could not be calculated on {jc}. In Israel: {jc.InIsrael}.");
                yield break;
            }

            ParashaMarks marks = _cholHamoeldPesach[dayOfCholHaMoed - 1];

            book = GetBook(marks.BookNumber);
            string[] chapter = GetChapter(book, marks.Chapter);

            foreach (var p in MarksToParagraphs(marks, chapter, null))
            {
                yield return p;
            }

            book = GetBook(4);
            chapter = GetChapter(book, 28);

            yield return new ParagraphModel(AppResources.FourthTorahReaderTitle, Flatten(GetPsukimByFirstAndLength(19, 7, chapter, null)));

            int GetEffectiveCholHaMoedDay()
            {
                int dayOfMonth = jc.JewishDayOfMonth + (jc.InIsrael ? 0 : 1);

                if (dayOfMonth == 16)
                {
                    return 1;
                }
                if (dayOfMonth == 17)
                {
                    return 2;
                }
                if (dayOfMonth == 18)
                {
                    if (jc.DayOfWeek == 1)
                    {
                        return 2;
                    }

                    return 3;
                }
                if (dayOfMonth == 19)
                {
                    if (jc.DayOfWeek == 2)
                    {
                        return 3;
                    }

                    return 4;
                }
                if (dayOfMonth == 20)
                {
                    return 5;
                }

                return -1;
            }
        }

        private static IEnumerable<ParagraphModel> GetCholHaMoedSukkot(JewishCalendar jc)
        {
            int firstPasuk = 17;
            int cholHaMoedDay = jc.JewishDayOfMonth - 15; //The 16th is the first day.

            if (!jc.InIsrael)
            {
                cholHaMoedDay--; // Abroad, Chol HaMoed starts a day later. So 17th is the 1st day, not the 2nd.
            }

            int pasukMultiplyingFactor = cholHaMoedDay - 1; // The first day should add 0 psukim, the 1st add 3, the 2nd add 6, 3rd add 9, and so on.


            ResourceManager book = GetBook(4);
            string[] chapter = GetChapter(book, 29);

            firstPasuk += 3 * pasukMultiplyingFactor;

            yield return new ParagraphModel(AppResources.For4TorahOlimTitle, Flatten(GetPsukimByFirstAndLength(firstPasuk, 3, chapter, null)));
        }

        private static IEnumerable<ParagraphModel> GetHanukkah(JewishCalendar jc)
        {
            int day = jc.DayOfChanukah;

            ResourceManager book = GetBook(4);
            Func<string[]> part1Factory = () => GetChapter(book, 6);
            string[] part2 = GetChapter(book, 7);
            Func<string[]> part3Factory = () => GetChapter(book, 8);

            string cohen, levi;

            switch (day)
            {
                case 1:
                    yield return new ParagraphModel(AppResources.CohenTitle, Flatten(GetPsukimByRange(22, 12, part1Factory(), () => part2)));
                    yield return new ParagraphModel(AppResources.LeviTitle, Flatten(GetPsukimByFirstAndLength(12, 3, part2, null)));
                    yield return new ParagraphModel(AppResources.ThirdTorahReaderTitle, Flatten(GetPsukimByFirstAndLength(15, 3, part2, null)));
                    yield break;
                case 2:
                    cohen = Flatten(GetPsukimByFirstAndLength(18, 3, part2, null));
                    levi = Flatten(GetPsukimByFirstAndLength(21, 3, part2, null));
                    break;
                case 3:
                    cohen = Flatten(GetPsukimByFirstAndLength(24, 3, part2, null));
                    levi = Flatten(GetPsukimByFirstAndLength(27, 3, part2, null));
                    break;
                case 4:
                    cohen = Flatten(GetPsukimByFirstAndLength(30, 3, part2, null));
                    levi = Flatten(GetPsukimByFirstAndLength(33, 3, part2, null));
                    break;
                case 5:
                    cohen = Flatten(GetPsukimByFirstAndLength(36, 3, part2, null));
                    levi = Flatten(GetPsukimByFirstAndLength(39, 3, part2, null));
                    break;
                case 6:
                    cohen = Flatten(GetPsukimByFirstAndLength(42, 3, part2, null));
                    levi = Flatten(GetPsukimByFirstAndLength(45, 3, part2, null));
                    break;
                case 7:
                    cohen = Flatten(GetPsukimByFirstAndLength(48, 3, part2, null));
                    levi = Flatten(GetPsukimByFirstAndLength(51, 3, part2, null));
                    break;
                case 8:
                    yield return new ParagraphModel(AppResources.CohenTitle, Flatten(GetPsukimByFirstAndLength(54, 3, part2, null)));
                    yield return new ParagraphModel(AppResources.LeviTitle, Flatten(GetPsukimByFirstAndLength(57, 3, part2, null)));
                    yield return new ParagraphModel(AppResources.ThirdTorahReaderTitle, Flatten(GetPsukimByRange(60, 5, part2, part3Factory)));
                    yield break;
                default:
                    yield break;
            }

            // on Day 2-7 of Hanukkah:
            yield return new ParagraphModel(AppResources.CohenTitle, cohen);
            yield return new ParagraphModel(AppResources.LeviTitle, levi);
            yield return new ParagraphModel(AppResources.ThirdTorahReaderTitle, Flatten(new[] { cohen, levi }));
        }

        private static IEnumerable<ParagraphModel> GetRoshChodesh()
        {
            ResourceManager book = GetBook(4);

            string[] chapter = GetChapter(book, 28);

            yield return new ParagraphModel(AppResources.CohenTitle, Flatten(GetPsukimByRange(1, 4, chapter, null)));
            yield return new ParagraphModel(AppResources.LeviTitle, Flatten(GetPsukimByRange(3, 6, chapter, null)));
            yield return new ParagraphModel(AppResources.IsraelTitle, Flatten(GetPsukimByRange(6, 11, chapter, null)));
            yield return new ParagraphModel(AppResources.FourthTorahReaderTitle, Flatten(GetPsukimByFirstAndLength(11, 5, chapter, null)));
        }

        private static IEnumerable<ParagraphModel> GetMondayAndThursday(JewishCalendar jc)
        {
            if (jc.DayOfWeek != 2 && jc.DayOfWeek != 5)
            {
                return Enumerable.Empty<ParagraphModel>();
            }

            if (jc.DayOfWeek < 7)
            {
                jc = jc.CloneEx();

                do
                {
                    jc.forward();
                } while (jc.DayOfWeek < 7);
            }

            int parashaIndex = jc.ParshaIndex;
            ParashaMarks marks = _parashotToReadingMarks[parashaIndex];

            ResourceManager book = GetBook(marks.BookNumber);

            string[] chapter = GetChapter(book, marks.Chapter);
            Func<string[]> nextChapterFactory = () => GetChapter(book, marks.Chapter + 1);

            return MarksToParagraphs(marks, chapter, nextChapterFactory);
        }

        private static IEnumerable<ParagraphModel> MarksToParagraphs(ParashaMarks marks, string[] chapter, Func<string[]> nextChapterFactory)
        {
            yield return new ParagraphModel(AppResources.CohenTitle, Flatten(GetPsukimByRange(marks.FirstReaderStart, marks.SecondReaderStart, chapter, nextChapterFactory)));
            yield return new ParagraphModel(AppResources.LeviTitle, Flatten(GetPsukimByRange(marks.SecondReaderStart, marks.ThirdReaderStart, chapter, nextChapterFactory)));
            yield return new ParagraphModel(AppResources.IsraelTitle, Flatten(GetPsukimByFirstAndLength(marks.ThirdReaderStart, marks.ThirdReaderLength, chapter, nextChapterFactory)));
        }

        private static string Flatten(IEnumerable<string> psukim)
        {
            return string.Join(" ", psukim);
        }

        private static string[] GetChapter(ResourceManager book, int chapter)
        {
            return book.GetString("Chapter" + chapter).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static IEnumerable<string> GetPsukimByRange(int firstReaderStart, int secondReaderStart, string[] chapter, Func<string[]> nextChapterFactory)
        {
            int count = secondReaderStart - firstReaderStart;

            if (count <= 0)
            {
                count = (chapter.Length - firstReaderStart) + secondReaderStart;
            }

            return GetPsukimByFirstAndLength(firstReaderStart, count, chapter, nextChapterFactory);
        }

        private static IEnumerable<string> GetPsukimByFirstAndLength(int firstReaderStart, int count, string[] chapter, Func<string[]> nextChapterFactory)
        {
            int start = firstReaderStart - 1;

            if (firstReaderStart + count <= chapter.Length) // All in the same chapter
            {
                for (int i = start; i < start + count; i++)
                {
                    yield return chapter[i];
                }

                yield break;
            }
            else
            {
                for (int i = start; i < chapter.Length; i++)
                {
                    yield return chapter[i];
                }

                string[] nextChapter = nextChapterFactory();

                for (int i = 0; i < count - (chapter.Length - firstReaderStart); i++)
                {
                    yield return nextChapter[i];
                }
            }
        }

        private static ResourceManager GetBook(int bookNumber)
        {
            switch (bookNumber)
            {
                case 1:
                    return Genesis.ResourceManager;
                case 2:
                    return Exodus.ResourceManager;
                case 3:
                    return Leviticus.ResourceManager;
                case 4:
                    return Numbers.ResourceManager;
                case 5:
                    return Deuteronomy.ResourceManager;
                default:
                    throw new Exception($"Invalid Torah book number {bookNumber}.");
            }
        }

        private class ParashaMarks
        {
            public readonly int BookNumber;
            public readonly int Chapter;
            public readonly int FirstReaderStart;
            public readonly int SecondReaderStart;
            public readonly int ThirdReaderStart;
            public readonly int ThirdReaderLength;

            public ParashaMarks(int bookNumber, int chapter, int firstReaderStart, int secondReaderStart, int thirdReaderStart, int thirdReaderLast)
            {
                BookNumber = bookNumber;
                Chapter = chapter;
                FirstReaderStart = firstReaderStart;
                SecondReaderStart = secondReaderStart;
                ThirdReaderStart = thirdReaderStart;
                ThirdReaderLength = thirdReaderLast - thirdReaderStart + 1;
            }
        }

        private class PasukMark
        {
            public readonly int Chapter;
            public readonly int Pasuk;

            public PasukMark(int chapter, int pasuk)
            {
                Chapter = chapter;
                Pasuk = pasuk;
            }
        }
    }
}
