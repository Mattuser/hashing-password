namespace HashingPlayground.Core;

public interface IPasswordServiceResolver
{
    IPasswordService Resolve(string passwordHash);
    IPasswordService Resolve(PasswordServiceType serviceType);
}
