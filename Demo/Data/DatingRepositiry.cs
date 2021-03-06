using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data {
    public class DatingRepositiry : IDatingRepositiry {
        private readonly DataContext _context;
        public DatingRepositiry (DataContext context) {
            _context = context;

        }
        public void Add<T> (T entity) where T : class {
            _context.Add(entity);
        }

        public void Delete<T> (T entity) where T : class {
            _context.Remove(entity);
        }

        public Task<Photo> GetMainPhotoForUser(int userId)
        {
          return _context.Photos.Where(u => u.UserId == userId ).FirstOrDefaultAsync(p => p.IsMain);
        }

        public Task<Photo> GetPhoto(int id)
        {
           var photo = _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
           return photo;
        }

        public async Task<User> GetUserAsync (int id) {
             var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync () {
           
              var users = await _context.Users.Include(p => p.Photos).ToListAsync();
             return users;
        }

        public async Task<bool> SaveAll () {
           return await _context.SaveChangesAsync() > 0 ;
        }
    }
}