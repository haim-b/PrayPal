using Autofac.Features.AttributeFilters;
using PrayPal.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace PrayPal
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TextNameAttribute : Attribute
    {
        public TextNameAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Name = name;
        }

        public string Name { get; set; }
    }
}
