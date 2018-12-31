using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace QRAndBarCodeReader
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<string> _scanHistory = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = _scanHistory;

            CameraButton.Clicked += CameraButton_Clicked;

            Scan();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            await DisplayAlert("Scanned Barcode", e.Item.ToString(), "OK");

            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            await Scan();
        }

        private async Task Scan()
        {
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                AutoRotate = true,
                DelayBetweenContinuousScans = 200,
                DisableAutofocus = false,
                TryHarder = true
            };
            var scanPage = new ZXingScannerPage(options)
            {
                DefaultOverlayShowFlashButton = true
            };

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");
                    _scanHistory.Add(result.Text);
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}
