namespace HashingPlayground.Core;

public class PasswordServiceResolver : IPasswordServiceResolver
{
    private readonly IEnumerable<IPasswordService> _services;

    public PasswordServiceResolver(
        IEnumerable<IPasswordService> services)
    {
        _services = services;
    }

    public IPasswordService Resolve(string passwordHash)
    {
        var service = _services.FirstOrDefault(x => x.CanHandle(passwordHash));

        return service
               ?? throw new InvalidOperationException("Password hash format is not supported.");
    }

    public IPasswordService Resolve(PasswordServiceType serviceType)
    {
        var service = _services.FirstOrDefault(x => x.Type == serviceType);

        return service
               ?? throw new InvalidOperationException("Password service is not registered.");
    }
}
