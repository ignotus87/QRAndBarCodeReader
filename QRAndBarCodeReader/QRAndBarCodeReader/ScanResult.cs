using QRAndBarCodeReader.Resources;
using System;
using System.Collections.Generic;

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
        private static string _scanResultText;
        public static string ScanResultText
        {
            get
            {
                if (_scanResultText == null)
                {
                    _scanResultText = AppResources.ScanResultText;
                }
                return _scanResultText;
            }
        }

        public string Text { get; private set; }
        public ScanResultType Type { get; private set; }
        public string TypeText => Enum.GetName(typeof(ScanResultType), Type) ?? "N/A";

        private List<ScanResultOption> _options;
        public List<ScanResultOption> Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new List<ScanResultOption>();

                    if (Type == ScanResultType.Link)
                    {
                        _options.Add(ScanResultOptionFactory.Instance.Dictionary[ScanResultOptions.OpenLink]);
                    }

                    _options.AddRange(new[] 
                    {
                        ScanResultOptionFactory.Instance.Dictionary[ScanResultOptions.SearchInGoogle],
                        ScanResultOptionFactory.Instance.Dictionary[ScanResultOptions.CopyToClipboard],
                        ScanResultOptionFactory.Instance.Dictionary[ScanResultOptions.Delete]
                    });
                }
                return _options;
            }
        }

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
