using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GolfScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GolfScore.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
	    private MainViewModel _vm;
		public SettingsPage ()
		{
			InitializeComponent ();
		    _vm = App.IOC.Main;
		    BindingContext = _vm;
		}

	    private async void OK_Clicked(object sender, EventArgs e)
	    {
	        await _vm.SavePlayer();
	        await Navigation.PopModalAsync(true);
	    }

	    private async void Cancel_Clicked(object sender, EventArgs e)
	    {
	        if (string.IsNullOrEmpty(_vm.MyPlayer.Id))
	        {
	            _vm.ClearMyPlayer();
	        }
	        await Navigation.PopModalAsync(true);
	    }
	}
}