using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using RozetkaFinder.Models.Exceptions;
using RozetkaFinder.Models.User;
using System.Linq;

namespace RozetkaFinder.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _dbSet; 
        public Repository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync() => (IEnumerable<T>)await _dbSet.AsNoTracking().ToListAsync() ;
        
       

        public async Task<bool> CreateAsync(T entity)          //create entity
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public T ReadAsync(Func<T, bool> predicate) =>           //read entity
            _dbSet.AsNoTracking().Where<T>(predicate).FirstOrDefault();

        public async Task<bool> UpdateAsync(T entity, Func<T, bool> predicate)   //modify entity
        {
            T found = _dbSet.Where(predicate).FirstOrDefault();
            if (found == null)
                throw new UserNotFoundException();
            _dbSet.Remove(found);
            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(T entity)   //delete entity
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
