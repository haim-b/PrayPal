using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PrayPal.Droid;
using PrayPal.TextPresenter;
using Xamarin.Forms.Platform.Android;
using XFListView = Xamarin.Forms.ListView;

[assembly: Xamarin.Forms.ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]
namespace PrayPal.Droid
{
    public class ExtendedListViewRenderer : ListViewRenderer
    {
        public ExtendedListViewRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XFListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Scroll += OnNativeScroll; ;
            }
        }

        private void OnNativeScroll(object sender, AbsListView.ScrollEventArgs e)
        {
            ExtendedListView xlv = (ExtendedListView)Element;

            if (xlv == null || !xlv.IsGroupingEnabled)
            {
                return;
            }

            object group = xlv.GetItemGroup((int)e.View.PointToRowId(0, 0));// e.FirstVisibleItem);

            xlv.ActiveGroup = group;
        }
    }
}