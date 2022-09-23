using Microsoft.EntityFrameworkCore;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.Models.User;

namespace RozetkaFinder.Repository
{
    public class GoodRepository : IRepository<GoodItem>
    {
        private readonly ApplicationContext _context;
        public GoodRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GoodItem>> GetAllAsync() => 
            await _context.Goods.ToListAsync();
        public async Task<bool> CreateAsync(GoodItem good)
        {
            await _context.AddAsync(good);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<GoodItem> ReadAsync(string id)
        {
            return await _context.Goods.Where(u => Convert.ToString(u.IdGood) == id).FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateAsync(GoodItem good)
        {
            GoodItem goodItem = await _context.Goods.FirstOrDefaultAsync(u => u.IdGood == good.IdGood);
            if (goodItem == null)
                return false;
            else
                _context.Entry(goodItem).CurrentValues.SetValues(good);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(GoodItem good)
        {
            _context.Goods.Remove(good);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
