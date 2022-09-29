using System.Linq.Expressions;

namespace RozetkaFinder.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> CreateAsync(T item);
        T ReadAsync(Func<T, bool> predicate);
        Task<bool> UpdateAsync(T item, Func<T, bool> predicate);
        Task<bool> DeleteAsync(T item);


    }
}
