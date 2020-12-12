using PrayPal.Models;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace PrayPal.Content.Brachot
{
    public static class Parashot
    {
        private static readonly List<ParashaMarks> _parashotToReadingMarks;

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
        }

        //public static IEnumerable<ParagraphModel> GetParashaReadingForShacharit(int parashaIndex)
        //{
        //    ParashaMarks marks = _parashotToReadingMarks[parashaIndex];

        //    ResourceManager book = GetBook(marks.BookNumber);

        //    string[] chapter = book.GetString("Chapter" + marks.Chapter).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

        //    yield return new ParagraphModel(AppResources.CohenTitle, string.Join(" ", GetPsukimRange(marks.FirstReaderStart, marks.SecondReaderStart).Select(i => chapter[i])));
        //    yield return new ParagraphModel(AppResources.LeviTitle, string.Join(" ", GetPsukimRange(marks.SecondReaderStart, marks.ThirdReaderStart).Select(i => chapter[i])));
        //    yield return new ParagraphModel(AppResources.IsraelTitle, string.Join(" ", Enumerable.Range(marks.ThirdReaderStart.Pasuk - 1, marks.ThirdReaderLength).Select(i => chapter[i])));
        //}

        private static IEnumerable<int> GetPsukimRange(PasukMark firstReaderStart, PasukMark secondReaderStart)
        {
            int start = firstReaderStart.Pasuk - 1;
            int count = secondReaderStart.Pasuk - firstReaderStart.Pasuk;

            return Enumerable.Range(start, count);
        }

        private static ResourceManager GetBook(int bookNumber)
        {
            switch (bookNumber)
            {
                case 1:
                    return Genesis.ResourceManager;
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
