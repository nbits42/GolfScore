using System;
using System.Collections.ObjectModel;
using TeeScore.DTO;
using TeeScore.Services;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayerPage : ContentPage
	{
	    private readonly GamePlayerViewModel _vm;
		public PlayerPage(string gameId, ObservableCollection<PlayerDto> players)
		{
			InitializeComponent ();
		    _vm = App.IOC.GamePlayer;
		    _vm.Players = players;
		    _vm.GameId = gameId;
		    BindingContext = _vm;
		}

	    protected override async void OnAppearing()
	    {
	        await _vm.LoadAsync().ConfigureAwait(true);
	    }

        private async void OK_Clicked(object sender, EventArgs e)
	    {
	        if (await _vm.ValidateAndSave())
	        {
	            await Navigation.PopModalAsync(true).ConfigureAwait(true);
            }
	    }

	    private async void Cancel_Clicked(object sender, EventArgs e)
	    {
	        await Navigation.PopModalAsync(true).ConfigureAwait(true); 
	    }
	}
}