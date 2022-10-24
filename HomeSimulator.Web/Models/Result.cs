namespace HomeSimulator.Web.Models;

[Serializable]
public class Result
{
    public bool Successful { get; set; }

    public string? Message { get; set; }

    public Result()
    {
        Successful = false;
    }

    public Result Return(string message, bool successful = false)
    {
        this.Successful = successful;
        this.Message = message;
        return this;
    }

}

[Serializable]
public class Result<T> : Result
{

    public T? Data { get; set; }

    public Result<T> Return(string message, T data)
    {
        this.Successful = true;
        this.Message = message;
        this.Data = data;
        return this;
    }

    public Result<T> Return(string message)
    {
        this.Successful = false;
        this.Message = message;
        return this;
    }

    public Result<N> ChangeData<N>(N data)
    {
        var result = new Result<N>();
        result.Message = Message;
        result.Successful = Successful;
        result.Data = data;
        return result;
    }
}
