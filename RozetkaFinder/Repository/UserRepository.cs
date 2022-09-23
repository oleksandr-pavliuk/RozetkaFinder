using RozetkaFinder.Models.User;
using Microsoft.EntityFrameworkCore;

namespace RozetkaFinder.Repository
{
    public class UserRepository : IRepository<User>                                      // CRUD operations
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        //<------------ NEW USER ----------->
        public async Task<bool> CreateAsync(User user) 
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return true;
        }


        //<------------ SEARCH USER ----------->
        public async Task<User> ReadAsync(string email)    
        {
            User user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }

        //<--------------- UPDATE USER --------------->
        public async Task<bool> UpdateAsync(User user)   //modify user
        {
            User found = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (found == null)
                return false;
            else
                _context.Entry(found).CurrentValues.SetValues(user);
            
            await _context.SaveChangesAsync();
                 return true;
            
        }

        //<------------------ DELETE USER ---------------->
        public async Task<bool> DeleteAsync(User user)   //delete user
        {
            _context.Users.Remove(user);
           await _context.SaveChangesAsync();
            return true;
        }

    }
}


