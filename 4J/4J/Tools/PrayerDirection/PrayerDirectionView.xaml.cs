using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.Tools.PrayerDirection
{
    [ViewFor(typeof(PrayerDirectionViewModel))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayerDirectionView : ContentPage
    {
        public PrayerDirectionView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (Compass.IsMonitoring)
            {
                Compass.Stop();
            }

            return base.OnBackButtonPressed();
        }
    }
}