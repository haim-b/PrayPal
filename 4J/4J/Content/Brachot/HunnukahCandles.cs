using PrayPal.Common;
using PrayPal.Models;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Content.Brachot
{
    [TextName(PrayerNames.HannukahCandles)]
    [Nusach(Nusach.Ashkenaz, Nusach.Baladi, Nusach.Sfard)]
    public class HunnukahCandles : SpansPrayerBase
    {
        protected override string GetTitle()
        {
            return AppResources.HadlakatNerotHannukahTitle;
        }

        public override bool IsDayCritical
        {
            get { return true; }
        }

        protected override Task CreateOverride()
        {
            int candleIndex = _dayInfo.JewishCalendar.DayOfChanukah;

            AddInstruction(candleIndex);

            AddBlessing(candleIndex);

            AddHanerotHalalu();

            AddMaozTzur();

            return Task.FromResult(0);
        }

        private void AddInstruction(int candleIndex)
        {
            char[] lightingOrder = new char[8];

            int i = 1;

            // Insert empty candles:
            for (; i <= 8 - candleIndex; i++)
            {
                lightingOrder[i - 1] = (char)0x25cb;
            }

            // Insert real candles:
            for (; i <= 8; i++)
            {
                lightingOrder[i - 1] = (char)(0x2775 + i - (8 - candleIndex));
            }

            ParagraphModel[] instruction = new ParagraphModel[2];

            instruction[0] = new ParagraphModel(AppResources.HannukahCandlesOrderInstruction);

            instruction[1] = new ParagraphModel(null, new RunModel(new string(lightingOrder)) { IsLtr = true });

            Add(AppResources.HannukahCandlesOrderTitle, instruction);
        }

        private void AddBlessing(int candleIndex)
        {
            string candleName = AppResources.ResourceManager.GetString("HannukaCandle" + candleIndex);

            Add(string.Format(AppResources.HannukahCandleBlessingTitle, candleName), CommonPrayerTextProvider.Current.HannukahCandlesBlessing);

            if (_dayInfo.JewishCalendar.JewishDayOfMonth == 25)
            {
                Add(AppResources.ShehecheyanuTitle, CommonPrayerTextProvider.Current.Shehecheyanu);
            }
        }

        protected virtual void AddHanerotHalalu()
        {
            Add(AppResources.HanerotHalaluTitle, CommonPrayerTextProvider.Current.HanerotHalalu);
        }

        private void AddMaozTzur()
        {
            Add(AppResources.MaozTzurTitle, CommonPrayerTextProvider.Current.MaozTzur);
        }

        [Nusach(Nusach.EdotMizrach)]
        public class HannukahCandlesEdotHaMizrach : HunnukahCandles
        {
            protected override void AddHanerotHalalu()
            {
                base.AddHanerotHalalu();

                _items.Add(PrayersHelper.GetPsalm(30));
            }
        }
    }
}
