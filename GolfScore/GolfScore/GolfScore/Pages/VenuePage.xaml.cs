using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeeScore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenuePage : ContentPage
    {
        private readonly string _venueId;
        private readonly VenueViewModel _vm;

        public VenuePage(string venueId)
        {
            _venueId = venueId;
            InitializeComponent();
            _vm = App.IOC.Venue;
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.LoadAsync(_venueId);
        }

        private async void OK_Clicked(object sender, EventArgs e)
        {
            _vm.Validate();
            if (_vm.IsValid)
            {
                await _vm.SaveAsync();
                await Navigation.PopModalAsync(true);
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}