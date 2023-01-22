using VirtualLibrary.Utilites.Implementations;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;

namespace VirtualLibrary.Utilites.Interfaces
{
    public interface IDataStore<T, K> where T : class
    {
        public Task<ActionManagerResponse<IEnumerable<T>>> GetDataAsync();
        public Task<ActionManagerResponse<IEnumerable<T>>> GetSortedDataAsync(ModelFields modelField);
        public Task<ActionManagerResponse<T>> AddDataAsync(K entityDTO);
        public Task<ActionManagerResponse<T>> UpdateDataAsync(int id, K entityDTO);
        public Task<ActionManagerResponse<T>> DeleteDataAsync(int id);
    }
}
