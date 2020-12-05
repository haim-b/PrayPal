using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.SummaryView
{
    [ViewFor(typeof(SummaryPageViewModel))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryPageView : ContentPage
    {
        public SummaryPageView()
        {
            InitializeComponent();
        }
    }
}
