using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using TeeScore.Contracts;
using TeeScore.ViewModels;
using IDialogService = TeeScore.Contracts.IDialogService;
using INavigationService = TeeScore.Contracts.INavigationService;

namespace TeeScore.Services
{
    public class Ioc
    {
        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
        public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();
        public NewGameViewModel NewGame => SimpleIoc.Default.GetInstance<NewGameViewModel>();
        public PlayGameViewModel PlayGame => SimpleIoc.Default.GetInstance<PlayGameViewModel>();
        public VenueViewModel Venue => SimpleIoc.Default.GetInstance<VenueViewModel>();
        public GamePlayerViewModel GamePlayer => SimpleIoc.Default.GetInstance<GamePlayerViewModel>();

        public Ioc()
        {
            SimpleIoc.Default.Unregister<GalaSoft.MvvmLight.Views.INavigationService>();
            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register<Contracts.INavigationService>(() => navigationService);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IModalDialogService, ModalDialogService>();
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<VenueViewModel>();
            SimpleIoc.Default.Register<NewGameViewModel>();
            SimpleIoc.Default.Register<PlayGameViewModel>();
            SimpleIoc.Default.Register<GamePlayerViewModel>();
        }

        static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            

            return navigationService;
        }
    }
}
