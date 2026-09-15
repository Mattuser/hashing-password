using HashingPlayground.Core;

namespace PasswordHashing.Cli;

public sealed class PasswordHashingOptions
{
    public const string SectionName = "PasswordHashing";

    public string? Password { get; init; }
    public PasswordServiceType? HasherType { get; init; }
}
