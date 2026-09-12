namespace HashingPlayground.Core;

public interface IPasswordService
{
    PasswordServiceType Type { get; }
    string Hash(string password);
    bool Verify(string passwordHash, string password);
    bool CanHandle(string? passwordHash);
}
