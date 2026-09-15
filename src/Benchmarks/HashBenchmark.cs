using BenchmarkDotNet.Attributes;
using HashingPlayground.Core;

namespace Benchmarks;

public class HashBenchmark
{
    private const string Password = "password@password123";

    private IPasswordServiceResolver _resolver = null!;
    private IPasswordService _bcryptService = null!;
    private IPasswordService _aspNetService = null!;
    private IPasswordService _argon2Service = null!;

    [GlobalSetup]
    public void Setup()
    {
        var services = new IPasswordService[]
        {
            new BCryptPasswordService(),
            new AspNetPasswordService(),
            new Argon2PasswordService()
        };

        _resolver = new PasswordServiceResolver(services);
        _bcryptService = _resolver.Resolve(PasswordServiceType.BCrypt);
        _aspNetService = _resolver.Resolve(PasswordServiceType.AspNet);
        _argon2Service = _resolver.Resolve(PasswordServiceType.Argon2);
    }

    [Benchmark]
    public string HashWithBCrypt()
        => _bcryptService.Hash(Password);

    [Benchmark]
    public string HashWithAspNet()
        => _aspNetService.Hash(Password);

    [Benchmark]
    public string HashWithArgon2()
        => _argon2Service.Hash(Password);
}
