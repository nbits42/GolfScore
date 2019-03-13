using System.Globalization;
using AutoMapper;
using Microsoft.WindowsAzure.MobileServices;
using TeeScore.Contracts;
using TeeScore.Mapping;
using TeeScore.Services;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TeeScore
{
    public partial class App : Application
    {
        private static MobileServiceClient _mobileServiceClient;
        private static Ioc _ioc;
        public static CultureInfo CurrentCulture;
        public static DeviceType DeviceType = DeviceType.Droid;
        public static ICalendarService CalendarService;
        public static MainViewModel viewModel;
        public static bool AutoMapperInitialized;
        public static Ioc IOC => _ioc ?? (_ioc = new Ioc());

        public App()
        {
            LogWarningsToApplicationOutput = true;

            // https://www.syncfusion.com/account/downloads  & keys
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI0MTZAMzEzNjJlMzQyZTMwWmJEWFBiZGhoSlcraHZuYVJUUitKU3ArcmR6c3Rwc0s3RjBNVUwrZk55UT0="); // version 16.4.*


            // Setting up for translation
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                Translations.Labels.Culture = ci; // set the RESX for resource localization
                Translations.Messages.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
                CurrentCulture = ci;
            }
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    DeviceType = DeviceType.Droid;
                    break;
                case Device.UWP:
                    DeviceType = DeviceType.Uwp;
                    break;
                case Device.iOS:
                    DeviceType = DeviceType.Ios;
                    break;
            }

            viewModel = IOC.Main;

            if (!AutoMapperInitialized)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<DefaultProfile>();
                });
                AutoMapperInitialized = true;
            }

            CalendarService = DependencyService.Get<ICalendarService>();


            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            ((NavigationPage) MainPage).BarBackgroundColor = (Color)Resources["PageHeadingBackgroundColor"];
            ((NavigationPage) MainPage).BarTextColor = (Color)Resources["PageHeadingTextColor"];
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await viewModel.LoadAsync().ConfigureAwait(false);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            await viewModel.LoadAsync().ConfigureAwait(false);
        }

        public static MobileServiceClient MobileService
        {
            get
            {
                if (_mobileServiceClient == null)
                {
//#if DEBUG
//                    var url = GolfScore.Properties.Resources.DebugServiceUrl;
//#else
                var url = TeeScore.Properties.Resources.ReleaseServiceUrl;
//#endif
                    _mobileServiceClient = new MobileServiceClient(url);
                }

                return _mobileServiceClient;


            }
        }
    }
}
