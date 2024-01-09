using XploringMe.Core.Interfaces.Utility.Security;

namespace XploringMe.BlazorApp.Extensions;
public static class IApplicationBuilderExtensions
{
    public static void ConfigureSeedData(this IApplicationBuilder _, IServiceScope serviceScope, IWebHostEnvironment environment)
    {
        var context = serviceScope.ServiceProvider.GetService<XploringMeContext>() ?? throw new InvalidOperationException("context");
        context.Database.SetCommandTimeout(10000);
        context.Database.Migrate();

        var encryption = serviceScope.ServiceProvider.GetService<IEncryption>();
        if (encryption != null)
            context.AddInitialData(encryption, environment);
    }
}
