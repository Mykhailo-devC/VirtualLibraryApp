global using VirtualLibrary.Models;
using Microsoft.Extensions.Logging;
using VirtualLibrary.Logic.Interface;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public abstract class ModelLogicBase<T, K> : IModelLogic<T, K> where T : class
    {
        protected IRepository<T, K> _repository;
        protected ILogger<ModelLogicBase<T, K>> _logger;
        public ModelLogicBase(IRepository<T, K> repository, ILogger<ModelLogicBase<T, K>> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public abstract Task<Response> GetDataAsync();
        public abstract Task<Response> GetSortedDataAsync(string modelField);
        public abstract Task<Response> GetDatabyId(string id);
        public abstract Task<Response> AddDataAsync(K entityDTO);
        public abstract Task<Response> UpdateDataAsync(string id, K entityDTO);
        public abstract Task<Response> DeleteDataAsync(string id);
    }
}
