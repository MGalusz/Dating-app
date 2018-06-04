using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Models;

namespace Demo.Data
{
    public interface IDatingRepositiry
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        
}
}