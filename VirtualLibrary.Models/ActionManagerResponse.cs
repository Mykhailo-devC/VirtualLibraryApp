namespace VirtualLibrary.Models;

public class ActionManagerResponse<T> : ActionManagerResponse where T : class
{
    public T ActionResult { get; set; } = null;
}

public class ActionManagerResponse
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; }
}