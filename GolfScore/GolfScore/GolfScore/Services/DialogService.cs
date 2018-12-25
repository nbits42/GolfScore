using System;
using System.Linq;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;
using IDialogService = TeeScore.Contracts.IDialogService;

namespace TeeScore.Services
{
    public class DialogService :BaseDialogService,  IDialogService
    {
        protected  override Page GetCurrentPage()
        {
            return Application.Current.MainPage.Navigation.NavigationStack.Count > 0 
                ? Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() 
                : Application.Current.MainPage;
        }

      
    }

}
