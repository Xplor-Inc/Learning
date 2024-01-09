using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XploringMe.Core.Conductors;
using XploringMe.Core.Conductors.Users;
using XploringMe.Core.Interfaces.Conductor;
using XploringMe.Core.Interfaces.Conductors.Accounts;
using XploringMe.Core.Interfaces.Utility;
using XploringMe.Core.Interfaces.Utility.Security;
using XploringMe.Core.Utilities;
using XploringMe.Core.Utilities.Security;

namespace XploringMe.Core.Extensions.Middleware;
public static class IServiceColletionsStartup
{
    public static void AddUtilityResolver(this IServiceCollection services)
    {
        //services.AddScoped<IAccountConductor,       AccountConductor>();
        services.AddScoped<IEncryption,             Encryption>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserAgentConductor,     UserAgentConductor>();


        services.AddScoped(typeof(IRepositoryConductor<>), typeof(RepositoryConductor<>));
    }
}