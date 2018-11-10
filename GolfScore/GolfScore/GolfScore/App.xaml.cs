

using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GolfScore
{


    public partial class App : Application
    {
        private static MobileServiceClient _mobileServiceClient;

        public App()
        {

            InitializeComponent();

            MainPage = new MainPage();
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
#if DEBUG
                    var url = GolfScore.Properties.Resources.DebugServiceUrl;
#else
                var url = GolfScore.Properties.Resources.ReleaseServiceUrl;
#endif
                    _mobileServiceClient = new MobileServiceClient(url);
                }

                return _mobileServiceClient;


            }
        }
    }
}
