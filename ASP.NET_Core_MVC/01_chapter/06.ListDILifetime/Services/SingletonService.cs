namespace DILifetime.Services;

public class SingletonService : IService
{
    public SingletonService()
    {
        CreatedDateTime = DateTime.Now;
        Uid = Guid.NewGuid();
    }

    /// <inheritdoc />
    public DateTime CreatedDateTime { get; }

    /// <inheritdoc />
    public Guid Uid { get; }

    /// <inheritdoc />
    public override string ToString() =>
        $"{nameof(SingletonService)}. Created at: {CreatedDateTime:O}, uid: {Uid}";
}
