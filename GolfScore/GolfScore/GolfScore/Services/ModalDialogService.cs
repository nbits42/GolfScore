using System;
using System.Linq;
using GalaSoft.MvvmLight.Views;
using TeeScore.Contracts;
using Xamarin.Forms;

namespace TeeScore.Services
{
    public class ModalDialogService :BaseDialogService,  IModalDialogService
    {
        protected  override Page GetCurrentPage()
        {
            return Application.Current.MainPage.Navigation.ModalStack.Count > 0 
                ? Application.Current.MainPage.Navigation.ModalStack.LastOrDefault() 
                : Application.Current.MainPage;
        }
    }
}
