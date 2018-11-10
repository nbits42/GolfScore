using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;
using INavigationService = GolfScore.Contracts.INavigationService;

namespace GolfScore.Services
{
    public class ViewModelLocator
    {
        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
        public RallyViewModel Rally => SimpleIoc.Default.GetInstance<RallyViewModel>();
        public StageViewModel Stage => SimpleIoc.Default.GetInstance<StageViewModel>();
        public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();
        public NewsViewModel News => SimpleIoc.Default.GetInstance<NewsViewModel>();
        public HomeViewModel Home => SimpleIoc.Default.GetInstance<HomeViewModel>();
        public GalleriesViewModel Galleries => SimpleIoc.Default.GetInstance<GalleriesViewModel>();


        static ViewModelLocator()
        {
            SimpleIoc.Default.Unregister<INavigationService>();
            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            var restservice = DependencyService.Get<IRestService>();

//            SimpleIoc.Default.Register<IRestService> (() => restservice);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
                SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RallyViewModel>();
            SimpleIoc.Default.Register<StageViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<NewsViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<GalleriesViewModel>();
            SimpleIoc.Default.Register<ILocalDataService, LocalDataService>();
        }

        static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
//#if WINDOWS_PHONE_APP
//            navigationService.Configure("RallyDetails", typeof(RallyDetailsPage));
//            navigationService.Configure("StageDetails", typeof(StageDetailsPage));
//#else
//            navigationService.Configure("RallyDetails", typeof(MainPage));
//#endif
            // navigationService.Configure("key1", typeof(OtherPage1));
            // navigationService.Configure("key2", typeof(OtherPage2));

            return navigationService;
        }
    }

}
