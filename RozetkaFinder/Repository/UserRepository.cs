using RozetkaFinder.Models.User;
using Microsoft.EntityFrameworkCore;

namespace RozetkaFinder.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        void Create(User user);
        Task<User> Read(string email);
        Task<bool> Update(User user);
        void Delete(User user);


    }
    public class UserRepository : IUserRepository                                      // CRUD operations
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll() => await _context.Users.ToListAsync();

        //<------------ NEW USER ----------->
        public async void Create(User user) 
        {
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }


        //<------------ SEARCH USER ----------->
        public async Task<User> Read(string email)    
        {
            User user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            
            return user;
        }

        //<--------------- UPDATE USER --------------->
        public async Task<bool> Update(User user)   //modify user
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
        public async void Delete(User user)   //delete user
        {
            _context.Users.Remove(user);
           await _context.SaveChangesAsync();

        }

    }
}


