using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;
using VirtualLibrary.Utilites.Interfaces;

namespace VirtualLibrary.Utilites.Implementations.DataStore
{
    public abstract class DataStoreBase<T, K> : IDataStore<T, K> where T : class
    {
        protected IRepository<T, K> _repository;
        protected ILogger<DataStoreBase<T, K>> _logger;
        public DataStoreBase(RepositoryFactory factory, ILogger<DataStoreBase<T, K>> logger)
        {
            _repository = factory.GetRepository<T, K>();
            _logger = logger;
        }
        public abstract Task<ActionManagerResponse<IEnumerable<T>>> GetDataAsync();
        public abstract Task<ActionManagerResponse<IEnumerable<T>>> GetSortedDataAsync(ModelFields modelField);
        public abstract Task<ActionManagerResponse<T>> AddDataAsync(K entityDTO);
        public abstract Task<ActionManagerResponse<T>> UpdateDataAsync(int id, K entityDTO);
        public abstract Task<ActionManagerResponse<T>> DeleteDataAsync(int id);
    }
}
