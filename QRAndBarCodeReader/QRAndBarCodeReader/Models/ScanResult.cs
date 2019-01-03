using QRAndBarCodeReader.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.Web;

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

        [PrimaryKey, AutoIncrement]
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

        public string TypeText => AppResources.ResourceManager.GetString("ScanResultType" + Enum.GetName(typeof(ScanResultType), Type)) ?? "N/A";

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
                        ScanResultOptionFactory.Instance.Dictionary[ScanResultOptions.Share],
                        ScanResultOptionFactory.Instance.Dictionary[ScanResultOptions.Delete]
                    });
                }
                return _options;
            }
        }

        public ScanResult()
        {
        }

        public ScanResult(string text)
        {
            Text = text;
        }

        private void DetermineType()
        {
            if (Text.StartsWith("http://") || Text.StartsWith("https://") || Text.ToLower().StartsWith("www."))
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

        public Uri GetUri()
        {
            switch (Type)
            {
                case ScanResultType.Link:
                    return new Uri((!Text.StartsWith("http") ? "http://" : "") + Text);

                case ScanResultType.Product:
                    return new Uri("https://www.google.com/search?q=" + HttpUtility.UrlEncode(Text));

                default:
                    return null;
            }
        }
    }
}
