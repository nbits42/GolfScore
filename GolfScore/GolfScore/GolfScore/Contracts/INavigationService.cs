using System.Threading.Tasks;

namespace GolfScore.Contracts
{
    public interface INavigationService
    {
        Task GoBack();
        Task NavigateTo(string pageKey);
        Task NavigateTo(string pageKey, object parameter);
        string CurrentPageKey { get; }
    }
}
