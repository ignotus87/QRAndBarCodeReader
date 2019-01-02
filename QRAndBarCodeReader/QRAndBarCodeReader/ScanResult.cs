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
        public int ID { get; set; }
        public string Text { get; set; }

        private ScanResultType? _type;
        public ScanResultType Type
        {
            get
            {
                if (_type == null)
                {
                    DetermineType();
                }
                return (ScanResultType)_type;
            }
        }

        public string TypeText => Enum.GetName(typeof(ScanResultType), Type) ?? "N/A";

        public ScanResult()
        {
        }

        public ScanResult(string text)
        {
            Text = text;
        }

        private void DetermineType()
        {
            if (Text.StartsWith("http://") || Text.StartsWith("https://"))
            {
                _type = ScanResultType.Link;   
            }
            else if (Text.Length > 0 && char.IsNumber(Text[0]))
            {
                _type = ScanResultType.Product;
            }
            else
            {
                _type = ScanResultType.Text;
            }
            
        }
    }
}
