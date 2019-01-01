using Android.Content;
using QRAndBarCodeReader.Droid.DependencyServices;
using QRAndBarCodeReader.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClipboardService))]
namespace QRAndBarCodeReader.Droid.DependencyServices
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            var clipboardManager = (ClipboardManager)Android.App.Application.Context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("Android Clipboard", text);
            clipboardManager.PrimaryClip = clip;
        }
    }
}