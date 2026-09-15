using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HashingPlayground.Core;

public class AspNetPasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher;

    public AspNetPasswordService()
    {
        var options = Options.Create(new PasswordHasherOptions
        {
            CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
            IterationCount = 100_000
        });

        _hasher = new(options);
    }

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
