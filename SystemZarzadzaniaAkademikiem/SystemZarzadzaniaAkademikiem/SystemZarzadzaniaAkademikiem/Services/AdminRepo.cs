using SQLite;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SystemZarzadzaniaAkademikiem.Data;
using SystemZarzadzaniaAkademikiem.Models;

namespace SystemZarzadzaniaAkademikiem.Services
{
    public class AdminRepo : IAdminRepo
    {
        readonly SQLiteAsyncConnection _database;
        public AdminRepo(AppDatabase database)
        {
            _database = database.Database;
            if (_database.Table<SuperUser>().FirstOrDefaultAsync().Result==null)
            {
                string password = RandomString(50);
                _database.InsertAsync(new SuperUser { Login="Admin",Password= StringCipher.Encrypt("S3cr3tP@ss",password), Salt=password});
            }
        }
        public Task<SuperUser> GetAdmin()
        {
            return _database.Table<SuperUser>().FirstOrDefaultAsync();
        }
        public Task<int> SaveAdminAsync(SuperUser admin)
        {
            string password = RandomString(50);
            admin.Password = StringCipher.Encrypt(admin.Password, password);
            admin.Salt = password;
            if (admin.Id != 0)
            {
                return _database.UpdateAsync(admin);
            }
            else
            {
                return _database.InsertAsync(admin);
            }
        }
        public static string RandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }
    }
}
