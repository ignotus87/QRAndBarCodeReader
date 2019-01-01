﻿
using QRAndBarCodeReader.Interfaces;
using QRAndBarCodeReader.Resources;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRAndBarCodeReader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultPage : ContentPage
	{
        public ScanResult Result { get; private set; }

        public ResultPage(ScanResult scanResult)
        {
            InitializeComponent();

            Result = scanResult;
            BindingContext = Result;
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            var selectedOption = (ScanResultOption)e.Item;
            
            switch (selectedOption.Option)
            {
                case ScanResultOptions.CopyToClipboard:
                    var clipboardService = DependencyService.Get<IClipboardService>();
                    clipboardService?.CopyToClipboard(Result.Text);
                    await DisplayAlert(AppResources.MainPageTitle, AppResources.TextCopiedText, AppResources.OKText);
                    break;

                case ScanResultOptions.OpenLink:
                    var link = (!Result.Text.StartsWith("http") ? "http://" : "") + Result.Text;
                    Device.OpenUri(new System.Uri(link));
                    break;

                case ScanResultOptions.SearchInGoogle:
                    var searchLink = "https://www.google.com/search?q=" + HttpUtility.UrlEncode(Result.Text);
                    Device.OpenUri(new System.Uri(searchLink));
                    break;

                case ScanResultOptions.Delete:
                    var isSure = await DisplayAlert(AppResources.DeleteItemText, AppResources.DeleteAreYouSureText, AppResources.DeleteYes, AppResources.DeleteNo);
                    if (isSure)
                    {
                        MainPage.RemoveFromScanHistory(Result);
                        await Navigation.PopAsync(true);
                    }
                    break;
            }

            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }
}