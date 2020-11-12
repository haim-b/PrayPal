using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrayPal.TextPresenter
{
    public static class GroupingHelper
    {
        public static readonly BindableProperty ActiveGroupProperty =
                     BindableProperty.CreateAttached("ActiveGroup", typeof(object), typeof(GroupingHelper), null);

        public static object GetActiveGroup(BindableObject view)
        {
            return view.GetValue(ActiveGroupProperty);
        }

        public static void SetActiveGroup(BindableObject view, object value)
        {
            view.SetValue(ActiveGroupProperty, value);
        }
    }
}
