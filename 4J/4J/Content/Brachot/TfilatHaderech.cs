using PrayPal.Common;
using PrayPal.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayPal.Content.Brachot
{
    [PrayerName(PrayerNames.TfilatHaDerech)]
    [Nusach(Nusach.Ashkenaz)]
    [Nusach(Nusach.Baladi)]
    [Nusach(Nusach.EdotMizrach)]
    [Nusach(Nusach.Sfard)]
    public class TfilatHaderech : ParagraphsPrayerBase
    {
        protected override string GetTitle()
        {
            return AppResources.TfilatHaderechTitle;
        }

        protected override Task CreateOverrideAsync()
        {
            Add(CommonPrayerTextProvider.Current.TfilatHaderech);

            return Task.FromResult(0);
        }
    }
}
