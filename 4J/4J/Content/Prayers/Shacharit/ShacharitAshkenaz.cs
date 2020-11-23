using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    [Nusach(Nusach.Ashkenaz)]
    public class ShacharitAshkenaz : ShacharitSfard
    {
        protected override async Task CreateOverrideAsync()
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


            // פסוקי דזמרה
            SpanModel psukeyDezimra = new SpanModel(AppResources.PsukeyDezimraTitle);
            psukeyDezimra.Add(Psalms.Psalm30);

            // קדיש יתום
            psukeyDezimra.AddRange(PrayersHelper.GetKadishYatom(DayInfo, true));

            // ברוך שאמר
            psukeyDezimra.Add(CommonPrayerTextProvider.Current.BaruchSheamar);

            // הודו
            psukeyDezimra.Add(CommonPrayerTextProvider.Current.MizmorLifneyHaAron1, CommonPrayerTextProvider.Current.MizmorLifneyHaAron2, CommonPrayerTextProvider.Current.MizmorLifneyHaAron3);

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
            }

            bool isTachanunDay = DayInfo.IsTachanunDay(GetNusach());

            // קריאת התורה
            bool torahReadingAdded = AddTorahReading();

            if (torahReadingAdded)
            {
                lastSpan = AddTorahReadingEnding();
            }


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

            if (hasMusaf)
            {
                Add(PrayersHelper.GetHalfKadish(DayInfo));

                await AddShmoneEsre(Prayer.Mussaf);

                AddFullKadish();
            }

            // עלינו לשבח
            AddAleinuLeshabeach();

            // קדיש יתום
            AddKadishYatom();

            // שיר של יום
            AddDayVerse();

            // קווה אל ה'
            Add(AppResources.PrayerEndingTitle, CommonPrayerTextProvider.Current.KavehElHashem, CommonPrayerTextProvider.Current.PitumHaktoret1, CommonPrayerTextProvider.Current.KavehElHashemEnding);

            // קדיש דרבנן
            Add(AppResources.KadishDerabananTitle, PrayersHelper.GetKadishDerabanan(DayInfo, false).ToArray());

            AddLeDavid();

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

        protected override ShmoneEsreBase GetShmoneEsre(Prayer prayer)
        {
            return new ShmoneEsreAshkenaz(prayer);
        }

        protected override void AddAvinuMalkenu()
        {
            SpanModel avinuMalkenu = new SpanModel(AppResources.AvinuMalkenuTitle);

            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu1);

            if (DayInfo.AseretYameyTshuva)
            {
                avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu2AYT);
            }
            else
            {
                avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu2Teanit);
            }

            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu3);
            avinuMalkenu.Add(CommonPrayerTextProvider.Current.AvinuMalkenu4);

            _items.Add(avinuMalkenu);
        }

        protected override bool ShouldAddHallelBlessing(HallelMode hallelMode)
        {
            return true;
        }

        protected override bool ShouldAddHallelEnding(HallelMode hallelMode)
        {
            return true;
        }

        protected override void AddViduyAnd13Midot(SpanModel tachanun)
        {
            if (IsMondayOrThursday() && !ShouldShowSlichot())
            {
                // Show viduy and 13 Midot only on Monday and Thursday when there are no Slichot:
                base.AddViduyAnd13Midot(tachanun);
            }

            if (IsMondayOrThursday())
            {
                tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH1, CommonPrayerTextProvider.Current.TachanunBH2, CommonPrayerTextProvider.Current.TachanunBH3, CommonPrayerTextProvider.Current.TachanunBH4, CommonPrayerTextProvider.Current.TachanunBH5, CommonPrayerTextProvider.Current.TachanunBH6);
            }
        }

        protected override void AddMondayThursdayText(SpanModel tachanun)
        {
            tachanun.Add(CommonPrayerTextProvider.Current.TachanunBH7, CommonPrayerTextProvider.Current.TachanunBH8);
        }

        protected override void AddDayVerseEnding(SpanModel span)
        {

        }

        protected override void AddDayVerseExtras(SpanModel dayVerse)
        {
            dayVerse.AddRange(PrayersHelper.GetKadishYatom(DayInfo, true));
        }

        protected override void AddAleinuLeshabeach()
        {
            Add(AppResources.AleinuLeshabeachTitle, CommonPrayerTextProvider.Current.AleinuLeshabeach);
        }

    }
}
