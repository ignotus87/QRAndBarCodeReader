using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QRAndBarCodeReader
{
    public class ScanHistoryDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ScanHistoryDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ScanResult>().Wait();
        }

        public Task<List<ScanResult>> GetItemsAsync()
        {
            return _database.Table<ScanResult>().ToListAsync();
        }

        public Task<int> SaveItemAsync(ScanResult item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(ScanResult item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
