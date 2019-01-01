using System;

namespace QRAndBarCodeReader
{
    public enum ScanResultType
    {
        Text,
        Link,
        Product
    }

    public class ScanResult
    {
        public string Text { get; private set; }
        public ScanResultType Type { get; private set; }
        public string TypeText => Enum.GetName(typeof(ScanResultType), Type) ?? "N/A";

        public ScanResult(string text)
        {
            Text = text;

            DetermineType();
        }

        private void DetermineType()
        {
            if (Text.StartsWith("http://") || Text.StartsWith("https://"))
            {
                Type = ScanResultType.Link;   
            }
            else if (Text.Length > 0 && char.IsNumber(Text[0]))
            {
                Type = ScanResultType.Product;
            }
            else
            {
                Type = ScanResultType.Text;
            }
            
        }
    }
}
