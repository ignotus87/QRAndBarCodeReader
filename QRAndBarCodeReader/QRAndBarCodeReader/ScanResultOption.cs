using QRAndBarCodeReader.Resources;
using System;

namespace QRAndBarCodeReader
{
    public enum ScanResultOptions
    {
        OpenLink,
        SearchInGoogle,
        CopyToClipboard,
        Delete
    }

    public class ScanResultOption
    {
        public ScanResultOptions Option { get; private set; }
        public string Text { get; private set; }

        public ScanResultOption(ScanResultOptions option)
        {
            Option = option;
            Text = AppResources.ResourceManager.GetString(Enum.GetName(typeof(ScanResultOptions), Option) + "Text");
        }
    }
}
