using System;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
	    private SettingsViewModel _vm;
		public SettingsPage ()
		{
			InitializeComponent ();
		    _vm = App.IOC.Settings;
		    BindingContext = _vm;
		}

	    protected override async void OnAppearing()
	    {
	        await _vm.LoadAsync();
	    }

        private async void OK_Clicked(object sender, EventArgs e)
	    {
	        await _vm.SaveAsync();
	        await Navigation.PopModalAsync(true);
	    }

	    private async void Cancel_Clicked(object sender, EventArgs e)
	    {
	        await Navigation.PopModalAsync(true);
	    }
	}
}