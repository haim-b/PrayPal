using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using Zmanim.HebrewCalendar;

namespace PrayPal.Content
{
    public abstract class ShmoneEsreBase : ParagraphsPrayerBase
    {
        protected readonly Prayer _prayer;

        public ShmoneEsreBase(Prayer prayer)
        {
            _prayer = prayer;
            Title = GetTitle();
        }

        protected override Task CreateOverride()
        {
            AddOpening();

            AddPart1();

            AddPart2();

            if (_prayer != Prayer.Mussaf)
            {
                AddPart3();

                AddParts4To15();

                AddPart16();
            }
            else
            {
                AddPart3Musaf();
                AddMusafMiddleBlessing1();

                if (!_dayInfo.JewishCalendar.RoshChodesh)
                {
                    // IF not Rosh Chodesh, we're on a holiday:
                    AddMussafHolidayPart();
                }
            }

            AddPart17();

            AddModim();

            AddAlHanissim();

            AddPart18();

            AddBirkatCohanim();

            AddPart19();

            AddEnding1();

            AddOseShalom();

            AddEnding2();

            return Task.CompletedTask;
        }

        protected virtual void AddOpening()
        {
            Add(CommonPrayerTextProvider.Current.SfatayTiftach);
        }

        protected void AddPart1()
        {
            if (!_dayInfo.AseretYameyTshuva)
            {
                Add(string.Format(CommonPrayerTextProvider.Current.SE01, string.Empty));
            }
            else
            {
                AddStringFormat(CommonPrayerTextProvider.Current.SE01, CommonPrayerTextProvider.Current.SE01AYT, true, true);
            }
        }

        protected void AddPart2()
        {
            string[] texts = string.Format(CommonPrayerTextProvider.Current.SE02, "|", "|").Split('|');

            if (texts.Length < 2)
            {
                return;
            }

            ParagraphModel p = new ParagraphModel(texts[0]);

            if (_dayInfo.IsMoridHatal())
            {
                p.Add(new RunModel(CommonPrayerTextProvider.Current.SE02Summer, false, true));
            }
            else
            {
                p.Add(new RunModel(CommonPrayerTextProvider.Current.SE02Winter, false, true));
            }

            if (!string.IsNullOrEmpty(texts[1]))
            {
                p.Add(texts[1]);
            }

            if (_dayInfo.AseretYameyTshuva)
            {
                p.Add(new RunModel(CommonPrayerTextProvider.Current.SE02AYT, true));
            }

            if (!string.IsNullOrEmpty(texts[2]))
            {
                p.Add(texts[2]);
            }

            _items.Add(p);
        }

        protected virtual void AddPart3()
        {
            if (_prayer != Prayer.Arvit)
            {
                Add(CommonPrayerTextProvider.Current.Kdusha, AppResources.KdushaTitle, true);
            }

            AddStringFormat(CommonPrayerTextProvider.Current.SE03, _dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.SE03Hamelech : CommonPrayerTextProvider.Current.SE03Hael, _dayInfo.AseretYameyTshuva);
        }

        protected void AddParts4To15()
        {
            bool showAttaChonantanu = _prayer == Prayer.Arvit && _dayInfo.ShowAttaChonantanu();
            bool aseretYameyTshuva = _dayInfo.AseretYameyTshuva;

            AddStringFormat(CommonPrayerTextProvider.Current.SE04, showAttaChonantanu ? CommonPrayerTextProvider.Current.SE04Havdalah : "", false, true);

            Add(CommonPrayerTextProvider.Current.SE05);
            Add(CommonPrayerTextProvider.Current.SE06);
            Add(CommonPrayerTextProvider.Current.SE07);

            if ((_prayer == Prayer.Mincha || _prayer == Prayer.Shacharit) && _dayInfo.Teanit)
            {
                Add(string.Format(CommonPrayerTextProvider.Current.Anenu, CommonPrayerTextProvider.Current.AnenuEnding), AppResources.InHazarahTitle);
            }

            Add(CommonPrayerTextProvider.Current.SE08);

            AddBirkatHaShanim();

            Add(CommonPrayerTextProvider.Current.SE10);

            AddStringFormat(CommonPrayerTextProvider.Current.SE11, aseretYameyTshuva ? CommonPrayerTextProvider.Current.SE11Aseret : CommonPrayerTextProvider.Current.SE11Regular, false, aseretYameyTshuva);

            Add(CommonPrayerTextProvider.Current.SE12);
            Add(CommonPrayerTextProvider.Current.SE13);

            bool isAv9th = _dayInfo.YomTov == JewishCalendar.TISHA_BEAV;
            AddStringFormat(CommonPrayerTextProvider.Current.SE14, isAv9th ? CommonPrayerTextProvider.Current.Nachem : CommonPrayerTextProvider.Current.SE14B, false, isAv9th);

            Add(CommonPrayerTextProvider.Current.SE15);
        }

        protected virtual void AddBirkatHaShanim()
        {
            string arg;

            if (_dayInfo.IsVetenBracha())
            {
                arg = CommonPrayerTextProvider.Current.SE09Summer;
                Settings.ShowVeanenu = false;
            }
            else
            {
                arg = CommonPrayerTextProvider.Current.SE09Winter;
            }


            ParagraphModel p = PrayersHelper.CreateParagraphForStringFormat(CommonPrayerTextProvider.Current.SE09, new RunModel(arg, false, true));

            _items.Add(p);
        }

