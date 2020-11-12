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

        private void OnGroupPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Label.Text) || Device.RuntimePlatform != Device.Android)
            {
                return;
            }

            Label label = (Label)sender;

            if (label.BindingContext == lst.ItemsSource?.Cast<object>()?.FirstOrDefault())
            {
                label.IsVisible = false;
            }
        }
    }
}
