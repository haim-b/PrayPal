using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Resources;

namespace PrayPal.Content.Prayers.BedtimeShma
{
    [TextName(PrayerNames.BedtimeShma)]
    public abstract class BedtimeShmaBase : ParagraphsPrayerBase
    {
        protected override string GetTitle()
        {
            return AppResources.BedtimeShmaTitle;
        }
    }
}
