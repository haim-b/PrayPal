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
    [Nusach(Nusach.Baladi)]
    public class ShacharitBaladi : ShacharitEdotHaMizrach
    {
        public ShacharitBaladi(IPermissionsService permissionsService)
            : base(permissionsService)
        { }

        protected override async Task CreateOverrideAsync()
        {
            // Sedder Hashkama
            SpanModel hashkama = new SpanModel(AppResources.SederHashkamaTitle, CommonPrayerTextProvider.Current.ModeAni);

            hashkama.Add(new ParagraphModel(AppResources.MorningNetilatYadayimInstruction + ":" + Environment.NewLine + CommonPrayerTextProvider.Current.NetilatYadayimBlessing));
            hashkama.Add(new ParagraphModel(BaladiPrayerTextProvider.Instance.AdonHaOlaminMorning));
            hashkama.Add(new ParagraphModel(AppResources.AsherYatzarTitle, CommonPrayerTextProvider.Current.AsherYatzar));
            hashkama.Add(new ParagraphModel(CommonPrayerTextProvider.Current.BirkatNeshama));// { Title2 = AppResources.BirkatNeshamaTitle });

            _items.Add(hashkama);

            // Birkot HaShachar
            List<string> bh = new List<string>(5);
            bh.Add(DayInfo.YomTov == JewishCalendar.TISHA_BEAV ? EdotHaMizrachPrayerTextProvider.Instance.BirkotHashachar1Av9th : CommonPrayerTextProvider.Current.BirkotHashachar1);
            bh.Add(CommonPrayerTextProvider.Current.BirkotHashachar2);

            if (DayInfo.YomTov != JewishCalendar.TISHA_BEAV)
            {
                bh.Add(BaladiPrayerTextProvider.Instance.BirkotHashachar2NotAv9th);
            }

            bh.Add(CommonPrayerTextProvider.Current.BirkotHashachar3);
            bh.Add(CommonPrayerTextProvider.Current.BirkotHashachar4);

            Add(AppResources.BirkotHashacharTitle, bh.ToArray());

            // Birkot HaTorah
            Add(AppResources.BirkotHatorahTitle, CommonPrayerTextProvider.Current.BirkotHatorah, CommonPrayerTextProvider.Current.ParashatBirkatCohanim);

            SpanModel talitTfillin;

            // Talit and Tfillin
            if (DayInfo.IsTfillinTime())
            {
                talitTfillin = new SpanModel(AppResources.TalitAndTfillinTitle);
                talitTfillin.Add(new ParagraphModel(AppResources.AtifatTalitTitle, CommonPrayerTextProvider.Current.AtifatTalit));

                talitTfillin.Add(new ParagraphModel(AppResources.HanachatTfillinTitle, CommonPrayerTextProvider.Current.HanachatTfillin));
                await AddTfillinMirrorAsync(talitTfillin);
            }
            else
            {
                talitTfillin = new SpanModel(AppResources.AtifatTalitTitle, CommonPrayerTextProvider.Current.AtifatTalit);
            }

            SpanModel haMehulal = new SpanModel(AppResources.PsukeyDezimraTitle);

            if (DayInfo.AseretYameyTshuva)
            {
                haMehulal.Add(new ParagraphModel(AppResources.InAseretYameyTshuvaTitle, BaladiPrayerTextProvider.Instance.ShofetKolHaAretz));
            }


            // Psukey DeZimra
            haMehulal.Add(CommonPrayerTextProvider.Current.BaruchSheamar, Psalms.Psalm100,
            CommonPrayerTextProvider.Current.ShacharitVerse2, CommonPrayerTextProvider.Current.Ashrey, Psalms.Psalm146, Psalms.Psalm147, Psalms.Psalm148, Psalms.Psalm149, Psalms.Psalm150,
                BaladiPrayerTextProvider.Instance.PsalmsEnding, CommonPrayerTextProvider.Current.ShacharitVerse3, CommonPrayerTextProvider.Current.ShacharitVerse5);

            _items.Add(haMehulal);

            // Shirat Hayam and Yishtabach
            Add(AppResources.ShiratHayamTitle, new ParagraphModel(BaladiPrayerTextProvider.Instance.ShiratHayamHtml) { IsTextStretched = true }, new ParagraphModel(BaladiPrayerTextProvider.Instance.VatikachMiryam));

            SpanModel yishtabach = new SpanModel(AppResources.YishtabachTitle, BaladiPrayerTextProvider.Instance.Refaeni, CommonPrayerTextProvider.Current.Yishtabach);

            yishtabach.Add(PrayersHelper.GetHalfKadish(DayInfo));

            _items.Add(yishtabach);

            // Brachot Shma
            Add(AppResources.BrachotKriatShmaTitle, BaladiPrayerTextProvider.Instance.BarchuShacharit, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit1, CommonPrayerTextProvider.Current.KadoshKadosh, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit2, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit3);

            // Kriat Shma
            Add(AppResources.KriatShmaTitle, CommonPrayerTextProvider.Current.KriatShma1, CommonPrayerTextProvider.Current.KriatShma2, CommonPrayerTextProvider.Current.KriatShma3,
                CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit1, CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit2);

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

            if (DayInfo.YomTov == JewishCalendar.TISHA_BEAV)
            {
                throw new NotificationException(AppResources.TishaBeavMessage);
            }
            else if (!IsPrayerFullyHandled())
            {
                string moedTitle = HebDateHelper.GetMoedTitle(DayInfo.JewishCalendar, true);
                throw new NotificationException("שים לב שהיום " + moedTitle + ", וייתכן שיש שינויים בתפילה שלא יוצגו.");
            }
        }

        protected override void AddTorahBookGadlu(SpanModel span)
        {

        }

        protected override ShmoneEsreBase GetShmoneEsre(Prayer prayer)
        {
            return new ShmoneEsreBaladi(prayer);
        }

        protected override SpanModel AddTachanun()
        {
            bool showTachanun = DayInfo.IsTachanunDay(GetNusach());

            if (showTachanun)
            {
                SpanModel tachanun = new SpanModel(AppResources.TachanunTitle);

                bool isBH = DayInfo.DayOfWeek == DayOfWeek.Monday || DayInfo.DayOfWeek == DayOfWeek.Thursday;

                tachanun.Add(new ParagraphModel(GetNefilatApayimTitle(), PrayersHelper.GetNefilatApayim(GetNusach())));

                if (DayInfo.AseretYameyTshuva)
                {
                    AddAvinuMalkenu();
                }

                if (isBH)
                {
                    tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH1, CommonPrayerTextProvider.Current.TachanunBH2, CommonPrayerTextProvider.Current.TachanunBH3,
                        DayInfo.DayOfWeek == DayOfWeek.Monday ? BaladiPrayerTextProvider.Instance.TachanunBHMon : BaladiPrayerTextProvider.Instance.TachanunBHThu, CommonPrayerTextProvider.Current.TachanunBH4);
                }

                tachanun.Add(CommonPrayerTextProvider.Current.TachanunEnding);

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
                CommonPrayerTextProvider.Current.AvinuMalkenu1);
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
            bool isRochChodeshOrCholHaMoed = DayInfo.YomTov == JewishCalendar.ROSH_CHODESH || DayInfo.IsCholHamoed;
            span.Add(string.Format(EdotHaMizrachPrayerTextProvider.Instance.PreSTNoTachanun, isRochChodeshOrCholHaMoed ? Environment.NewLine + BaladiPrayerTextProvider.Instance.PreSTNoTachanunWithMussaf : ""));
        }

        protected override void AddTextBeforeTorahReading(SpanModel span)
        {
            span.Add(BaladiPrayerTextProvider.Instance.BeforeTorahReading);
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
