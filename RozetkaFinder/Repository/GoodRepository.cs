using Microsoft.EntityFrameworkCore;
using RozetkaFinder.Models;

namespace RozetkaFinder.Repository
{
    public class GoodRepository : IRepository<Good>
    {
        private readonly ApplicationContext _context;
        public GoodRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Good>> GetAllAsync() => await _context.Goods.ToListAsync();
        public async void CreateAsync(Good good)
        {
            throw new NotImplementedException();
        }
        public async Task<Good> ReadAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateAsync(Good good)
        {
            throw new NotImplementedException();
        }
        public async void DeleteAsync(Good good)
        {
            throw new NotImplementedException();
        }
    }
}
