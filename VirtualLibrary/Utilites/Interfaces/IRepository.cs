using VirtualLibrary.Utilites.Implementations;

namespace VirtualLibrary.Utilites.Interfaces
{
    public interface IRepository<T, K>
    {
        public Task<ActionManagerResponse<IEnumerable<T>>> GetAllAsync();
        public Task<ActionManagerResponse<T>> GetByIdAsync(int id);
        public Task<ActionManagerResponse<T>> CreateAsync(K entity);
        public Task<ActionManagerResponse<T>> UpdateAsync(int id, K entity);
        public Task<ActionManagerResponse<T>> DeleteAsync(int id);
        public Task SaveAsync();
    }
}
