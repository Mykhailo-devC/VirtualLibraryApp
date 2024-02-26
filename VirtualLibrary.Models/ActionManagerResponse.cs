namespace VirtualLibrary.Models;

public class Response<T> : Response where T : class
{
    public T Data { get; set; } = null;
}

public class Response
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; }
}