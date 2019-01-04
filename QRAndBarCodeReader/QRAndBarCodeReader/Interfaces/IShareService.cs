using System;
using System.Threading.Tasks;

namespace QRAndBarCodeReader.Interfaces
{
    public interface IShareService
    {
        Task ShareLink(string title, string message, Uri uri);
        Task ShareText(string title, string message, string text);
    }
}
