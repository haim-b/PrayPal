using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    public class Properties
    {

        public static bool GetIsTextStretched(Label obj)
        {
            return (bool)obj.GetValue(IsTextStretchedProperty);
        }

        public static void SetIsTextStretched(Label obj, bool value)
        {
            obj.SetValue(IsTextStretchedProperty, value);
        }

        public static readonly BindableProperty IsTextStretchedProperty =
            BindableProperty.CreateAttached("IsTextStretched", typeof(bool), typeof(Utils), false);


    }
}
