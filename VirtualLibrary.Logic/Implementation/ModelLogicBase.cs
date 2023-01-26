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
        public abstract Task<ActionManagerResponse> GetDataAsync();
        public abstract Task<ActionManagerResponse> GetSortedDataAsync(string modelField);
        public abstract Task<ActionManagerResponse> GetDatabyId(int id);
        public abstract Task<ActionManagerResponse> AddDataAsync(K entityDTO);
        public abstract Task<ActionManagerResponse> UpdateDataAsync(int id, K entityDTO);
        public abstract Task<ActionManagerResponse> DeleteDataAsync(int id);
    }
}
