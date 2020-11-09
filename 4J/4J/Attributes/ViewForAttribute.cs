using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace PrayPal
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewForAttribute : Attribute
    {
        public ViewForAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; }
    }
}
