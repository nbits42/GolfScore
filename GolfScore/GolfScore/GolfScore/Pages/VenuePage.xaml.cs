using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeeScore.DTO;
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

        public VenuePage(string venueId, ObservableCollection<VenueDto> venues)
        {
            _venueId = venueId;
            InitializeComponent();
            _vm = App.IOC.Venue;
            _vm.Venues = venues;
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.LoadAsync(_venueId);
        }

        private async void OK_Clicked(object sender, EventArgs e)
        {
            if (await _vm.ValidateAndSave())
            {
                await Navigation.PopModalAsync(true);
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }
    }
}