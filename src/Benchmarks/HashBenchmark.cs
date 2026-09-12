using BenchmarkDotNet.Attributes;
using HashingPlayground.Core;

namespace Benchmarks;

public class HashBenchmark
{
    private const string Password = "password@password123";

    private IPasswordServiceResolver _resolver = null!;
    private IPasswordService _bcryptService = null!;
    private IPasswordService _aspNetService = null!;

    [GlobalSetup]
    public void Setup()
    {
        var services = new IPasswordService[]
        {
            new BCryptPasswordService(),
            new AspNetPasswordService()
        };

        _resolver = new PasswordServiceResolver(services);
        _bcryptService = _resolver.Resolve(PasswordServiceType.BCrypt);
        _aspNetService = _resolver.Resolve(PasswordServiceType.AspNet);
    }

    [Benchmark]
    public string HashWithBCrypt()
        => _bcryptService.Hash(Password);

    [Benchmark]
    public string HashWithAspNet()
        => _aspNetService.Hash(Password);
}
