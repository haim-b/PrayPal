using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.DayTimes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayTimesPageView : ContentPage
    {
        public DayTimesPageView()
        {
            InitializeComponent();
        }
    }
}
