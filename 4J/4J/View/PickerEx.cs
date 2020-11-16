using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace PrayPal
{
    public class PickerEx : Picker
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                InvalidateMeasure();
            }
        }
    }
}
