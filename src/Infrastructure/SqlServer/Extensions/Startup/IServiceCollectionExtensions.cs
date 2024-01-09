using XploringMe.SqlServer.Repositories;
using XploringMe.Core.Interfaces.Data;

namespace XploringMe.SqlServer.Extensions;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSqlRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}