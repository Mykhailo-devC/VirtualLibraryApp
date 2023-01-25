global using VirtualLibrary.Models;

namespace VirtualLibrary.Repository.Interface
{
    public interface IRepository<T, K>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> CreateAsync(K entity);
        public Task<T> UpdateAsync(int id, K entity);
        public Task<T> DeleteAsync(int id);
        public Task SaveAsync();
    }
}
