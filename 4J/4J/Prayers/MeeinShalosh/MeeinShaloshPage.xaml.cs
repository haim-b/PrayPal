using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.Prayers.MeeinShalosh
{
    [ViewFor(typeof(MeeinShaloshPageViewModel))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeeinShaloshPage : ContentPage
    {
        public MeeinShaloshPage()
        {
            InitializeComponent();
        }
    }
}