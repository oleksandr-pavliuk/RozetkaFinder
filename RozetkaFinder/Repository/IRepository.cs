namespace RozetkaFinder.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> CreateAsync(T item);
        Task<T> ReadAsync(string identify);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T item);


    }
}
