using System.CommandLine;
using HashingPlayground.Core;
using Microsoft.Extensions.Options;

namespace PasswordHashing.Cli;

public sealed class CommandConfiguration
{
    private readonly PasswordHashGenerator _passwordHashGenerator;
    private readonly IPasswordServiceResolver _passwordServiceResolver;
    private readonly PasswordHashingOptions _options;

    public CommandConfiguration(
        PasswordHashGenerator passwordHashGenerator,
        IPasswordServiceResolver passwordServiceResolver,
        IOptions<PasswordHashingOptions> options)
    {
        _passwordHashGenerator = passwordHashGenerator;
        _passwordServiceResolver = passwordServiceResolver;
        _options = options.Value;
    }

    public int Run(string[] args)
        => CreateRootCommand()
            .Parse(args)
            .Invoke();

    private RootCommand CreateRootCommand()
    {
        var rootCommand = new RootCommand("Generate and identify password hashes.");
        rootCommand.Subcommands.Add(CreateHashCommand());
        rootCommand.Subcommands.Add(CreateIdentifyCommand());

        return rootCommand;
    }

    private Command CreateHashCommand()
    {
        var passwordOption = new Option<string?>("--password")
        {
            Description = "Password to hash. Overrides PasswordHashing:Password from appsettings.json."
        };

        var hasherTypeOption = new Option<PasswordServiceType?>("--hasher-type")
        {
            Description = "Password hasher implementation. Overrides PasswordHashing:HasherType from appsettings.json."
        };

        var command = new Command("hash", "Hash a password using the selected password hasher.")
        {
            passwordOption,
            hasherTypeOption
        };

        command.SetAction(parseResult => ExecuteHash(
            parseResult.GetValue(passwordOption),
            parseResult.GetValue(hasherTypeOption)));

        return command;
    }

    private Command CreateIdentifyCommand()
    {
        var hashOption = new Option<string>("--hash")
        {
            Description = "Password hash to identify.",
            Required = true
        };

        var command = new Command("identify", "Identify the password hasher from a hash.")
        {
            hashOption
        };

        command.SetAction(parseResult => ExecuteIdentify(
            parseResult.GetValue(hashOption)));

        return command;
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

        var passwordHash = _passwordHashGenerator.Hash(password, hasherType.Value);

        Console.WriteLine($"Hasher type: {hasherType.Value}");
        Console.WriteLine($"Password hash: {passwordHash}");
        return 0;
    }

    private int ExecuteIdentify(string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            Console.Error.WriteLine("A password hash is required. Use --hash.");
            return 1;
        }

        try
        {
            var passwordService = _passwordServiceResolver.Resolve(passwordHash);
            Console.WriteLine($"Hasher type: {passwordService.Type}");
            return 0;
        }
        catch (InvalidOperationException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 1;
        }
    }
}
