namespace VirtualLibrary.Utilites.Implementations
{
    public class ActionManagerResponse<T> where T : class
    {
        public T ActionResult { get; set; } = null;
        public string Message { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }


    }
}