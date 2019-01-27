using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using TeeScore.Contracts;

namespace TeeScore.ViewModels
{
    public class JoinGameViewModel: MyViewModelBase
    {
        private readonly IDialogService _dialogService;
        private int _invitationNumber;
        private bool _isWaiting;

        public JoinGameViewModel(IDataService dataService, INavigationService navigationService, IDialogService dialogService) : base(dataService, navigationService)
        {
            _dialogService = dialogService;
        }

        /* =========================================== property: InvitationNumber ====================================== */
        /// <summary>
        /// Sets and gets the InvitationNumber property.
        /// </summary>
        public int InvitationNumber
        {
            get => _invitationNumber;
            set
            {
                if (value == _invitationNumber)
                {
                    return;
                }

                _invitationNumber = value;
                RaisePropertyChanged();
            }
        }

        /* =========================================== property: IsWaiting ====================================== */
        /// <summary>
        /// Sets and gets the IsWaiting property.
        /// </summary>
        public bool IsWaiting
        {
            get => _isWaiting;
            set
            {
                if (value == _isWaiting)
                {
                    return;
                }

                _isWaiting = value;
                RaisePropertyChanged();
            }
        }




    }
}
