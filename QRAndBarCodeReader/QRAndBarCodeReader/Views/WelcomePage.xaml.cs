using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRAndBarCodeReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        Func<Task> _scanStarterCallback;

        public WelcomePage(Func<Task> scanStarterCallback)
        {
            InitializeComponent();

            _scanStarterCallback = scanStarterCallback;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();

            _scanStarterCallback();
        }
    }
}