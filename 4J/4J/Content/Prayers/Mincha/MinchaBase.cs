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
    [TextName(PrayerNames.Mincha)]
    public abstract class MinchaBase : SpansPrayerBase
    {
        protected override string GetTitle()
        {
            return CommonResources.MinchaTitle;
        }

        protected async override Task CreateOverrideAsync()
        {
            AddOpening();
            AddAshrey();

            if (ShouldAddTorahReading())
            {
                AddTorahReading();
            }

            await AddShmoneEsre();

            AddTachanun();

            AddFullKadhish();

            AddAleinuLeshabeach();

            if (DayInfo.YomTov == JewishCalendar.TISHA_BEAV)
            {
                throw new NotificationException(AppResources.TishaBeavMessage);
            }
        }

        protected virtual void AddOpening()
        { }

        private void AddAshrey()
        {
            SpanModel span = new SpanModel(AppResources.AshreyTitle);
            span.AddRange(GetAshrey());
            span.Add(PrayersHelper.GetHalfKadish(DayInfo));
            _items.Add(span);
        }

        protected virtual IEnumerable<ParagraphModel> GetAshrey()
        {
            yield return new ParagraphModel(CommonPrayerTextProvider.Current.Ashrey);
        }

        private bool ShouldAddTorahReading()
        {
            return DayInfo.YomTov == JewishCalendar.FAST_OF_GEDALYAH
                    || DayInfo.YomTov == JewishCalendar.FAST_OF_ESTHER
                    || DayInfo.YomTov == JewishCalendar.TENTH_OF_TEVES
                    || DayInfo.YomTov == JewishCalendar.SEVENTEEN_OF_TAMMUZ
                    || DayInfo.YomTov == JewishCalendar.TISHA_BEAV;
        }

        private void AddTorahReading()
        {
            SpanModel span = new SpanModel(AppResources.TorahBookHotzaaTitle);
            _items.Add(span);
            AddTorahBookHotzaa(span);

            span = new SpanModel(AppResources.TorahReadingAndHaftarahTitle);
            _items.Add(span);
            AddTorahReadingText(span);

            // Half Kadish
            span.Add(PrayersHelper.GetHalfKadish(DayInfo));

            AddAfterTorahReading(span);

            //Torah book replacing:
            span = new SpanModel(AppResources.TorahBookReplacingTitle);
            _items.Add(span);
            AddTorahBookReplacing(span);
        }

        protected abstract void AddTorahBookHotzaa(SpanModel span);

        private static void AddTorahReadingText(SpanModel span)
        {
            span.Add(CommonPrayerTextProvider.Current.BarchuTorah);
            span.Add(CommonPrayerTextProvider.Current.BrachaBeforeTorah);
            span.Add(CommonPrayerTextProvider.Current.BrachaAfterTorah);

            string torahReadingFont = "Tahoma";

            span.Add(new ParagraphModel(AppResources.CohenTitle, new RunModel(AppResources.TeanitReadingCohen) { Font = torahReadingFont }));
            span.Add(new ParagraphModel(AppResources.LeviTitle, new RunModel(AppResources.TeanitReadingLevi) { Font = torahReadingFont }));
            span.Add(new ParagraphModel(AppResources.IsraelTitle, new RunModel(AppResources.TeanitReadingIsrael) { Font = torahReadingFont }));

            span.Add(new ParagraphModel(AppResources.HaftarahBlessingTitle, CommonPrayerTextProvider.Current.BeforeHaftarahBlessing));

            span.Add(new ParagraphModel(AppResources.HaftarahTitle, new RunModel(AppResources.TeanitHaftarah) { Font = torahReadingFont }));
            span.Add(new ParagraphModel(AppResources.AfterHaftarahTitle, CommonPrayerTextProvider.Current.AfterHaftarahBlessing));

        }

        protected virtual void AddAfterTorahReading(SpanModel span)
        {

        }

        protected abstract void AddTorahBookReplacing(SpanModel span);

        private async Task AddShmoneEsre()
        {
            ShmoneEsreBase shmoneEsre = GetShmoneEsre();
            await shmoneEsre.CreateAsync(DayInfo, Logger);

            SpanModel shmoneEsreSpan = new SpanModel(shmoneEsre.Title);

            shmoneEsreSpan.AddRange(shmoneEsre.Items);

            _items.Add(shmoneEsreSpan);
        }

        protected abstract ShmoneEsreBase GetShmoneEsre();

        private void AddTachanun()
        {
            bool showTachanun = DayInfo.IsTachanunDay(GetNusach(), true);

            if (ShouldAddAvinuMalkenu())
            {
                AddAvinuMalkenu();
                showTachanun = true;
            }

            if (showTachanun)
            {
                SpanModel tachanun = new SpanModel(AppResources.TachanunTitle);

                AddViduyAnd13Midot(tachanun);

                tachanun.Add(new ParagraphModel(GetNefilatApayimTitle(), PrayersHelper.GetNefilatApayim(GetNusach())));

                tachanun.Add(new ParagraphModel(CommonPrayerTextProvider.Current.TachanunEnding));

                _items.Add(tachanun);
            }
            else
            {
                //texts[texts.Count - 1].Add(PrayTexts.ResourceManager.GetString("NoTachanunText"));
            }
        }

        protected virtual string GetNefilatApayimTitle()
        {
            return null;
        }

        protected virtual bool ShouldAddAvinuMalkenu()
        {
            return DayInfo.AseretYameyTshuva;
        }

        protected abstract void AddAvinuMalkenu();

        protected virtual void AddViduyAnd13Midot(SpanModel tachanun)
        {
            tachanun.Add(CommonPrayerTextProvider.Current.Viduy1);
            tachanun.Add(CommonPrayerTextProvider.Current.Viduy2);
            tachanun.Add(CommonPrayerTextProvider.Current.PreYgMidot);
            tachanun.Add(CommonPrayerTextProvider.Current.YgMidot);
        }

        protected void AddFullKadhish()
        {
            SpanModel fullKadish = new SpanModel(AppResources.KadishShalemTitle);
            fullKadish.AddRange(PrayersHelper.GetFullKadish(DayInfo));
            _items.Add(fullKadish);
        }

        protected void AddKadishYatom()
        {
            SpanModel kadishYatom = new SpanModel(AppResources.KadishYatomTitle);
            kadishYatom.AddRange(PrayersHelper.GetKadishYatom(DayInfo, false));
            _items.Add(kadishYatom);
        }

        protected virtual void AddAleinuLeshabeach()
        {
            SpanModel aleinu = new SpanModel(AppResources.AleinuLeshabeachTitle);
            aleinu.Add(new ParagraphModel(CommonPrayerTextProvider.Current.AleinuLeshabeach));
            _items.Add(aleinu);
        }
    }
}
