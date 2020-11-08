using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Common.Resources;
using PrayPal.Common.Services;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content.Prayers.Arvit
{
    [PrayerName(PrayerNames.Arvit)]
    public abstract class ArvitBase : SpansPrayerBase
    {
        private readonly ILocationService _locationService;

        public ArvitBase(ILocationService locationService)
        {
            if (locationService == null)
            {
                throw new ArgumentNullException("locationService");
            }

            _locationService = locationService;
        }

        protected override string GetTitle()
        {
            return CommonResources.ArvitTitle;
        }

        public override bool IsDayCritical
        {
            get { return true; }
        }

        protected async override Task CreateOverrideAsync()
        {
            if (IsYomHaazmaut())
            {
                AddYomHaazmaut();
            }

            AddVersesBeforeArvit();

            SpanModel shma = new SpanModel(AppResources.KriatShmaAndBlessingTitle);
            shma.AddRange(GetShmaAndBlessings().Select(p => new ParagraphModel(p)));

            shma.Add(PrayersHelper.GetHalfKadish(_dayInfo));

            _items.Add(shma);

            await AddShmoneEsre();

            if (ShouldShowVeyehiNoam())
            {
                SpanModel motzaeyShabbat = new SpanModel(AppResources.ForMotzashTitle);
                motzaeyShabbat.Add(PrayersHelper.GetHalfKadish(_dayInfo));
                motzaeyShabbat.Add(CommonPrayerTextProvider.Current.VeyehiNoam, CommonPrayerTextProvider.Current.VeataKadosh1, CommonPrayerTextProvider.Current.VeataKadosh2);

                _items.Add(motzaeyShabbat);
            }

            Add(AppResources.KadishShalemTitle, PrayersHelper.GetFullKadish(_dayInfo).ToArray());

            AddPsalm121();

            AddEnding();

            if (_dayInfo.YomTov == JewishCalendar.TISHA_BEAV)
            {
                throw new NotificationException(AppResources.TishaBeavMessage);
            }

            if (_dayInfo.DayOfOmer != -1)
            {
                if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 23)
                {
                    if (!Settings.UseLocation)
                    {
                        throw new NotificationException(AppResources.OmerCountUnusedLocationWarning);
                    }
                    else if (await _locationService.GetCurrentPositionAsync() == null)
                    {
                        throw new NotificationException(AppResources.OmerCountNoLocationWarning);
                    }
                }
            }
        }

        private bool IsYomHaazmaut()
        {
            if (_dayInfo.YomTov == JewishCalendar.YOM_HAATZMAUT)
            {
                return true;
            }
            else
            {
                JewishCalendar jc = HebDateHelper.Clone(_dayInfo.JewishCalendar);
                jc.forward();

                if (jc.YomTovIndex == JewishCalendar.YOM_HAATZMAUT && DateTime.Now.Hour > 12)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddYomHaazmaut()
        {
            HebrewDateFormatter formatter = new HebrewDateFormatter { UseGershGershayim = false };

            SpanModel hodaa = new SpanModel(AppResources.YomHaazmautARvitHodaaTitle);
            hodaa.Add(new ParagraphModel(string.Format(AppResources.PsalmTitle, formatter.formatHebrewNumber(107)), Psalms.Psalm107));
            hodaa.Add(new ParagraphModel(string.Format(AppResources.PsalmTitle, formatter.formatHebrewNumber(97)), Psalms.Psalm97));
            hodaa.Add(new ParagraphModel(string.Format(AppResources.PsalmTitle, formatter.formatHebrewNumber(98)), Psalms.Psalm98));

            _items.Add(hodaa);
        }

        protected virtual void AddVersesBeforeArvit()
        {

        }

        protected virtual IEnumerable<string> GetShmaAndBlessings()
        {
            yield return CommonPrayerTextProvider.Current.VehuRachum;
            yield return CommonPrayerTextProvider.Current.Barchu;
            yield return CommonPrayerTextProvider.Current.BrachotBeforeShmaArvit;
            yield return CommonPrayerTextProvider.Current.KriatShma1;
            yield return CommonPrayerTextProvider.Current.KriatShma2;
            yield return CommonPrayerTextProvider.Current.KriatShma3;
            yield return CommonPrayerTextProvider.Current.BrachotAfterShmaArvit1;
            yield return CommonPrayerTextProvider.Current.BrachotAfterShmaArvit2;
        }

        private async Task AddShmoneEsre()
        {
            ShmoneEsreBase shmoneEsre = GetShmoneEsre();
            await shmoneEsre.CreateAsync(_dayInfo, Logger);

            SpanModel shmoneEsreSpan = new SpanModel(shmoneEsre.Title);

            shmoneEsreSpan.AddRange(shmoneEsre.Items);

            _items.Add(shmoneEsreSpan);
        }

        protected abstract ShmoneEsreBase GetShmoneEsre();

        private bool ShouldShowVeyehiNoam()
        {
            JewishCalendar jc = HebDateHelper.Clone(_dayInfo.JewishCalendar);

            if (jc.DayOfWeek == 1) //= Motzaey Shabbat
            {
                for (int i = 1; i <= 5; i++)
                {
                    jc.forward();

                    int yomTov = jc.YomTovIndex;

                    switch (yomTov)
                    {
                        case JewishCalendar.ROSH_HASHANA:
                        case JewishCalendar.YOM_KIPPUR:
                        case JewishCalendar.PESACH:
                        case JewishCalendar.SHAVUOS:
                        case JewishCalendar.SUCCOS:
                        case JewishCalendar.CHOL_HAMOED_PESACH:
                        case JewishCalendar.CHOL_HAMOED_SUCCOS:
                        case JewishCalendar.HOSHANA_RABBA:
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

        protected virtual void AddPsalm121()
        {
            Add(AppResources.Psalm121Title, Psalms.Psalm121);

            Add(AppResources.KadishYatomTitle, PrayersHelper.GetKadishYatom(_dayInfo, false).ToArray());
        }

        protected abstract void AddEnding();

        protected abstract void AddAleinuLeshabeach();

        protected virtual void AddSfiratHaOmer()
        {
            int omer = _dayInfo.DayOfOmer;

            if (omer < 0)
            {
                return;
            }

            SpanModel sfiratHaOmer = new SpanModel(AppResources.OmerCountTitle);

            AddTextBeforeSforatHaOmer(sfiratHaOmer);

            sfiratHaOmer.Add(CommonPrayerTextProvider.Current.SfiratHaOmerBlessing);

            string[] days = omer == 1 ? null : CommonPrayerTextProvider.Current.SfiratHaOmer2To49.Split('|');

            string omerDay = omer > 1 ? days[omer - 2] : null; ///omer -2 because we subtract 1 for having zero-index and another 1 because our array starts from 2 and not 1.
            string omerCount;

            if (omer == 1)
            {
                omerCount = CommonPrayerTextProvider.Current.SfiratHaOmer1;
            }
            else if (omer <= 6)
            {
                omerCount = string.Format(CommonPrayerTextProvider.Current.SfiratHaOmerShort_F0, omerDay);
            }
            else
            {
                string omerInWeeks = CommonPrayerTextProvider.Current.SfiratHaOmerWeeks.Split('|')[(omer / 7) - 1];

                int omerDaysMod = (omer % 7) - 1;

                if (omerDaysMod >= 0)
                {
                    omerInWeeks += CommonPrayerTextProvider.Current.SfiratHaOmerDays.Split('|')[omerDaysMod];
                }

                string omerFormat = omer <= 10 ? CommonPrayerTextProvider.Current.SfiratHaOmerLong2_F1 : CommonPrayerTextProvider.Current.SfiratHaOmerLong_F1;

                omerCount = string.Format(omerFormat, omerDay, omerInWeeks);
            }

            sfiratHaOmer.Add(omerCount);

            sfiratHaOmer.Add(CommonPrayerTextProvider.Current.AfterSfiratHaOmer1, Psalms.Psalm67, CommonPrayerTextProvider.Current.AnnaBechoach);

            AddTextAfterSforatHaOmer(sfiratHaOmer);

            _items.Add(sfiratHaOmer);
        }

        protected abstract void AddTextBeforeSforatHaOmer(SpanModel sfiratHaOmer);

        protected virtual void AddTextAfterSforatHaOmer(SpanModel sfiratHaOmer)
        {

        }
    }
}
