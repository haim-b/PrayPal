using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    [TextName(PrayerNames.Shacharit)]
    public abstract class ShacharitBase : SpansPrayerBase
    {
        protected async override Task CreateOverrideAsync()
        {
            // Sedder Hashkama
            SpanModel hashkama = new SpanModel(AppResources.SederHashkamaTitle);
            hashkama.Add(CommonPrayerTextProvider.Current.ModeAni);

            hashkama.Add(new ParagraphModel(null, new RunModel(AppResources.MorningNetilatYadayimInstruction + ":" + Environment.NewLine) /*{ FontSize = ParagraphModel.GetTitleSize() }*/, new RunModel(CommonPrayerTextProvider.Current.NetilatYadayimBlessing)));
            hashkama.Add(new ParagraphModel(AppResources.AsherYatzarTitle, CommonPrayerTextProvider.Current.AsherYatzar));
            hashkama.Add(CommonPrayerTextProvider.Current.BirkatNeshama);

            _items.Add(hashkama);

            // Birkot HaTorah
            Add(AppResources.BirkotHatorahTitle, CommonPrayerTextProvider.Current.BirkotHatorah, CommonPrayerTextProvider.Current.ParashatBirkatCohanim);

            SpanModel talitTfillin;

            // Talit and Tfillin
            if (DayInfo.IsTfillinTime())
            {
                talitTfillin = new SpanModel(AppResources.TalitAndTfillinTitle);
                talitTfillin.Add(new ParagraphModel(AppResources.AtifatTalitTitle, CommonPrayerTextProvider.Current.AtifatTalit));

                talitTfillin.Add(new ParagraphModel(AppResources.HanachatTfillinTitle, CommonPrayerTextProvider.Current.HanachatTfillin));
                talitTfillin.Add(CommonPrayerTextProvider.Current.ParashahAfterTfillin1, CommonPrayerTextProvider.Current.ParashahAfterTfillin2);
            }
            else
            {
                talitTfillin = new SpanModel(AppResources.AtifatTalitTitle, CommonPrayerTextProvider.Current.AtifatTalit);
            }

            _items.Add(talitTfillin);


            // Bikot HaShachar
            SpanModel birkotHaShachar = new SpanModel(AppResources.BirkotHashacharTitle);
            birkotHaShachar.Add(CommonPrayerTextProvider.Current.BirkotHashachar1, CommonPrayerTextProvider.Current.BirkotHashachar2, CommonPrayerTextProvider.Current.BirkotHashachar3, CommonPrayerTextProvider.Current.BirkotHashachar4);

            _items.Add(birkotHaShachar);


            // ברייתא דרבי ישמעאל
            Add(AppResources.BrayitaDerabiYishmaelTitle, CommonPrayerTextProvider.Current.BrayitaDerabiYishmael);
            Add(AppResources.KadishDerabananTitle, PrayersHelper.GetKadishDerabanan(DayInfo, false).ToArray());


            // תפילות השחר
            Add(AppResources.TfilotHashacharTitle, CommonPrayerTextProvider.Current.MizmorLifneyHaAron1, CommonPrayerTextProvider.Current.MizmorLifneyHaAron2,
                CommonPrayerTextProvider.Current.MizmorLifneyHaAron3, Psalms.Psalm30, CommonPrayerTextProvider.Current.ShacharitVerse1, Psalms.Psalm67);

            // פסוקי דזמרה
            SpanModel psukeyDezimra = new SpanModel(AppResources.PsukeyDezimraTitle);

            // ברוך שאמר
            psukeyDezimra.Add(CommonPrayerTextProvider.Current.BaruchSheamar);

            if (ShouldSayMizmorLetoda())
            {
                psukeyDezimra.Add(Psalms.Psalm100);
            }

            psukeyDezimra.Add(CommonPrayerTextProvider.Current.ShacharitVerse2, CommonPrayerTextProvider.Current.Ashrey, Psalms.Psalm146, Psalms.Psalm147, Psalms.Psalm148, Psalms.Psalm149, Psalms.Psalm150,
                Psalms.PsalmEnding, CommonPrayerTextProvider.Current.ShacharitVerse3, CommonPrayerTextProvider.Current.ShacharitVerse4, CommonPrayerTextProvider.Current.ShacharitVerse5);

            _items.Add(psukeyDezimra);


            // שירת הים וישתבח
            Add(AppResources.ShiratHayamTitle, CommonPrayerTextProvider.Current.ShiratHayam);

            SpanModel yistabach = new SpanModel(AppResources.YishtabachTitle, CommonPrayerTextProvider.Current.Yishtabach);

            if (DayInfo.AseretYameyTshuva)
            {
                yistabach.Add(new ParagraphModel(Psalms.Psalm130));
            }

            yistabach.Add(PrayersHelper.GetHalfKadish(DayInfo));

            _items.Add(yistabach);

            // ברכות קריאת שמע
            SpanModel shmaBlessings = new SpanModel(AppResources.BrachotKriatShmaTitle);
            shmaBlessings.Add(CommonPrayerTextProvider.Current.Barchu, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit1, CommonPrayerTextProvider.Current.KadoshKadosh, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit2, CommonPrayerTextProvider.Current.BrachotBeforeShmaShacharit3);

            _items.Add(shmaBlessings);

            // קריאת שמע
            SpanModel shma = new SpanModel(AppResources.KriatShmaTitle);
            shma.Add(CommonPrayerTextProvider.Current.KriatShma1, CommonPrayerTextProvider.Current.KriatShma2, CommonPrayerTextProvider.Current.KriatShma3);

            shma.Add(CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit1, CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit2, CommonPrayerTextProvider.Current.BrachotAfterShmaShacharit3);

            _items.Add(shma);

            // שמונה עשרה
            SpanModel lastSpan = await AddShmoneEsre(Prayer.Shacharit);

            // תחנון
            lastSpan = AddTachanun() ?? lastSpan;

            // הלל
            lastSpan = AddHallel() ?? lastSpan;

            bool hasMusaf = PrayersHelper.IsMussafDay(DayInfo);

            if (!hasMusaf)
            {
                lastSpan.Add(PrayersHelper.GetHalfKadish(DayInfo));
            }
            else
            {
                AddFullKadish();
                AddDayVerse();
            }

            bool isTachanunDay = DayInfo.IsTachanunDay(GetNusach());

            // קריאת התורה
            bool torahReadingAdded = AddTorahReading();

            // אשרי
            Add(AppResources.AshreyTitle, CommonPrayerTextProvider.Current.Ashrey);

            // למנצח, ובא לציון
            SpanModel kdushaDesidra = new SpanModel(AppResources.KdushaDesidraTitle);

            if (!hasMusaf && ShouldSayLamnatzeach())
            {
                kdushaDesidra.Add(Psalms.Psalm20);
            }

            kdushaDesidra.Add(CommonPrayerTextProvider.Current.UvaLetzion, CommonPrayerTextProvider.Current.VeataKadosh1, CommonPrayerTextProvider.Current.VeataKadosh2);

            _items.Add(kdushaDesidra);

            // קדיש שלם
            if (!hasMusaf)
            {
                AddFullKadish();
            }

            if (torahReadingAdded)
            {
                lastSpan = AddTorahReadingEnding();
            }

            if (hasMusaf)
            {
                lastSpan.Add(PrayersHelper.GetHalfKadish(DayInfo));

                await AddShmoneEsre(Prayer.Mussaf);

                AddFullKadish();
            }

            // שיר של יום
            if (!hasMusaf)
            {
                AddDayVerse();
            }

            // קווה אל ה'
            Add(AppResources.PrayerEndingTitle, CommonPrayerTextProvider.Current.KavehElHashem, CommonPrayerTextProvider.Current.PitumHaktoret1, CommonPrayerTextProvider.Current.PitumHaktoret2, CommonPrayerTextProvider.Current.KavehElHashemEnding);

            // קדיש דרבנן
            Add(AppResources.KadishDerabananTitle, PrayersHelper.GetKadishDerabanan(DayInfo, false).ToArray());


            // עלינו לשבח
            AddAleinuLeshabeach();

            // קדיש יתום
            AddKadishYatom();

            if (DayInfo.YomTov != JewishCalendar.CHOL_HAMOED_SUCCOS && DayInfo.YomTov != JewishCalendar.HOSHANA_RABBA) // We say LeDavid after verse of day on Chol Hamoed Succot.
            {
                AddLeDavid();
            }

            lastSpan = _items.Last();

            if (isTachanunDay)
            {
                lastSpan.Add(new ParagraphModel(AppResources.InMoarningHouseTitle, Psalms.Psalm49) { IsCollapsible = true });
            }
            else
            {
                lastSpan.Add(new ParagraphModel(AppResources.InMoarningHouseTitle, Psalms.Psalm16) { IsCollapsible = true });
            }


            if (DayInfo.YomTov == JewishCalendar.TISHA_BEAV)
            {
                throw new NotificationException(AppResources.TishaBeavMessage);
            }
            else if (DayInfo.YomTov != -1 && !DayInfo.JewishCalendar.CholHamoed && DayInfo.YomTov != JewishCalendar.YOM_HAZIKARON)
            {
                string moedTitle = HebDateHelper.GetMoedTitle(DayInfo.JewishCalendar, true);
                throw new NotificationException("שים לב שהיום " + moedTitle + ", וייתכן שיש שינויים בתפילה שלא יוצגו.");
            }

            //ShmoneEsreBase shmoneEsre = GetShmoneEsre();
            //await shmoneEsre.CreateAsync(_dayInfo, Logger);

            //SpanModel shmoneEsreSpan = new SpanModel(shmoneEsre.Title);

            //shmoneEsreSpan.AddRange(shmoneEsre.Items);

            //_items.Add(shmoneEsreSpan);
        }

        protected bool ShouldSayMizmorLetoda()
        {
            if (DayInfo.JewishCalendar.JewishMonth == JewishCalendar.TISHREI && DayInfo.JewishCalendar.GregorianDayOfMonth == 9)// Erev Kippur
            {
                return false;
            }

            if (DayInfo.JewishCalendar.JewishMonth == JewishDate.NISSAN && DayInfo.JewishCalendar.JewishDayOfMonth >= 14 && DayInfo.JewishCalendar.JewishDayOfMonth <= 20)
            {
                return false;
            }

            return true;
        }

        protected async Task<SpanModel> AddShmoneEsre(Prayer prayer)
        {
            ShmoneEsreBase shmoneEsre = GetShmoneEsre(prayer);
            await shmoneEsre.CreateAsync(DayInfo, Logger);

            SpanModel shmoneEsreSpan = new SpanModel(shmoneEsre.Title);

            shmoneEsreSpan.AddRange(shmoneEsre.Items);

            _items.Add(shmoneEsreSpan);

            return shmoneEsreSpan;
        }

        protected abstract ShmoneEsreBase GetShmoneEsre(Prayer prayer);

        protected virtual SpanModel AddTachanun()
        {
            bool showTachanun = false;

            if (ShouldAddAvinuMalkenu())
            {
                AddAvinuMalkenu();
                showTachanun = true;
            }

            showTachanun |= DayInfo.IsTachanunDay(GetNusach());

            if (showTachanun)
            {
                SpanModel tachanun = new SpanModel(AppResources.TachanunTitle);

                AddViduyAnd13Midot(tachanun);

                tachanun.Add(new ParagraphModel(GetNefilatApayimTitle(), PrayersHelper.GetNefilatApayim(GetNusach())));

                AddTextAfterNefilatApayim(tachanun);

                if (IsMondayOrThursday())
                {
                    AddMondayThursdayText(tachanun);
                }

                AddTachanunEnding(tachanun);

                _items.Add(tachanun);

                return tachanun;
            }
            else
            {
                //texts[texts.Count - 1].Add(PrayTexts.ResourceManager.GetString("NoTachanunText"));
                return null;
            }
        }

        protected virtual void AddTachanunEnding(SpanModel tachanun)
        {

        }

        protected abstract void AddMondayThursdayText(SpanModel tachanun);

        protected virtual void AddTextAfterNefilatApayim(SpanModel tachanun)
        {

        }

        protected virtual void AddViduyAnd13Midot(SpanModel tachanun)
        {
            tachanun.Add(new ParagraphModel(CommonPrayerTextProvider.Current.Viduy1));
            tachanun.Add(new ParagraphModel(CommonPrayerTextProvider.Current.Viduy2));
            tachanun.Add(new ParagraphModel(CommonPrayerTextProvider.Current.PreYgMidot));
            tachanun.Add(new ParagraphModel(CommonPrayerTextProvider.Current.YgMidot));
        }

        protected virtual string GetNefilatApayimTitle()
        {
            return null;
        }

        protected bool IsMondayOrThursday()
        {
            return DayInfo.DayOfWeek == DayOfWeek.Monday || DayInfo.DayOfWeek == DayOfWeek.Thursday;
        }

        protected virtual bool ShouldAddAvinuMalkenu()
        {
            return DayInfo.AseretYameyTshuva;
        }

        protected abstract void AddAvinuMalkenu();

        protected virtual SpanModel AddHallel()
        {
            HallelMode hallelMode = HallelMode.None;

            if (DayInfo.YomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || DayInfo.YomTov == JewishCalendar.HOSHANA_RABBA || DayInfo.YomTov == JewishCalendar.CHANUKAH || DayInfo.YomTov == JewishCalendar.YOM_HAATZMAUT || DayInfo.YomTov == JewishCalendar.YOM_YERUSHALAYIM)
            {
                hallelMode = HallelMode.Full;
            }
            else if (DayInfo.JewishCalendar.RoshChodesh || DayInfo.YomTov == JewishCalendar.CHOL_HAMOED_PESACH)
            {
                hallelMode = HallelMode.Partial;
            }

            if (hallelMode == HallelMode.None)
            {
                return null;
            }

            SpanModel hallel = new SpanModel(AppResources.HallelTitle);

            if (ShouldAddHallelBlessing(hallelMode))
            {
                hallel.Add(CommonPrayerTextProvider.Current.HallelBlessing);
            }

            hallel.Add(Psalms.Psalm113);
            hallel.Add(Psalms.Psalm114);

            if (hallelMode == HallelMode.Full)
            {
                hallel.Add(Psalms.Psalm115.Substring(0, 656));
            }

            hallel.Add(Psalms.Psalm115.Substring(657));

            if (hallelMode == HallelMode.Full)
            {
                hallel.Add(Psalms.Psalm116.Substring(0, 621));
            }

            hallel.Add(Psalms.Psalm116.Substring(622));

            hallel.Add(Psalms.Psalm117);

            hallel.Add(Psalms.Psalm118.Substring(0, 1032));

            hallel.Add(CommonPrayerTextProvider.Current.HallelEnding1, CommonPrayerTextProvider.Current.HallelEnding2, CommonPrayerTextProvider.Current.HallelEnding3);

            if (ShouldAddHallelEnding(hallelMode))
            {
                hallel.Add(CommonPrayerTextProvider.Current.HallelEnding4);
            }

            _items.Add(hallel);

            return hallel;
        }

        protected bool ShouldShowSlichot()
        {
            return false;
        }

        protected abstract bool ShouldAddHallelBlessing(HallelMode hallelMode);

        protected virtual bool ShouldAddHallelEnding(HallelMode hallelMode)
        {
            return hallelMode == HallelMode.Full;
        }

        protected enum HallelMode
        {
            None, Partial, Full
        }

        protected override string GetTitle()
        {
            return CommonResources.Shacharit;
        }

        protected virtual void AddAleinuLeshabeach()
        {
            Add(AppResources.AleinuLeshabeachTitle, CommonPrayerTextProvider.Current.Barchu, CommonPrayerTextProvider.Current.AleinuLeshabeach);
        }

        protected void AddFullKadish()
        {
            SpanModel fullKadish = new SpanModel(AppResources.KadishShalemTitle);
            fullKadish.AddRange(PrayersHelper.GetFullKadish(DayInfo));
            _items.Add(fullKadish);
        }

        protected void AddKadishYatom()
        {
            SpanModel kadishYatom = new SpanModel(AppResources.KadishYatomTitle);
            kadishYatom.AddRange(PrayersHelper.GetKadishYatom(DayInfo));
            _items.Add(kadishYatom);
        }

        protected virtual bool AddTorahReading()
        {
            if (!IsMondayOrThursday()
                && !DayInfo.Teanit
                && !DayInfo.JewishCalendar.RoshChodesh
                && DayInfo.YomTov != JewishCalendar.CHANUKAH
                && DayInfo.YomTov != JewishCalendar.PURIM
                && DayInfo.YomTov != JewishCalendar.CHOL_HAMOED_SUCCOS
                && DayInfo.YomTov != JewishCalendar.HOSHANA_RABBA
                && DayInfo.YomTov != JewishCalendar.CHOL_HAMOED_PESACH)
            {
                return false;
            }

            SpanModel span = new SpanModel(AppResources.TorahBookHotzaaTitle);
            _items.Add(span);

            if (IsMondayOrThursday())
            {
                bool isInIsrael = DayInfo.JewishCalendar.InIsrael;

                try
                {
                    DayInfo.JewishCalendar.InIsrael = false;

                    if (!DayInfo.JewishCalendar.RoshChodesh
                        && DayInfo.YomTov != JewishCalendar.SUCCOS
                        && DayInfo.YomTov != JewishCalendar.CHOL_HAMOED_SUCCOS
                        && DayInfo.YomTov != JewishCalendar.HOSHANA_RABBA
                        && DayInfo.YomTov != JewishCalendar.PESACH
                        && DayInfo.YomTov != JewishCalendar.CHOL_HAMOED_PESACH
                        && DayInfo.YomTov != JewishCalendar.CHANUKAH
                        && DayInfo.YomTov != JewishCalendar.PURIM
                        && DayInfo.YomTov != JewishCalendar.YOM_HAATZMAUT
                        && DayInfo.YomTov != JewishCalendar.YOM_YERUSHALAYIM
                        && DayInfo.YomTov != JewishCalendar.TISHA_BEAV
                        && DayInfo.YomTov != JewishCalendar.TU_BEAV
                        && !DayInfo.JewishCalendar.ErevYomTov)
                    {
                        AddPreTorahBookHotzaa(span);
                    }
                    else
                    {
                        AddPreTorahBookHotzaaNoTachanun(span);
                    }
                }
                finally
                {
                    DayInfo.JewishCalendar.InIsrael = isInIsrael;
                }
            }

            AddAronKodeshOpeningText(span);

            span.Add(CommonPrayerTextProvider.Current.Gadlu);

            span.Add(CommonPrayerTextProvider.Current.TorahBookWalking);

            AddTextBeforeTorahReading(span);

            //if (prayer == Prayer.Mincha)
            //{
            //    span = new TextsModel() { Title = AppResources.TorahReadingAndHaftarahTitle };
            //    texts.Add(span);
            //}

            span.Add(CommonPrayerTextProvider.Current.BarchuTorah);
            span.Add(CommonPrayerTextProvider.Current.BrachaBeforeTorah);
            span.Add(CommonPrayerTextProvider.Current.BrachaAfterTorah);

            AddTorahReadingText(span);

            // חצי קדיש
            span.Add(PrayersHelper.GetHalfKadish(DayInfo));

            AddTextAfterTorahReading(span);

            return true;
        }

        protected virtual void AddPreTorahBookHotzaa(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.PreST);
        }

        protected virtual void AddPreTorahBookHotzaaNoTachanun(SpanModel span)
        {

        }

        protected virtual void AddAronKodeshOpeningText(SpanModel span)
        {

        }

        protected virtual void AddTextBeforeTorahReading(SpanModel span)
        {

        }

        protected static void AddTorahReadingText(SpanModel span)
        {
            //span.Add(CommonPrayerTextProvider.Current.BarchuTorah);
            //span.Add(CommonPrayerTextProvider.Current.BrachaBeforeTorah);
            //span.Add(CommonPrayerTextProvider.Current.BrachaAfterTorah);

            //FontFamily torahReadingFont = new FontFamily("Tahoma");

            //span.Add(new ParagraphModel(AppResources.CohenTitle, new RunModel(AppResources.TeanitReadingCohen) { Font = torahReadingFont }));
            //span.Add(new ParagraphModel(AppResources.LeviTitle, new RunModel(AppResources.TeanitReadingLevi) { Font = torahReadingFont }));
            //span.Add(new ParagraphModel(AppResources.IsraelTitle, new RunModel(AppResources.TeanitReadingIsrael) { Font = torahReadingFont }));

            //span.Add(new ParagraphModel(AppResources.HaftarahBlessingTitle, CommonPrayerTextProvider.Current.BeforeHaftarahBlessing));

            //span.Add(new ParagraphModel(AppResources.HaftarahTitle, new RunModel(AppResources.TeanitHaftarah) { Font = torahReadingFont }));
            //span.Add(new ParagraphModel(AppResources.AfterHaftarahTitle, CommonPrayerTextProvider.Current.AfterHaftarahBlessing));
        }

        protected virtual void AddTextAfterTorahReading(SpanModel span)
        {

        }

        protected abstract SpanModel AddTorahReadingEnding();

        protected bool ShouldSayLamnatzeach()
        {
            if (DayInfo.JewishCalendar.RoshChodesh)
            {
                return false;
            }

            bool isInIsrael = DayInfo.JewishCalendar.InIsrael;

            // Mark not in israel to include issru chag:
            try
            {
                DayInfo.JewishCalendar.InIsrael = false;

                switch (DayInfo.YomTov)
                {
                    case JewishCalendar.SUCCOS:
                    case JewishCalendar.CHOL_HAMOED_SUCCOS:
                    case JewishCalendar.HOSHANA_RABBA:
                    case JewishCalendar.SIMCHAS_TORAH: // Equals issru chag
                    case JewishCalendar.CHANUKAH:
                    case JewishCalendar.TU_BESHVAT:
                    case JewishCalendar.PURIM:
                    case JewishCalendar.SHUSHAN_PURIM:
                    case JewishCalendar.PURIM_KATAN:
                    case JewishCalendar.PESACH: // Includes issru chag
                    case JewishCalendar.CHOL_HAMOED_PESACH:
                    case JewishCalendar.PESACH_SHENI:
                    case JewishCalendar.YOM_HAATZMAUT:
                    case JewishCalendar.YOM_YERUSHALAYIM:
                    case JewishCalendar.TISHA_BEAV:
                    case JewishCalendar.TU_BEAV:
                        return false;
                }
            }
            finally
            {
                DayInfo.JewishCalendar.InIsrael = isInIsrael;
            }

            if (DayInfo.JewishCalendar.ErevYomTov)
            {
                return false;
            }

            return true;
        }

        protected void AddDayVerse()
        {
            //CultureInfo culture = new CultureInfo("he-IL");
            DayOfWeek day = DayInfo.DayOfWeek;

            //TextsModel t = new TextsModel(string.Format(AppResources.DayVerseTitle_F0, culture.DateTimeFormat.DayNames[(int)day]));
            SpanModel dayVerse = new SpanModel(AppResources.DayVerseTitle);

            string opening;
            string verse;

            switch (day)
            {
                case DayOfWeek.Sunday:
                    opening = CommonPrayerTextProvider.Current.DayVerseDay1;
                    verse = Psalms.Psalm24;
                    break;
                case DayOfWeek.Monday:
                    opening = CommonPrayerTextProvider.Current.DayVerseDay2;
                    verse = Psalms.Psalm48;
                    break;
                case DayOfWeek.Tuesday:
                    opening = CommonPrayerTextProvider.Current.DayVerseDay3;
                    verse = Psalms.Psalm82;
                    break;
                case DayOfWeek.Wednesday:
                    opening = CommonPrayerTextProvider.Current.DayVerseDay4;
                    verse = Psalms.Psalm94 + " " + CommonPrayerTextProvider.Current.DayVerseWedEnd;
                    break;
                case DayOfWeek.Thursday:
                    opening = CommonPrayerTextProvider.Current.DayVerseDay5;
                    verse = Psalms.Psalm81;
                    break;
                case DayOfWeek.Friday:
                    opening = CommonPrayerTextProvider.Current.DayVerseDay6;
                    verse = Psalms.Psalm93;
                    break;
                default:
                    return;
            }

            dayVerse.Add(string.Format(CommonPrayerTextProvider.Current.DayVerseOpening_F0, opening), verse);

            AddDayVerseEnding(dayVerse);

            AddDayVerseExtras(dayVerse);

            _items.Add(dayVerse);
        }

        protected virtual void AddDayVerseEnding(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.DayVerseEnd);
        }

        protected virtual void AddDayVerseExtras(SpanModel dayVerse)
        {

        }

        protected virtual void AddLeDavid()
        {
            if (DayInfo.ShouldSayLeDavid())
            {
                SpanModel ledavid = PrayersHelper.GetPsalm(27);
                _items.Add(ledavid);

                AddKadishYatom();
            }
        }

    }
}
