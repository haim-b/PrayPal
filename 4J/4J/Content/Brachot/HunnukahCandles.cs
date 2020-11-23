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

        protected override Task CreateOverrideAsync()
        {
            int candleIndex = DayInfo.JewishCalendar.DayOfChanukah;

            AddInstruction(candleIndex);

            AddBlessing(candleIndex);

            AddHanerotHalalu();

            AddMaozTzur();

            return Task.FromResult(0);
        }

        private void AddInstruction(int candleIndex)
        {
            char[] lightingOrder = new char[8];

            for (int i = 0; i < 8; i++)
            {
                lightingOrder[i] = GetCandleChar(candleIndex - i);
            }

            ParagraphModel[] instruction = new ParagraphModel[2];

            instruction[0] = new ParagraphModel(AppResources.HannukahCandlesOrderInstruction);

            instruction[1] = new ParagraphModel(null, new RunModel(new string(lightingOrder)));// { IsLtr = true };

            Add(AppResources.HannukahCandlesOrderTitle, instruction);
        }

        private char GetCandleChar(int candle)
        {
            if (candle <= 0)
            {
                return (char)0x25cb;
            }

            return (char)(0x2775 + candle);
        }

        private void AddBlessing(int candleIndex)
        {
            string candleName = AppResources.ResourceManager.GetString("HannukaCandle" + candleIndex);

            Add(string.Format(AppResources.HannukahCandleBlessingTitle, candleName), CommonPrayerTextProvider.Current.HannukahCandlesBlessing);

            if (DayInfo.JewishCalendar.JewishDayOfMonth == 25)
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
