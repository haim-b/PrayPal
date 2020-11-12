using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrayPal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchEx : ContentView
    {
        public SwitchEx()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TitleProperty =
               BindableProperty.Create(nameof(Title), typeof(string), typeof(SwitchEx));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }


        public static readonly BindableProperty DetailsProperty =
            BindableProperty.Create(nameof(Details), typeof(string), typeof(SwitchEx));

        public string Details
        {
            get => (string)GetValue(DetailsProperty);
            set => SetValue(DetailsProperty, value);
        }


        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(SwitchEx), false, BindingMode.TwoWay);

        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;
        }
    }
}