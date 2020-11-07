using System;
using System.Collections.Generic;
using _4J.ViewModels;
using _4J.Views;
using Xamarin.Forms;

namespace PrayPal
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
