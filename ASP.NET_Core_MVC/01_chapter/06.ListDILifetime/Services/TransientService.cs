namespace DILifetime.Services;

public class TransientService : IService
{
    public TransientService()
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
        $"{nameof(TransientService)}. Created at: {CreatedDateTime:O}, uid: {Uid}";
}
