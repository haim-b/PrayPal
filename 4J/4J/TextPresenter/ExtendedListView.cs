using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PrayPal.TextPresenter
{
    public class ExtendedListView : ListView
    {
        public static readonly BindableProperty ActiveGroupProperty =
                     BindableProperty.Create("ActiveGroup", typeof(object), typeof(ExtendedListView), null);

        public static readonly BindableProperty ActiveGroupStartYPositionProperty =
                     BindableProperty.Create("ActiveGroupStartYPosition", typeof(int), typeof(ExtendedListView), 0);

        public event EventHandler ItemsSourceChanged;

        public ExtendedListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {

        }

        public object ActiveGroup
        {
            get { return GetValue(ActiveGroupProperty); }
            set { SetValue(ActiveGroupProperty, value); }
        }

        public int ActiveGroupStartYPosition
        {
            get { return (int)GetValue(ActiveGroupStartYPositionProperty); }
            set { SetValue(ActiveGroupStartYPositionProperty, value); }
        }

        public object GetItemGroup(int itemIndexIncludingGroups)
        {
            if (itemIndexIncludingGroups < 0 || !IsGroupingEnabled || ItemsSource == null)
            {
                return null;
            }

            int index = 0;

            foreach (IEnumerable<object> group in ItemsSource)
            {
                if (itemIndexIncludingGroups == index++)
                {
                    return group;
                }

                foreach (object item in group)
                {
                    if (itemIndexIncludingGroups == index++)
                    {
                        return group;
                    }
                }
            }

            return null;
        }

        public int VerticalScrollPosition { get; set; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ItemsSource))
            {
                ItemsSourceChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
