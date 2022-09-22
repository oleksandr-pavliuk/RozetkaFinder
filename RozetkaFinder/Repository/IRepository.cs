namespace RozetkaFinder.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        void CreateAsync(T item);
        Task<T> ReadAsync(string identify);
        Task<bool> UpdateAsync(T item);
        void DeleteAsync(T item);


    }
}
