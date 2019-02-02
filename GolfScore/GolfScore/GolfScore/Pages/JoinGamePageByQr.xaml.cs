using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeeScore.Contracts;
using TeeScore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace TeeScore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinGamePageByQr : ContentPage
    {
        private readonly JoinGameViewModel _vm;
        public JoinGamePageByQr()
        {
            InitializeComponent();
            _vm = App.IOC.JoinGame;
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.ShowGameData = false;
            ScannerView.Options = new MobileBarcodeScanningOptions
            {
                UseNativeScanning = true
            };
            _vm.IsScanning = true;
            _vm.IsAnalyzing = true;
        }
    }
}