using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.Books.Psalms
{
    [ViewFor(typeof(PsalmSelectionPageViewModel))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PsalmSelectionPageView : ContentPage
    {
        public PsalmSelectionPageView()
        {
            InitializeComponent();
        }
    }
}