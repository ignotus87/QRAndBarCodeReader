using QRAndBarCodeReader.Resources;
using System;

namespace QRAndBarCodeReader
{
    public enum ScanResultOptions
    {
        OpenLink,
        SearchInGoogle,
        CopyToClipboard,
        Share,
        Delete
    }

    public class ScanResultOption
    {
        public ScanResultOptions Option { get; private set; }
        public string Text { get; private set; }
        public string Icon { get; private set; }
        public string TextColor { get; private set; }


        public ScanResultOption(ScanResultOptions option)
        {
            var optionName = Enum.GetName(typeof(ScanResultOptions), option);

            Option = option;
            Text = AppResources.ResourceManager.GetString(optionName + "Text");
            Icon = optionName + ".png";

            TextColor = option == ScanResultOptions.Delete ? "#FF0000" : "#000000";
        }
    }
}
