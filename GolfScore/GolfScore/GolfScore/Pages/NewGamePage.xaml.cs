using System;
using System.Linq;
using Syncfusion.Licensing.crypto;
using Syncfusion.ListView.XForms;
using TeeScore.Domain;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewGamePage : ContentPage
	{
	    private readonly NewGameViewModel _vm;
        private string _searchText;

        public NewGamePage ()
		{
			InitializeComponent ();
		    _vm = App.IOC.NewGame;
		    BindingContext = _vm;
            _vm.NewGame();
		}

	    protected override async void OnAppearing()
	    {
	        base.OnAppearing();
	        await _vm.LoadAsync().ConfigureAwait(true);
	    }

	    private async void AddVenueButton_OnClicked(object sender, EventArgs e)
	    {
	        var venuePage = new VenuePage(string.Empty);
	        await Navigation.PushModalAsync(venuePage);
	    }

	    private void VenueSearch_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
	        if (sender is Entry searchBox && VenuesView.DataSource != null)
	        {
	            _searchText = searchBox.Text;
	            VenuesView.DataSource.Filter = FilterVenues;
                VenuesView.DataSource.RefreshFilter();
	        }
	    }

        private bool FilterVenues(object obj)
        {
            if (string.IsNullOrEmpty(_searchText))
            {
                return true;
            }

            return obj is Venue venue && (venue.Name.ToLower().Contains(_searchText.ToLower()));
        }

	    private void VenuesView_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
	    {
	        var item = e.AddedItems.FirstOrDefault();
	        _vm.SelectedVenue = item as Venue;
	    }

	    private void AddPlayerButton_OnClicked(object sender, EventArgs e)
	    {
	        
	    }

	    private void PlayerSearch_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
	        
	    }
	}
}