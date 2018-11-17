using System.Threading.Tasks;

namespace TeeScore.Contracts
{
    public interface INavigationService
    {
        Task GoBack();
        Task NavigateTo(string pageKey);
        Task NavigateTo(string pageKey, object parameter);
        string CurrentPageKey { get; }
    }
}
