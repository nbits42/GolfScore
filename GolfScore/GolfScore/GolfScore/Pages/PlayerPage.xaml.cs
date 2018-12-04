using System;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayerPage : ContentPage
	{
	    private NewGameViewModel _vm;
		public PlayerPage()
		{
			InitializeComponent ();
		    _vm = App.IOC.NewGame;
		    BindingContext = _vm;
		}

	    protected override async void OnAppearing()
	    {
	        await _vm.LoadAsync();
	    }

        private async void OK_Clicked(object sender, EventArgs e)
	    {
	        await _vm.SaveSelectedPlayerAsync();
	        await Navigation.PopModalAsync(true);
	    }

	    private async void Cancel_Clicked(object sender, EventArgs e)
	    {
	        await Navigation.PopModalAsync(true);
	    }
	}
}