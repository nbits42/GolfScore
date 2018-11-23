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
        private bool _isValid;

        protected ValidatableViewModelBase(IDataService dataService, INavigationService navigationService): base(dataService, navigationService)
        {
        }

        protected abstract void AddValidations();

        public abstract bool Validate();

        /* =========================================== property: IsValid ====================================== */
        /// <summary>
        /// Sets and gets the IsValid property.
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (value == _isValid)
                {
                    return;
                }

                _isValid = value;
                RaisePropertyChanged();
            }
        }



    }


}
