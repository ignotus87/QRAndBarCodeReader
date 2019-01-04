using QRAndBarCodeReader.Resources;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace QRAndBarCodeReader
{
    public partial class MainPage : ContentPage
    {
        private const string SEPARATOR = "§§§";
        private const string SCAN_HISTORY = "ScanHistory";

        private static ObservableCollection<ScanResult> _scanHistory = new ObservableCollection<ScanResult>();

        public MainPage()
        {
            InitializeComponent();

            InitializeScanHistory();

            this.BindingContext = _scanHistory;

            ScanButton.Clicked += ScanButton_Clicked;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Scan();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public static async void RemoveFromScanHistory(ScanResult scanResult)
        {
            await RemoveScanResult(scanResult);
        }

        private async void InitializeScanHistory()
        {
            await RestoreScanHistory();
        }

        private static async Task AddScanResult(ScanResult scanResult)
        {
            _scanHistory.Insert(0, scanResult);
            await App.Database.SaveScanResultsAsync(scanResult);
        }

        private static async Task RemoveScanResult(ScanResult scanResult)
        {
            _scanHistory.Remove(scanResult);
            await App.Database.DeleteScanResultsAsync(scanResult);
        }

        private async Task RestoreScanHistory()
        {
            var savedResults = await App.Database.GetScanResultsAsync();

            foreach (var result in savedResults)
            {
                _scanHistory.Add(result);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            await OpenResultPage(((ScanResult)e.Item));

            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        public async Task OpenResultPage(ScanResult scanResult)
        {
            var resultPage = new ResultPage(scanResult);
            await Navigation.PushAsync(resultPage);
        }

        private async void ScanButton_Clicked(object sender, EventArgs e)
        {
            await Scan();
        }

        private async Task Scan()
        {
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                DelayBetweenContinuousScans = 100,
                DisableAutofocus = false,
                TryHarder = true
            };

            var scanPage = new ZXingScannerPage(options)
            {
                Title = AppResources.PositionCodeToLine,
                DefaultOverlayShowFlashButton = true
            };

            scanPage.OnScanResult += async (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                var scanResult = new ScanResult(result.Text);

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await AddScanResult(scanResult);
                    await OpenResultPage(scanResult);
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}
