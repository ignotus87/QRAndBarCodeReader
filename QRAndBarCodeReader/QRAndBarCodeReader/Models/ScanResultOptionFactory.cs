using System.Collections.Generic;

namespace QRAndBarCodeReader
{
    public class ScanResultOptionFactory
    {
        public Dictionary<ScanResultOptions, ScanResultOption> Dictionary { get; private set; }

        private static ScanResultOptionFactory _instance;
        public static ScanResultOptionFactory Instance
        {
            get
            {
                if (_instance == null) { _instance = new ScanResultOptionFactory(); }
                return _instance;
            }
        }

        private ScanResultOptionFactory()
        {
            Dictionary = new Dictionary<ScanResultOptions, ScanResultOption>()
            {
                {ScanResultOptions.OpenLink,  new ScanResultOption(ScanResultOptions.OpenLink) },
                {ScanResultOptions.SearchInGoogle, new ScanResultOption(ScanResultOptions.SearchInGoogle) },
                {ScanResultOptions.CopyToClipboard, new ScanResultOption(ScanResultOptions.CopyToClipboard) },
                {ScanResultOptions.Delete, new ScanResultOption(ScanResultOptions.Delete) }
            };
        }
    }
}
