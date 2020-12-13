using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    [Nusach(Nusach.EdotMizrach)]
    public class ShacharitEdotHaMizrach : ShacharitBase
    {
        public ShacharitEdotHaMizrach(IPermissionsService permissionsService)
            : base(permissionsService)
        { }

        protected override async Task CreateOverrideAsync()
        {
            // Sedder Hashkama
            SpanModel hashkama = new SpanModel(AppResources.SederHashkamaTitle, EdotHaMizrachPrayerTextProvider.Instance.GiluyDaat, CommonPrayerTextProvider.Current.ModeAni);

            hashkama.Add(new ParagraphModel(AppResources.MorningNetilatYadayimInstruction + ":" + Environment.NewLine + CommonPrayerTextProvider.Current.NetilatYadayimBlessing));
            hashkama.Add(new ParagraphModel(AppResources.AsherYatzarTitle, CommonPrayerTextProvider.Current.AsherYatzar));
            hashkama.Add(new ParagraphModel(CommonPrayerTextProvider.Current.BirkatNeshama));// { Title2 = AppResources.BirkatNeshamaTitle });

            _items.Add(hashkama);

            // Birkot HaShachar
            Add(AppResources.BirkotHashacharTitle, DayInfo.YomTov == JewishCalendar.TISHA_BEAV ? EdotHaMizrachPrayerTextProvider.Instance.BirkotHashachar1Av9th : CommonPrayerTextProvider.Current.BirkotHashachar1, CommonPrayerTextProvider.Current.BirkotHashachar2, CommonPrayerTextProvider.Current.BirkotHashachar3, CommonPrayerTextProvider.Current.BirkotHashachar4);

            // Birkot HaTorah
            Add(AppResources.BirkotHatorahTitle, CommonPrayerTextProvider.Current.BirkotHatorah, CommonPrayerTextProvider.Current.ParashatBirkatCohanim);

            // Patach Elijahu
            Add(AppResources.PatachEliyahuTitle, EdotHaMizrachPrayerTextProvider.Instance.PatachEliyahu1, EdotHaMizrachPrayerTextProvider.Instance.PatachEliyahu2,
                EdotHaMizrachPrayerTextProvider.Instance.PatachEliyahu3, EdotHaMizrachPrayerTextProvider.Instance.PatachEliyahu4, EdotHaMizrachPrayerTextProvider.Instance.PatachEliyahu5,
                EdotHaMizrachPrayerTextProvider.Instance.PatachEliyahu6);

            SpanModel talitTfillin;

            // Talit and Tfillin
            if (DayInfo.IsTfillinTime())
            {
                talitTfillin = new SpanModel(AppResources.TalitAndTfillinTitle);
                talitTfillin.Add(new ParagraphModel(AppResources.AtifatTalitTitle, CommonPrayerTextProvider.Current.AtifatTalit));

                talitTfillin.Add(new ParagraphModel(AppResources.HanachatTfillinTitle, CommonPrayerTextProvider.Current.HanachatTfillin));
                talitTfillin.Add(EdotHaMizrachPrayerTextProvider.Instance.HanachatTfillin2);
                await AddTfillinMirrorAsync(talitTfillin);
                talitTfillin.Add(CommonPrayerTextProvider.Current.ParashahAfterTfillin1, CommonPrayerTextProvider.Current.ParashahAfterTfillin2);
            }
            else
            {
                talitTfillin = new SpanModel(AppResources.AtifatTalitTitle, CommonPrayerTextProvider.Current.AtifatTalit);
            }

            _items.Add(talitTfillin);

            // Vatitpallel Channah
            Add(AppResources.VatitpallelChannahTitle, EdotHaMizrachPrayerTextProvider.Instance.VatitpalelChannah, EdotHaMizrachPrayerTextProvider.Instance.PreShacharit1);
            Add(AppResources.LeshemYichudTitle, EdotHaMizrachPrayerTextProvider.Instance.PreShacharit2);

            // Parashat HaAkedah
            Add(AppResources.ParashatHaakedahTitle, EdotHaMizrachPrayerTextProvider.Instance.PreShacharit3,
            EdotHaMizrachPrayerTextProvider.Instance.ParashatHaakedah,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit4,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit5,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit6,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit7,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit8,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit9,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit10,
            EdotHaMizrachPrayerTextProvider.Instance.AnnaBechoach,
            EdotHaMizrachPrayerTextProvider.Instance.PreShacharit11);

            //UNDONE: Add לכן יהי רצון מלפניך ה אלוקינו...
            //UNDONE: Add פרק איזהו מקומן

            Add(AppResources.KtoretHasamimTitle, EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim1, EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim2,
                CommonPrayerTextProvider.Current.PitumHaktoret1, CommonPrayerTextProvider.Current.PitumHaktoret2);

            // Braita DeRabbi Ishmael
            SpanModel bdi = new SpanModel(AppResources.BrayitaDerabiYishmaelTitle, CommonPrayerTextProvider.Current.BrayitaDerabiYishmael);
            bdi.AddRange(PrayersHelper.GetKadishDerabanan(DayInfo));

            _items.Add(bdi);

            // Tfilot HaShachar
            SpanModel shachar = new SpanModel(AppResources.TfilotHashacharTitle);
            shachar.Add(CommonPrayerTextProvider.Current.MizmorLifneyHaAron1, CommonPrayerTextProvider.Current.MizmorLifneyHaAron2, CommonPrayerTextProvider.Current.MizmorLifneyHaAron3, EdotHaMizrachPrayerTextProvider.Instance.Psalm30Shacharit);

            shachar.Add(string.Format(CommonPrayerTextProvider.Current.ShacharitVerse1, DayInfo.AseretYameyTshuva ? EdotHaMizrachPrayerTextProvider.Instance.ShacharitVerse1AYT : ""), Psalms.Psalm67);

            _items.Add(shachar);


            // Psukey DeZimra
            Add(AppResources.PsukeyDezimraTitle, CommonPrayerTextProvider.Current.BaruchSheamar, Psalms.Psalm100,
            CommonPrayerTextProvider.Current.ShacharitVerse2, CommonPrayerTextProvider.Current.Ashrey, Psalms.Psalm146, Psalms.Psalm147, Psalms.Psalm148, Psalms.Psalm149, Psalms.Psalm150,
                Psalms.PsalmEnding, CommonPrayerTextProvider.Current.ShacharitVerse3, CommonPrayerTextProvider.Current.ShacharitVerse4, CommonPrayerTextProvider.Current.ShacharitVerse5);


            // Shirat Hayam and Yishtabach
            Add(AppResources.ShiratHayamTitle, CommonPrayerTextProvider.Current.ShiratHayam);

            SpanModel yishtabach = new SpanModel(AppResources.YishtabachTitle, CommonPrayerTextProvider.Current.Yishtabach);

            if (DayInfo.AseretYameyTshuva)
            {
                yishtabach.Add(Psalms.Psalm130);
            }

            yishtabach.Add(PrayersHelper.GetHalfKadish(DayInfo));

            _items.Add(yishtabach);

            // Brachot Shma
            Add(AppResources.BrachotKriatShmaTitle, CommonPrayerTextProvider.Current.Barchu, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit1, CommonPrayerTextProvider.Current.KadoshKadosh, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit2, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit3);

            // Kriat Shma
            Add(AppResources.KriatShmaTitle, CommonPrayerTextProvider.Current.KriatShma1, CommonPrayerTextProvider.Current.KriatShma2, CommonPrayerTextProvider.Current.KriatShma3,
                CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit1, CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit2, CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit3);

            // Shmone Esre
            SpanModel lastSpan = await AddShmoneEsre(Prayer.Shacharit);

            ///תחנון
            lastSpan = AddTachanun() ?? lastSpan;

            ///הלל
            lastSpan = AddHallel() ?? lastSpan;

            bool hasMussaf = PrayersHelper.IsMussafDay(DayInfo);

            if (!hasMussaf)
            {
                lastSpan.Add(PrayersHelper.GetHalfKadish(DayInfo));
            }
            else
            {
                AddFullKadish();
            }

            ///קריאת התורה
            bool torahReadingAdded = AddTorahReading();

            ///אשרי
            Add(AppResources.AshreyTitle, EdotHaMizrachPrayerTextProvider.Instance.YehiChasdecha, CommonPrayerTextProvider.Current.Ashrey);

            ///למנצח, ובא לציון
            SpanModel kdushaDesidra = new SpanModel(AppResources.KdushaDesidraTitle);

            if (DayInfo.IsTachanunDay(GetNusach()))
            {
                kdushaDesidra.Add(Psalms.Psalm20);
            }

            if (DayInfo.YomTov != JewishCalendar.TISHA_BEAV)
            {
                kdushaDesidra.Add(CommonPrayerTextProvider.Current.UvaLetzion);
            }

            kdushaDesidra.Add(CommonPrayerTextProvider.Current.VeataKadosh1, CommonPrayerTextProvider.Current.VeataKadosh2);

            if (hasMussaf)
            {
                kdushaDesidra.Add(EdotHaMizrachPrayerTextProvider.Instance.PreVerseOfDay2);
            }

            _items.Add(kdushaDesidra);

            ///(שיר של יום (ביום של מוסף
            if (hasMussaf)
            {
                AddDayVerse();
            }

            ///קדיש שלם
            if (!hasMussaf)
            {
                AddFullKadish();
            }

            if (torahReadingAdded)
            {
                lastSpan = AddTorahReadingEnding();

                if (hasMussaf)
                {
                    lastSpan.Add(PrayersHelper.GetHalfKadish(DayInfo));

                    await AddShmoneEsre(Prayer.Mussaf);

                    AddFullKadish();

                    Add(AppResources.BarchiNafshiTitle, Psalms.Psalm104);
                }
            }

            ///(שיר של יום (רגיל
            if (!hasMussaf)
            {
                SpanModel verseOfDay = new SpanModel(AppResources.DayVerseTitle);

                if (DayInfo.IsTachanunDay(GetNusach()))
                {
                    verseOfDay.Add(EdotHaMizrachPrayerTextProvider.Instance.PreVerseOfDay1);
                }

                verseOfDay.Add(EdotHaMizrachPrayerTextProvider.Instance.PreVerseOfDay2);

                AddDayVerse(verseOfDay);
            }

            ///קווה אל ה'
            Add(AppResources.PrayerEndingTitle, CommonPrayerTextProvider.Current.KavehElHashem, EdotHaMizrachPrayerTextProvider.Instance.KtoretHasamim2, CommonPrayerTextProvider.Current.PitumHaktoret1, CommonPrayerTextProvider.Current.PitumHaktoret2, CommonPrayerTextProvider.Current.KavehElHashemEnding);

            ///קדיש דרבנן
            Add(AppResources.KadishDerabananTitle, PrayersHelper.GetKadishDerabanan(DayInfo, false));

            ///עלינו לשבח
            SpanModel aleinu = new SpanModel(AppResources.AleinuLeshabeachTitle);

            aleinu.Add(CommonPrayerTextProvider.Current.Barchu);

            aleinu.Add(CommonPrayerTextProvider.Current.AleinuLeshabeach);
            _items.Add(aleinu);
        }

        protected override ShmoneEsreBase GetShmoneEsre(Prayer prayer)
        {
            return new ShmoneEsreEdotHamizrach(prayer);
        }

        protected override SpanModel AddTachanun()
        {
            bool showTachanun = false;

            if (DayInfo.AseretYameyTshuva)
            {
                AddAvinuMalkenu();
                showTachanun = true;
            }

            showTachanun |= DayInfo.IsTachanunDay(GetNusach());

            if (showTachanun)
            {
                SpanModel tachanun = new SpanModel(AppResources.TachanunTitle);

                bool isBH = DayInfo.DayOfWeek == DayOfWeek.Monday || DayInfo.DayOfWeek == DayOfWeek.Thursday;

                AddViduyAnd13Midot(tachanun);

                tachanun.Add(new ParagraphModel(GetNefilatApayimTitle(), PrayersHelper.GetNefilatApayim(GetNusach())));

                tachanun.Add(CommonPrayerTextProvider.Current.TachanunEnding);

                if (isBH)
                {
                    tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHsYg, EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS1);
                    tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHsYg, EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS2);
                    tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHsYg, EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS3);
                    tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS4);
                    tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH1, CommonPrayerTextProvider.Current.TachanunBH2, CommonPrayerTextProvider.Current.TachanunBH3,
                        CommonPrayerTextProvider.Current.TachanunBH4, CommonPrayerTextProvider.Current.TachanunBH5, CommonPrayerTextProvider.Current.TachanunBH6);
                }

                _items.Add(tachanun);

                return tachanun;
            }
            else if (GetHallelMode() == HallelMode.None)
            {
                var noTachanun = new SpanModel(AppResources.AfterHazarahTitle, EdotHaMizrachPrayerTextProvider.Instance.NoTachanunText);
                _items.Add(noTachanun);
                return noTachanun;
            }

            return null;
        }

        protected override void AddAvinuMalkenu()
        {
            Add(AppResources.AvinuMalkenuTitle,
                CommonPrayerTextProvider.Current.AvinuMalkenu1,
                CommonPrayerTextProvider.Current.AvinuMalkenu3,
                CommonPrayerTextProvider.Current.AvinuMalkenu4);
        }

        protected override bool ShouldAddHallelBlessing(ShacharitBase.HallelMode hallelMode)
        {
            return hallelMode == HallelMode.Full && DayInfo.YomTov != JewishCalendar.YOM_HAATZMAUT;
        }

        protected override void AddTextAfterNefilatApayim(Models.SpanModel tachanun)
        {
            tachanun.Add(CommonPrayerTextProvider.Current.TachanunEnding);
        }

        protected override void AddMondayThursdayText(SpanModel tachanun)
        {
            tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHsYg, EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS1);
            tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHsYg, EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS2);
            tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHsYg, EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS3);
            tachanun.Add(EdotHaMizrachPrayerTextProvider.Instance.TachanunBHS4);
            tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH1, CommonPrayerTextProvider.Current.TachanunBH2, CommonPrayerTextProvider.Current.TachanunBH3, CommonPrayerTextProvider.Current.TachanunBH4, CommonPrayerTextProvider.Current.TachanunBH5, CommonPrayerTextProvider.Current.TachanunBH6);
        }

        protected override void AddPreTorahBookHotzaaNoTachanun(SpanModel span)
        {
            span.Add(EdotHaMizrachPrayerTextProvider.Instance.PreSTNoTachanun);
        }

        protected override void AddTextBeforeTorahReading(SpanModel span)
        {
            span.Add(new ParagraphModel(AppResources.HagbahaTitle, CommonPrayerTextProvider.Current.VezotHatorah));
        }

        protected override SpanModel AddTorahReadingEnding()
        {
            SpanModel span = new SpanModel(AppResources.TorahBookReplacingTitle, CommonPrayerTextProvider.Current.TorahBookReplacing1);
            _items.Add(span);
            return span;
        }

        protected override void AddDayVerseExtras(SpanModel dayVerse)
        {
            int yomTov = DayInfo.YomTov;

            if (yomTov == JewishCalendar.FAST_OF_GEDALYAH || yomTov == JewishCalendar.TENTH_OF_TEVES)
            {
                dayVerse.Add(Psalms.Psalm83);
            }
            else if (yomTov == JewishCalendar.CHANUKAH)
            {
                dayVerse.Add(Psalms.Psalm30);
            }
            else if (yomTov == JewishCalendar.FAST_OF_ESTHER || yomTov == JewishCalendar.PURIM)
            {
                dayVerse.Add(Psalms.Psalm22);
            }
            else if (yomTov == JewishCalendar.SEVENTEEN_OF_TAMMUZ)
            {
                dayVerse.Add(Psalms.Psalm79);
            }
            else if (DayInfo.IsAfterYomKippur())
            {
                dayVerse.Add(Psalms.Psalm85);
            }

            dayVerse.Add(new ParagraphModel(AppResources.InMourningHouseTitle, Psalms.Psalm49) { IsCollapsible = true });

            dayVerse.AddRange(PrayersHelper.GetKadishYatom(DayInfo, true));
        }
    }
}
