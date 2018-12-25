using System;
using System.Linq;
using System.Threading.Tasks;
using TeeScore.Contracts;
using Xamarin.Forms;

namespace TeeScore.Services
{
    public abstract class BaseDialogService : IDialogService
    {
        public Page CurrentPage => GetCurrentPage();

        protected BaseDialogService()
        {
        }

        protected abstract Page GetCurrentPage();

        #region IDialogService implementation

        public void ShowError(string message, string title)
        {
            CurrentPage.DisplayAlert(title, message,"OK");
        }

        public void ShowError(Exception error, string title)
        {
            CurrentPage.DisplayAlert(title, error.Message, "OK");
        }

        public async Task<bool> ShowMessage(string message, string title, string acceptButtonText, string cancelButtonText)
        {
            return await CurrentPage.DisplayAlert(title, message, acceptButtonText,cancelButtonText);
        }

        #endregion
    }
}
