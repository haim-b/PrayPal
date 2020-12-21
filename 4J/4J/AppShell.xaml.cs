using PrayPal.TextPresenter;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace PrayPal
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void tab_BindingContextChanged(object sender, EventArgs e)
        {
            TabbedPage tab = (TabbedPage)sender;
            var vm = BindingContext as MainViewModel;

            if (vm == null)
            {
                return;
            }

            tab.CurrentPage = tab.Children.First(p => p.BindingContext == vm.CurrentView);
        }
    }
}
