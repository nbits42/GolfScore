using System;
using System.Threading.Tasks;

namespace TeeScore.Contracts
{
    public interface IDialogService
    {
        void ShowError(string message, string title);
        void ShowError(Exception error, string title);
        Task<bool> ShowMessage(string message, string title, string acceptButtonText, string cancelButtonText);
    }
}