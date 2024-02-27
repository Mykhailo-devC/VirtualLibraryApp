namespace VirtualLibrary.Logic.Interface
{
    public interface IModelLogic<T, K> where T : class
    {
        public Task<Response> GetDataAsync();
        public Task<Response> GetSortedDataAsync(string modelField);
        public Task<Response> GetDatabyId(string id);
        public Task<Response> AddDataAsync(K entityDTO);
        public Task<Response> UpdateDataAsync(string id, K entityDTO);
        public Task<Response> DeleteDataAsync(string id);
    }
}
