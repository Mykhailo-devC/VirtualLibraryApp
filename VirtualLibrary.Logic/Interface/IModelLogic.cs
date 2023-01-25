global using VirtualLibrary.Models;


namespace VirtualLibrary.Logic.Interface
{
    public interface IModelLogic<T, K> where T : class
    {
        public Task<ActionManagerResponse> GetDataAsync();
        public Task<ActionManagerResponse> GetSortedDataAsync(string modelField);
        public Task<ActionManagerResponse> GetDatabyId(int id);
        public Task<ActionManagerResponse> AddDataAsync(K entityDTO);
        public Task<ActionManagerResponse> UpdateDataAsync(int id, K entityDTO);
        public Task<ActionManagerResponse> DeleteDataAsync(int id);
    }
}
