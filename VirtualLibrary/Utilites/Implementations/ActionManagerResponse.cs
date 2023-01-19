namespace VirtualLibrary.Utilites.Implementations
{
    public class ActionManagerResponse<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public T ActionResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
