using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PrayPal.TextPresenter
{
    public class ExtendedListView : ListView
    {
        public static readonly BindableProperty ActiveGroupProperty =
                     BindableProperty.Create("ActiveGroup", typeof(object), typeof(ExtendedListView), null);

        public ExtendedListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {

        }

        public object ActiveGroup
        {
            get { return GetValue(ActiveGroupProperty); }
            set { SetValue(ActiveGroupProperty, value); }
        }

        public object GetItemGroup(int itemIndex)
        {
            if (itemIndex < 0 || !IsGroupingEnabled || ItemsSource == null)
            {
                return null;
            }

            int index = 0;

            foreach (IEnumerable<object> group in ItemsSource)
            {
                foreach (object item in group)
                {
                    if (itemIndex == index++)
                    {
                        return group;
                    }
                }
            }

            return null;
        }
    }
}
