using App.SQLite.Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace App.SQLite.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _dbPath;

        public DatabaseHelper()
        {
            _dbPath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "DB.sqlite"));
        }

        public void CriarDatabase()
        {
            if (!VerificarBaseExiste().Result)
            {
                using (var db = OpenConnection())
                {
                    db.CreateTable<Pessoa>();
                }
            }
        }

        public SQLiteConnection OpenConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), _dbPath);
        }

        private async Task<bool> VerificarBaseExiste()
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(_dbPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
