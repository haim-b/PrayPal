using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yahadut.DayTimes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayTimesView : ContentPage
    {
        public DayTimesView()
        {
            InitializeComponent();
        }
    }
}
