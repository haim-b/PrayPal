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
using Xamarin.Essentials;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Serilog.Sink.AppCenter;
using PrayPal.Helpers;
using System.Globalization;

namespace PrayPal
{
    public partial class App : Application, ISettingsListener
    {
        private readonly MainViewModel _mainViewModel;
        private static readonly Autofac.IContainer _container;
        private OSAppTheme _osTheme;

        static App()
        {
            // Android exposes Hebrew as iw-IL, which is illegal in .Net, so we need to fix it:
            if (CultureInfo.CurrentCulture == CultureInfo.InvariantCulture) // iw-IL makes it fallback to invariant
            {
                CultureInfo.CurrentCulture = new CultureInfo("he-IL");
            }

#if !DEBUG
            AppCenter.Start($"android={Secrets.AndroidSecredKey};" +
                      "uwp={Your UWP App secret here};" +
                      "ios={Your iOS App secret here}",
                      typeof(Analytics), typeof(Crashes));
#endif

            var logPath = Path.Combine(FileSystem.AppDataDirectory, "log.txt");

            Serilog.Core.Logger logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .Enrich.FromLogContext()
               .WriteTo.File(logPath)
               .WriteTo.AppCenterSink(new Serilog.Core.LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug), target: AppCenterTarget.ExceptionsAsCrashes, appCenterSecret: Secrets.AndroidSecredKey)
               .CreateLogger();

            IServiceCollection services = new ServiceCollection();
            services.AddLogging(l => l.AddSerilog(logger));

            Settings.SetSettingsProvider(new XamarinSettingsProvider());

            Assembly[] assemblies = new[]{
                Assembly.GetExecutingAssembly(),
                typeof(Nusach).Assembly
            };

            var builder = new ContainerBuilder();

            Type prayerInterfaceType = typeof(IPrayer);
            Type textProvider = typeof(CommonPrayerTextProvider);

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.Namespace.StartsWith("PrayPal") && !prayerInterfaceType.IsAssignableFrom(t) && !textProvider.IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetCustomAttribute<TextNameAttribute>() != null)
                .AsImplementedInterfaces()
                .WithCorrectMetadataFrom<TextNameAttribute>()
                .WithCorrectMetadataFrom<NusachAttribute>();

            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.Populate(services);
            _container = builder.Build();

            var viewsWithViewModels = _container.ComponentRegistry.Registrations.Select(r => r.Activator.LimitType).Where(t => t.GetCustomAttributes<ViewForAttribute>().Any());

            foreach (var viewType in viewsWithViewModels)
            {
                foreach (var vfa in viewType.GetCustomAttributes<ViewForAttribute>())
                {
                    Routing.RegisterRoute(vfa.ViewModelType.Name, new LocalRouteFactory(() => (Element)_container.Resolve(viewType), () => _container.Resolve(vfa.ViewModelType)));
                }
            }
        }

        public App()
        {
            _osTheme = RequestedTheme;

            OnSettingsChanged(nameof(Settings.Theme));

            InitializeComponent();

            _mainViewModel = _container.Resolve<MainViewModel>();

            PrayersHelper.SetPrayerTextProvider(Settings.Nusach);
            Settings.RegisterListener(this);

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

        public void OnSettingsChanged(string settingName)
        {
            if (settingName != nameof(Settings.Theme))
            {
                return;
            }

            OSAppTheme theme = OSAppTheme.Unspecified;

            switch (Settings.Theme)
            {
                case Theme.FromOS:
                    theme = _osTheme;
                    break;
                case Theme.Light:
                    theme = OSAppTheme.Light;
                    break;
                case Theme.Dark:
                    theme = OSAppTheme.Dark;
                    break;
                default:
                    break;
            }

            UserAppTheme = theme;
        }
    }
}
