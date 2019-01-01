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

        private ObservableCollection<string> _scanHistory = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();
            InitializeScanHistory();

            this.BindingContext = _scanHistory;

            CameraButton.Clicked += CameraButton_Clicked;

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
            Application.Current.Properties[SCAN_HISTORY] = string.Join(SEPARATOR, _scanHistory.ToList());
            await App.Current.SavePropertiesAsync();
        }

        private async void CreateScanHistory()
        {
            Application.Current.Properties.Add(SCAN_HISTORY, "");
            await App.Current.SavePropertiesAsync();
        }

        private void RestoreScanHistory()
        {
            _scanHistory = new ObservableCollection<string>((Application.Current.Properties[SCAN_HISTORY] as string ?? "").Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries));
        }

        private void OnChange(object sender, EventArgs e)
        {
            Application.Current.Properties[SCAN_HISTORY] = _scanHistory;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
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
                    _scanHistory.Insert(0, result.Text);
                    SaveScanHistory();
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}
