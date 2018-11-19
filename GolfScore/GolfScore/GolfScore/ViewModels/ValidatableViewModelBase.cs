using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using TeeScore.Contracts;
using INavigationService = TeeScore.Contracts.INavigationService;

namespace TeeScore.ViewModels
{
    public abstract class ValidatableViewModelBase: MyViewModelBase
    {
        protected ValidatableViewModelBase(IDataService dataService, INavigationService navigationService): base(dataService, navigationService)
        {
        }

        protected abstract void AddValidations();

        protected abstract bool Validate();

    }


}
