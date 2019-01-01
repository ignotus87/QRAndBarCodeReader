
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
	}
}