using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayPal
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class NusachAttribute : Attribute
    {
        public NusachAttribute(Nusach nusach)
        {
            Nusach = nusach;
        }

        public Nusach Nusach { get; set; }
    }
}
