namespace HashingPlayground.Core;

public sealed class PasswordHashGenerator
{
    private readonly IPasswordServiceResolver _resolver;

    public PasswordHashGenerator(IPasswordServiceResolver resolver)
    {
        _resolver = resolver;
    }

    public string Hash(string password, PasswordServiceType hasherType)
        => _resolver.Resolve(hasherType).Hash(password);
}
