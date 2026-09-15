using System.Text.RegularExpressions;
using Isopoh.Cryptography.Argon2;

namespace HashingPlayground.Core;

public class Argon2PasswordService : IPasswordService
{
    private static readonly Regex Argon2Regex = new(
        @"^\$argon2(id|i|d)\$v=\d+\$m=\d+,t=\d+,p=\d+\$[A-Za-z0-9+/=~_-]+\$[A-Za-z0-9+/=~_-]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public PasswordServiceType Type => PasswordServiceType.Argon2;

    public string Hash(string password)
        => Argon2.Hash(password);

    public bool Verify(string passwordHash, string password)
        => Argon2.Verify(passwordHash, password);

    public bool CanHandle(string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return false;

        return Argon2Regex.IsMatch(passwordHash);
    }
}
