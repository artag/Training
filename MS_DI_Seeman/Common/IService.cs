namespace Common;

public interface IService<in TRequest, TResponse>
{
    Task<Result<TResponse>> Execute(TRequest request);
}
