﻿using System;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace TeeScore.Services
{
    public class DialogService : IDialogService
    {
        public Page CurrentPage => Application.Current.MainPage;

        public DialogService()
        {
        }

        #region IDialogService implementation

        public System.Threading.Tasks.Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            return CurrentPage.DisplayAlert(title, message, buttonText);
        }

        public System.Threading.Tasks.Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            return CurrentPage.DisplayAlert(title, error.Message, buttonText);
        }

        public System.Threading.Tasks.Task ShowMessage(string message, string title)
        {
            return CurrentPage.DisplayAlert(title, message, "OK");
        }

        public System.Threading.Tasks.Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            return CurrentPage.DisplayAlert(title, message, buttonText);
        }

        public System.Threading.Tasks.Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            return CurrentPage.DisplayAlert(title, message, buttonConfirmText, buttonCancelText);
        }

        public System.Threading.Tasks.Task ShowMessageBox(string message, string title)
        {
            return CurrentPage.DisplayAlert(title, message, "OK");
        }

        #endregion
    }
}