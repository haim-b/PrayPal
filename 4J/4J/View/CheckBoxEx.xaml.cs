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
    public partial class CheckBoxEx : ContentView
    {
        public CheckBoxEx()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
               BindableProperty.Create(nameof(Text), typeof(string), typeof(CheckBoxEx));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBoxEx), false, BindingMode.TwoWay);

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
        }
    }
}