using Android.Content;
using QRAndBarCodeReader.Droid.DependencyServices;
using QRAndBarCodeReader.Interfaces;
using QRAndBarCodeReader.Resources;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareService))]
namespace QRAndBarCodeReader.Droid.DependencyServices
{
    public class ShareService : IShareService
    {
        private const string CONTENT_TYPE_TEXT = "text/plain";
        private static string PLAY_STORE_LINK = "<a href=\"#\">" + AppResources.AppName + "</a>";

        private readonly Context _context;
        public ShareService()
        {
            _context = Android.App.Application.Context;
        }

        public Task ShareText(string title, string message, string text)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType(CONTENT_TYPE_TEXT);
            intent.PutExtra(Intent.ExtraText, text + Environment.NewLine + " - " + string.Format(AppResources.ScannedWithAppNameText, PLAY_STORE_LINK));
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }

        public Task ShareLink(string title, string message, Uri uri)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType(CONTENT_TYPE_TEXT);
            intent.PutExtra(Intent.ExtraText, uri.AbsoluteUri + Environment.NewLine + " - " + string.Format(AppResources.ScannedWithAppNameText, PLAY_STORE_LINK));
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }
    }
}