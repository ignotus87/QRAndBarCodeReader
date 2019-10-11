using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using QRAndBarCodeReader.Interfaces;
using QRAndBarCodeReader.Resources;
using QRAndBarCodeReader.Views;
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
        private const string HAS_EVER_STARTED_KEY = "HasEverStarted";
        private static ObservableCollection<ScanResult> _scanHistory = new ObservableCollection<ScanResult>();

        private static ScanResult _emptyScanHistoryPlaceholder = new ScanResult(AppResources.ScanHistoryPlaceholder, ScanResultType.Placeholder);

        public MainPage()
        {
            InitializeComponent();

            InitializeScanHistory();

            EnsureCameraPermission();

            this.BindingContext = _scanHistory;

            ScanButton.Clicked += ScanButton_Clicked;

            if (IsFirstStart())
            {
                //ShowFirstStartInfo();
                SetFirstStartFlag();
            }
            else
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Scan();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private async void EnsureCameraPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Camera))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Camera });
                    status = results[Plugin.Permissions.Abstractions.Permission.Camera];
                }

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Camera Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Camera Denied", "Can not continue, try again.", "OK");
            }
        }

        private void SetFirstStartFlag()
        {
            App.Current.Properties.Add(HAS_EVER_STARTED_KEY, true);
        }

        //private void ShowFirstStartInfo()
        //{
        //    var welcomePage = new WelcomePage(Scan);
        //    Navigation.PushAsync(welcomePage);
        //}

        private bool IsFirstStart()
        {
            return !App.Current.Properties.ContainsKey(HAS_EVER_STARTED_KEY);
        }

        public static async void RemoveFromScanHistory(ScanResult scanResult)
        {
            await RemoveScanResult(scanResult);
        }

        private async void InitializeScanHistory()
        {
            await RestoreScanHistory();

            if (_scanHistory.Count == 0)
            {
                _scanHistory.Add(_emptyScanHistoryPlaceholder);
            }
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

            var tappedItem = ((ScanResult)e.Item);

            if (tappedItem != _emptyScanHistoryPlaceholder)
            {
                await OpenResultPage(tappedItem);
            }

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

        public async Task Scan()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner()
            {
                UseCustomOverlay = false,
                BottomText = AppResources.PositionCodeToLine
            };

            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                DelayBetweenContinuousScans = 100,
                DisableAutofocus = false,
                TryHarder = false,
                TryInverted = true
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
