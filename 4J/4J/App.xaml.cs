using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _4J.Services;
using _4J.Views;

namespace _4J
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