        protected abstract void AddPart16();

        private void AddPart17()
        {
            Add(CommonPrayerTextProvider.Current.SE17);

            if (_prayer != Prayer.Mussaf) // In Mussaf we don't say Yaaleh VeYavo
            {
                ParagraphModel yaalehVeYavo = PrayersHelper.GetYaalehVeYavo(_dayInfo);

                if (yaalehVeYavo != null)
                {
                    _items.Add(yaalehVeYavo);
                }
            }

            Add(CommonPrayerTextProvider.Current.SE17B);

        }

        private void AddModim()
        {
            Add(CommonPrayerTextProvider.Current.Modim);

            if (_prayer != Prayer.Arvit)
            {
                Add(CommonPrayerTextProvider.Current.ModimDeRabanan, AppResources.ModimDeRabananTitle);
            }
        }

        protected void AddAlHanissim()
        {
            if (_dayInfo.YomTov == JewishCalendar.CHANUKAH)
            {
                Add(CommonPrayerTextProvider.Current.AlHanissimHannukah, AppResources.AlHanissimHannukahTitle);
            }
            else if (_dayInfo.YomTov == JewishCalendar.PURIM)
            {
                Add(CommonPrayerTextProvider.Current.AlHanissimPurim, AppResources.AlHanissimPurimTitle);
            }
        }

        private void AddPart18()
        {
            AddStringFormat(CommonPrayerTextProvider.Current.SE18, _dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.SE18AYT : string.Empty, _dayInfo.AseretYameyTshuva);
        }

        private void AddBirkatCohanim()
        {
            bool shouldAdd = false;
            string title = null;

            if (_prayer == Prayer.Shacharit || _prayer == Prayer.Mussaf)
            {
                shouldAdd = true;
                title = AppResources.BirkatCohanimTitle;
            }
            else if (IsMinchaInTeanit && _dayInfo.YomTov != JewishCalendar.TISHA_BEAV)
            {
                shouldAdd = true;
                title = AppResources.BirkatCohanimInTeanitTitle;
            }

            if (shouldAdd)
            {
                Add(CommonPrayerTextProvider.Current.BirkatCohanim, title, true);
            }
        }

        protected virtual void AddPart19()
        {
            AddStringFormat(CommonPrayerTextProvider.Current.SE19, _dayInfo.AseretYameyTshuva ? CommonPrayerTextProvider.Current.SE19AYT : string.Empty, _dayInfo.AseretYameyTshuva);
        }

        private void AddEnding1()
        {
            Add(CommonPrayerTextProvider.Current.Yhiu);
            Add(CommonPrayerTextProvider.Current.ElokayNezor);
        }

        private void AddOseShalom()
        {
            _items.Add(PrayersHelper.GetOseShalom(_dayInfo));
        }

        protected virtual void AddEnding2()
        {
            Add(CommonPrayerTextProvider.Current.SE_End);
        }

        protected bool IsMinchaInTeanit
        {
            get { return _prayer == Prayer.Mincha && _dayInfo.Teanit; }
        }

        protected override string GetTitle()
        {
            return _prayer == Prayer.Mussaf ? AppResources.MussafTitle : AppResources.SE_Title;
        }

        public virtual bool IsPart9Bold
        {
            get { return true; }
        }




        protected abstract void AddPart3Musaf();

        private void AddMusafMiddleBlessing1()
        {
            if (_dayInfo.JewishCalendar.RoshChodesh)
            {
                Add(CommonPrayerTextProvider.Current.MussafRoshChodeshSE1);
                AddStringFormat(CommonPrayerTextProvider.Current.MussafRoshChodeshSE2, _dayInfo.IsIbburTime ? CommonPrayerTextProvider.Current.MussafRoshChodeshInIbbur : string.Empty);
            }
            else
            {
                string holidayText1 = null;
                string holidayText2 = null;

                if (_dayInfo.YomTov == JewishCalendar.CHOL_HAMOED_PESACH)
                {
                    holidayText1 = CommonPrayerTextProvider.Current.MussafPesach1;
                    holidayText2 = CommonPrayerTextProvider.Current.YaalehVeyavoPesach;
                }
                else if (_dayInfo.YomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || _dayInfo.YomTov == JewishCalendar.HOSHANA_RABBA)
                {
                    holidayText1 = CommonPrayerTextProvider.Current.MussafSukkot1;
                    holidayText2 = CommonPrayerTextProvider.Current.YaalehVeyavoSukkot;
                }

                AddStringFormat(CommonPrayerTextProvider.Current.MusasfCholHamoed1, holidayText1);
                AddStringFormat(CommonPrayerTextProvider.Current.MusasfCholHamoed2, holidayText2);

            }
        }

        protected virtual void AddMussafHolidayPart()
        {
            Add(CommonPrayerTextProvider.Current.MusasfCholHamoed3);
            Add(CommonPrayerTextProvider.Current.MusasfCholHamoed4);
        }
    }
}
