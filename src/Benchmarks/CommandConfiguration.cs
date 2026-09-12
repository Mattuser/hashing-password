using System.CommandLine;
using HashingPlayground.Core;
using Microsoft.Extensions.Options;

namespace Benchmarks;

public sealed class CommandConfiguration
{
    private readonly PasswordHashGenerator _passwordHashGenerator;
    private readonly PasswordHashingOptions _options;

    public CommandConfiguration(
        PasswordHashGenerator passwordHashGenerator,
        IOptions<PasswordHashingOptions> options)
    {
        _passwordHashGenerator = passwordHashGenerator;
        _options = options.Value;
    }

    public int Run(string[] args)
        => CreateRootCommand()
            .Parse(args)
            .Invoke();

    private RootCommand CreateRootCommand()
    {
        var passwordOption = new Option<string?>("--password")
        {
            Description = "Password to hash. Overrides PasswordHashing:Password from appsettings.json."
        };

        var hasherTypeOption = new Option<PasswordServiceType?>("--hasher-type")
        {
            Description = "Password hasher implementation. Overrides PasswordHashing:HasherType from appsettings.json."
        };

        var hashCommand = new Command("hash", "Hash a password using the selected password hasher.")
        {
            passwordOption,
            hasherTypeOption
        };

        hashCommand.SetAction(parseResult => ExecuteHash(
            parseResult.GetValue(passwordOption),
            parseResult.GetValue(hasherTypeOption)));

        return new RootCommand("Password hashing playground.")
        {
            hashCommand
        };
    }

    private int ExecuteHash(string? password, PasswordServiceType? hasherType)
    {
        password ??= _options.Password;
        hasherType ??= _options.HasherType;

        if (string.IsNullOrEmpty(password))
        {
            Console.Error.WriteLine(
                "A password is required. Use --password or set PasswordHashing:Password in appsettings.json.");
            return 1;
        }

        if (hasherType is null)
        {
            Console.Error.WriteLine(
                "A valid hasher type is required. Use --hasher-type BCrypt|AspNet or set PasswordHashing:HasherType in appsettings.json.");
            return 1;
        }

        Console.WriteLine(_passwordHashGenerator.Hash(password, hasherType.Value));
        return 0;
    }
}
