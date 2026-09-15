using HashingPlayground.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PasswordHashing.Cli;

public static class ApplicationRunner
{
    public static int Run(string[] args)
    {
        using var host = CreateHost();
        using var scope = host.Services.CreateScope();

        return scope.ServiceProvider
            .GetRequiredService<CommandConfiguration>()
            .Run(args);
    }

    private static IHost CreateHost()
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = [],
            ContentRootPath = AppContext.BaseDirectory
        });

        builder.Services.Configure<PasswordHashingOptions>(
            builder.Configuration.GetSection(PasswordHashingOptions.SectionName));

        builder.Services.AddScoped<IPasswordService, BCryptPasswordService>();
        builder.Services.AddScoped<IPasswordService, AspNetPasswordService>();
        builder.Services.AddScoped<IPasswordService, Argon2PasswordService>();
        builder.Services.AddScoped<IPasswordServiceResolver, PasswordServiceResolver>();
        builder.Services.AddScoped<PasswordHashGenerator>();
        builder.Services.AddScoped<CommandConfiguration>();

        return builder.Build();
    }
}
