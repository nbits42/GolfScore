using System.Globalization;
using AutoMapper;
using GolfScore.Contracts;
using GolfScore.Mapping;
using GolfScore.Services;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GolfScore
{


    public partial class App : Application
    {
        private static MobileServiceClient _mobileServiceClient;
        private static Ioc _ioc;
        public static CultureInfo CurrentCulture;
        public static DeviceType DeviceType = DeviceType.Droid;
        public static ICalendarService CalendarService;

        public static bool AutomapperInitialized;
        public static Ioc IOC => _ioc ?? (_ioc = new Ioc());

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk3MTZAMzEzNjJlMzMyZTMwZmFha0xUWmJPMHZTYjVkUEwwOWxEaEJ1bjR5WlAyVUhiQkJpbHhmTkx0dz0="); // version 16.3.*

            // Settingup for translation
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

            if (!AutomapperInitialized)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<DefaultProfile>();
                });
                AutomapperInitialized = true;
            }

            CalendarService = DependencyService.Get<ICalendarService>();


            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            ((NavigationPage) MainPage).BarBackgroundColor = Color.DarkGreen;
            ((NavigationPage) MainPage).BarTextColor = Color.White;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
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
                var url = GolfScore.Properties.Resources.ReleaseServiceUrl;
//#endif
                    _mobileServiceClient = new MobileServiceClient(url);
                }

                return _mobileServiceClient;


            }
        }
    }
}
