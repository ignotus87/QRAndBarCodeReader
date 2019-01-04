using QRAndBarCodeReader.Resources;
using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace QRAndBarCodeReader
{
    public partial class App : Application
    {
        static ScanHistoryDatabase database;

        public static ScanHistoryDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ScanHistoryDatabase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScanHistorySQLite.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            // force a specific culture, useful for quick testing
            //AppResources.Culture = new CultureInfo("de-DE");

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
