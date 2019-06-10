using SQLite;
using SystemZarzadzaniaAkademikiem.Models;

namespace SystemZarzadzaniaAkademikiem.Data
{
    public class AppDatabase : IAppDatabase
    {
        public SQLiteAsyncConnection Database { get; }
        public SQLiteConnection DatabaseNotAsync { get; }
        public AppDatabase(string dbfile)
        {
            Database = new SQLiteAsyncConnection(dbfile);
            DatabaseNotAsync = new SQLiteConnection(dbfile);
            Database.CreateTableAsync<SuperUser>().Wait();
            Database.CreateTableAsync<User>().Wait();
            Database.CreateTableAsync<Room>().Wait();
        }
    }
}
