using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(PrayPal.Droid.LabelRendererEx))]
namespace PrayPal.Droid
{
    public class LabelRendererEx : LabelRenderer
    {
        public LabelRendererEx(Context context)
            : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            // Android labels have unnecessary extra padding, which we don't want"
            Control?.SetIncludeFontPadding(false);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O && Element != null && Control != null)
            {
                if (Properties.GetIsTextStretched(Element))
                {
                    Control.JustificationMode = JustificationMode.InterWord;
                }
            }
        }
    }
}