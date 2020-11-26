using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace PrayPal.Droid
{
    [Activity(Label = "PrayPal", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().DetectAll().PenaltyLog().Build());
            //StrictMode.SetVmPolicy(new StrictMode.VmPolicy.Builder().DetectLeakedSqlLiteObjects().DetectLeakedClosableObjects().PenaltyLog().PenaltyDeath().Build());

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental", "SwipeView_Experimental", "Expander_Experimental");

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            //Intent i = new Intent();
            //i.SetPackage("com.google.android.apps.googleassistant");
            //i.SetClassName("com.google.android.apps.googleassistant", "com.google.android.apps.googleassistant.AssistantActivity");
            //StartActivity(i);

            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

    }
}