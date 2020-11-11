using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using _4J.Services;
using _4J.Views;
using Autofac;
using Autofac.Features.ResolveAnything;
using Yahadut;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using PrayPal.Common;
using PrayPal.Content;
using System.Linq;

namespace PrayPal
{
    public partial class App : Application
    {
        private readonly MainViewModel _mainViewModel;


        public App()
        {
            InitializeComponent();

            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "log.txt");

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File(logPath)
                .CreateLogger();

            IServiceCollection services = new ServiceCollection();
            services.AddLogging(l => l.AddSerilog(logger));

            Assembly[] assemblies = new[]{
                Assembly.GetExecutingAssembly(),
                typeof(Nusach).Assembly
            };

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Namespace.StartsWith("PrayPal") && t.GetCustomAttribute<TextNameAttribute>() == null && !t.Name.EndsWith("TextProvider"))
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetCustomAttribute<TextNameAttribute>() != null)
                .AsImplementedInterfaces()
                .WithCorrectMetadataFrom<TextNameAttribute>()
                .WithCorrectMetadataFrom<NusachAttribute>();

            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.Populate(services);
            var container = builder.Build();

            DependencyService.Register<MockDataStore>();
            _mainViewModel = container.Resolve<MainViewModel>();

            var viewsWithViewModels = container.ComponentRegistry.Registrations.Select(r => r.Activator.LimitType).Where(t => t.GetCustomAttributes<ViewForAttribute>().Any());

            foreach (var viewType in viewsWithViewModels)
            {
                foreach (var vfa in viewType.GetCustomAttributes<ViewForAttribute>())
                {
                    Routing.RegisterRoute(vfa.ViewModelType.Name, new LocalRouteFactory(() => (Element)container.Resolve(viewType), () => container.Resolve(vfa.ViewModelType)));
                }
            }

            PrayersHelper.SetPrayerTextProvider(Settings.Nusach);

            MainPage = new AppShell() { BindingContext = _mainViewModel };
        }

        protected override async void OnStart()
        {
            await _mainViewModel.GenerateContentAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
