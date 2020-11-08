using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrayPal.Common;
using PrayPal.Models;

namespace PrayPal.Content.Prayers.Mincha
{
    [Nusach(Nusach.Ashkenaz)]
    public class MinchaAshkenaz : MinchaSfard
    {
        protected override ShmoneEsreBase GetShmoneEsre()
        {
            return new ShmoneEsreAshkenaz(Prayer.Mincha);
        }

        protected override void AddViduyAnd13Midot(SpanModel tachanun)
        {

        }

        protected override void AddLeDavid()
        {

        }
    }
}
