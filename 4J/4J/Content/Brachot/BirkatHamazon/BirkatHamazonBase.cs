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
    [PrayerName(PrayerNames.BirkatHamazon)]
    public abstract class BirkatHamazonBase : ParagraphsPrayerBase
    {
        protected async override Task CreateOverrideAsync()
        {
            AddOpening();
            AddPart1();
            AddPart2();
            AddPart3();
            AddPart4();
            AddEnding();
        }

        protected override string GetTitle()
        {
            return AppResources.BirkatHamazonTitle;
        }

        protected abstract void AddOpening();

        private void AddPart1()
        {
            Add(CommonPrayerTextProvider.Current.Zimmun, AppResources.ZimmunTitle, true);

            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P1, AppResources.BirkatHamazonTitle);
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P2);

            if (_dayInfo.YomTov == JewishCalendar.CHANUKAH)
            {
                Add(CommonPrayerTextProvider.Current.AlHanissimHannukah, AppResources.AlHanissimHannukahTitle);
            }
            else if (_dayInfo.YomTov == JewishCalendar.PURIM)
            {
                Add(CommonPrayerTextProvider.Current.AlHanissimPurim, AppResources.AlHanissimPurimTitle);
            }

            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P3);
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P4);

            ParagraphModel yaalehVeYavo = PrayersHelper.GetYaalehVeYavo(_dayInfo);

            if (yaalehVeYavo != null)
            {
                _items.Add(yaalehVeYavo);
            }

            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P5);
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P6);
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P7);


        }

        protected virtual void AddPart2()
        {
            Add(CommonPrayerTextProvider.Current.BirkatHamazon_P8);
        }

        protected virtual void AddPart3()
        { }

        protected virtual void AddPart4()
        {
            if (_dayInfo.JewishCalendar.RoshChodesh)
            {
                Add(CommonPrayerTextProvider.Current.BirkatHamazon_RoshHodesh);
            }
            else if (_dayInfo.YomTov == JewishCalendar.SUCCOS || _dayInfo.YomTov == JewishCalendar.CHOL_HAMOED_SUCCOS || _dayInfo.YomTov == JewishCalendar.HOSHANA_RABBA)
            {
                Add(CommonPrayerTextProvider.Current.BirkatHamazon_Sukkot);
            }

            string magdil = CommonPrayerTextProvider.Current.Magdil;

            if (_dayInfo.JewishCalendar.RoshChodesh || _dayInfo.IsCholHamoed || _dayInfo.YomTov == JewishCalendar.CHANUKAH)
            {
                magdil = CommonPrayerTextProvider.Current.Migdol;
            }

            AddStringFormat(CommonPrayerTextProvider.Current.BirkatHamazon_P10, magdil);
        }

        protected virtual void AddEnding()
        { }
    }
}
