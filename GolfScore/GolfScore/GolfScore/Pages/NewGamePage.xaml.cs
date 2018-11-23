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
        private string _searchtext;

        public NewGamePage ()
		{
			InitializeComponent ();
		    _vm = App.IOC.NewGame;
		    BindingContext = _vm;
		}

	    protected override async void OnAppearing()
	    {
	        base.OnAppearing();
	        await _vm.LoadAsync();
	    }

	    private async void AddVenueButton_OnClicked(object sender, EventArgs e)
	    {
	        var venuePage = new VenuePage(string.Empty);
	        await Navigation.PushModalAsync(venuePage);
	    }

	    private void Editor_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
	        var searchBox = sender as Entry;
	        if (searchBox != null && VenuesView.DataSource != null)
	        {
	            _searchtext = searchBox.Text;
	            VenuesView.DataSource.Filter = FilterVenues;
                VenuesView.DataSource.RefreshFilter();
	        }
	    }

        private bool FilterVenues(object obj)
        {
            if (string.IsNullOrEmpty(_searchtext))
            {
                return true;
            }

            var venue = obj as Venue;
            return (venue.Name.ToLower().Contains(_searchtext.ToLower()));
        }

	    private void VenuesView_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
	    {
	        var item = e.AddedItems.FirstOrDefault();
	        _vm.SelectedVenue = item as Venue;
	    }
	}
}