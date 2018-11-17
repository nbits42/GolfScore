﻿using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using TeeScore.Contracts;
using TeeScore.ViewModels;
using INavigationService = TeeScore.Contracts.INavigationService;

namespace TeeScore.Services
{
    public class Ioc
    {
        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
        public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public Ioc()
        {
            SimpleIoc.Default.Unregister<GalaSoft.MvvmLight.Views.INavigationService>();
            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register<Contracts.INavigationService>(() => navigationService);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            

            return navigationService;
        }
    }
}
