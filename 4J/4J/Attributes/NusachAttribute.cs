using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace PrayPal
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NusachAttribute : Attribute
    {
        public NusachAttribute(params Nusach[] nusach)
        {
            if ((nusach?.Length).GetValueOrDefault() == 0)
            {
                throw new ArgumentException("At least one Nusach should be provided.");
            }

            Nusach = nusach;
        }

        public Nusach[] Nusach { get; set; }
    }
}
