using PrayPal.TextPresenter;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PrayPal
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var vm = BindingContext as MainViewModel;

            if (vm == null)
            {
                return;
            }

            if (vm.CurrentView == vm.Settings)
            {
                tab.CurrentItem = settingsTabItem;
            }
        }
    }
}
