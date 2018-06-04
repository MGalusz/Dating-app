using System.Collections.Generic;
using Demo.Models;
using Newtonsoft.Json;

namespace Demo.Data {
    public class Seed {
        private readonly DataContext _context;
        public Seed (DataContext context) {
            _context = context;

        }
        public void SeedUsers(){
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            //seed users
            var userDate = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userDate);
            foreach(var user in users)
            {
                byte[] passhordHash, passwordSalt;
                CreatePasswordHash("password",out passhordHash , out passwordSalt );
                user.PasswordHash = passhordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();
                _context.Users.Add(user);

            }
            _context.SaveChanges();
        }
                private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using( var hmac = new System.Security.Cryptography.HMACSHA512())
           {
               passwordSalt = hmac.Key;
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           }
        }
    }
}