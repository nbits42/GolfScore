using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GolfScore.Contracts;
using Xamarin.Forms;

namespace GolfScore.Services
{
    public class NavigationService : INavigationService
    {
        Dictionary<string, Type> Pages { get; set; }

        string _currentPageKey;

        public NavigationPage MainPage => new NavigationPage(Application.Current.MainPage);

        public NavigationService()
        {
            Pages = new Dictionary<string, Type>
            {
                {"RalliesPage", typeof(Pages.RalliesPage)},
                {"RallyDetails", typeof(Pages.DetailsPage)},
            };

        }

        #region INavigationService implementation

        public async Task GoBack()
        {
            if (MainPage.Navigation.ModalStack.Count > 0)
            {
                await MainPage.Navigation.PopModalAsync().ConfigureAwait(false);
            }
            else
            {
                await MainPage.Navigation.PopAsync().ConfigureAwait(false);
            }
        }

        public async Task NavigateTo(string pageKey)
        {
            await NavigateTo(pageKey, null).ConfigureAwait(false);
        }

        public async Task NavigateTo(string pageKey, object parameter)
        {
            try
            {
                object[] parameters = null;
                if (parameter != null)
                {
                    parameters = new object[] { parameter };
                }
                var displayPage = (Page)Activator.CreateInstance(Pages[pageKey], parameters);
                _currentPageKey = pageKey;
                var isModal = displayPage is IModalPage;
                if (isModal)
                {
                    await MainPage.Navigation.PushModalAsync(displayPage).ConfigureAwait(false);
                }
                else
                {
                    await MainPage.Navigation.PushAsync(new NavigationPage(displayPage)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string CurrentPageKey => _currentPageKey;

        #endregion
    }
}