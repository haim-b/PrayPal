using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal.TextPresenter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ViewFor(typeof(TextPresenterViewModel))]
    public partial class TextPresenterView : ContentPage
    {
        public TextPresenterView()
        {
            InitializeComponent();
        }

        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            object activeGroup = GroupingHelper.GetActiveGroup((ListView)sender);
        }
    }
}
