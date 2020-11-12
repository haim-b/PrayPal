﻿using System;
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
    }
}