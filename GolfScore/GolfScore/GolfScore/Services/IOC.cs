using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using GolfScore.Contracts;
using INavigationService = GolfScore.Contracts.INavigationService;

namespace GolfScore.Services
{
    public class Ioc
    {
        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();

        public Ioc()
        {
            SimpleIoc.Default.Unregister<GalaSoft.MvvmLight.Views.INavigationService>();
            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register<Contracts.INavigationService>(() => navigationService);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
        }

        static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            return navigationService;
        }
    }
}
