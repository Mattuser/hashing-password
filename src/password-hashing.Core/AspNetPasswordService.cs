using Microsoft.AspNetCore.Identity;

namespace HashingPlayground.Core;

public class AspNetPasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();

    public PasswordServiceType Type => PasswordServiceType.AspNet;

    public string Hash(string password)
        => _hasher.HashPassword(null!, password);

    public bool Verify(string passwordHash, string password)
    {
        var result = _hasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return result is
            PasswordVerificationResult.Success or
            PasswordVerificationResult.Failed;
    }

    public bool CanHandle(string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return false;

        return passwordHash.StartsWith("AQAAAA");
    }
}
