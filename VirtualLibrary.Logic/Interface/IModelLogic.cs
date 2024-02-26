namespace VirtualLibrary.Logic.Interface
{
    public interface IModelLogic<T, K> where T : class
    {
        public Task<Response> GetDataAsync();
        public Task<Response> GetSortedDataAsync(string modelField);
        public Task<Response> GetDatabyId(int id);
        public Task<Response> AddDataAsync(K entityDTO);
        public Task<Response> UpdateDataAsync(int id, K entityDTO);
        public Task<Response> DeleteDataAsync(int id);
    }
}
