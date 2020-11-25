using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    [ContentProperty(nameof(Template))]
    public class ViewModelTemplate : DataTemplate
    {
        public Type ForType { get; set; }
        public DataTemplate Template { get; set; }
    }
}
