using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using GolfScore.Services;
using INavigationService = GolfScore.Contracts.INavigationService;

namespace GolfScore.ViewModels
{
    public class MyViewModelBase: ViewModelBase
    {
        public bool IsDirty { get; set; }

        protected IDataService DataService;
        protected INavigationService NavigationService;

        public MyViewModelBase(IDataService dataservice, INavigationService navigationService)
        {
            DataService = dataservice;
            NavigationService = navigationService;
        }

        protected IDialogService DialogService => SimpleIoc.Default.GetInstance<IDialogService>();

        protected void ShowError(Exception ex, string title, string buttonText, Action afterHideCallBack)
        {
            DialogService.ShowError(ex, title, buttonText, afterHideCallBack);
        }
    }


}
