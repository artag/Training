namespace DILifetime.Services;

public class ScopedService : IService
{
    public ScopedService()
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
        $"{nameof(ScopedService)}. Created at: {CreatedDateTime:O}, uid: {Uid}";
}
