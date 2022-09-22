using Microsoft.EntityFrameworkCore;
using RozetkaFinder.Models.GoodObjects;

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
        public async void CreateAsync(GoodItem good)
        {
            throw new NotImplementedException();
        }
        public async Task<GoodItem> ReadAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateAsync(GoodItem good)
        {
            throw new NotImplementedException();
        }
        public async void DeleteAsync(GoodItem good)
        {
            throw new NotImplementedException();
        }
    }
}
