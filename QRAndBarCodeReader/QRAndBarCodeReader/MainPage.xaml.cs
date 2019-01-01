using QRAndBarCodeReader.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace QRAndBarCodeReader
{
    public partial class MainPage : ContentPage
    {
        private const string SEPARATOR = "§§§";
        private const string SCAN_HISTORY = "ScanHistory";

        private ObservableCollection<ScanResult> _scanHistory = new ObservableCollection<ScanResult>();

        public MainPage()
        {
            InitializeComponent();
            InitializeScanHistory();

            this.BindingContext = _scanHistory;

            ScanButton.Clicked += ScanButton_Clicked;

            Scan();
        }

        private void InitializeScanHistory()
        {
            if (Application.Current.Properties.ContainsKey(SCAN_HISTORY))
            {
                RestoreScanHistory();
            }
            else
            {
                CreateScanHistory();
            }
        }

        private async void SaveScanHistory()
        {
            Application.Current.Properties[SCAN_HISTORY] = string.Join(SEPARATOR, _scanHistory.Select(x => x.Text).ToList());
            await App.Current.SavePropertiesAsync();
        }

        private async void CreateScanHistory()
        {
            Application.Current.Properties.Add(SCAN_HISTORY, "");
            await App.Current.SavePropertiesAsync();
        }

        private void RestoreScanHistory()
        {
            _scanHistory = new ObservableCollection<ScanResult>((Application.Current.Properties[SCAN_HISTORY] as string ?? "").Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries).Select(x => new ScanResult(x)));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            await DisplayAlert(AppResources.ScannedBarcodeText, e.Item.ToString(), "OK");

            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private async void ScanButton_Clicked(object sender, EventArgs e)
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
                Title = AppResources.PositionCodeToLine,
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
                    _scanHistory.Insert(0, new ScanResult(result.Text));
                    SaveScanHistory();
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}
