namespace XploringMe.BlazorApp.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddCookieAuthentication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        var cookieConfig = configuration.GetSection("Authentication:Cookie").Get<CookieAuthenticationConfiguration>() ?? throw new NullReferenceException("Provide Authentication:Cookie section in AppSettings.json");
        services.AddSingleton((sp) => cookieConfig);

        var cookie = new CookieBuilder()
        {
            Name = cookieConfig.CookieName,
            SameSite = SameSiteMode.Strict
        };
        var cookieEvents = new CookieAuthenticationEvents
        {
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Task.CompletedTask;
            },
            OnValidatePrincipal = PrincipalValidator.ValidateAsync
        };

        services.AddAuthentication(cookieConfig.AuthenticationScheme)
            .AddCookie(cookieConfig.AuthenticationScheme, options =>
            {
                options.Cookie = cookie;
                options.Events = cookieEvents;
            });
    }

    public static void AddContexts(this IServiceCollection services, string connectionString)
    {
        var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(Log.Logger, false);
        services.AddDbContext<XploringMeContext>(ServiceLifetime.Scoped);
        services.AddScoped((sp) => new XploringMeContext(connectionString, loggerFactory));
        services.AddScoped<DataContext<User>>((sp) => new XploringMeContext(connectionString, loggerFactory));
        services.AddScoped<IDataContext<User>>((sp) => new XploringMeContext(connectionString, loggerFactory));
        services.AddScoped<IContext>((sp) => new XploringMeContext(connectionString, loggerFactory));
        services.AddScoped<IXploringMeContext>((sp) => new XploringMeContext(connectionString, loggerFactory));
    }

    public static void AddConfigurationFiles(this IServiceCollection services, IConfigurationRoot configuration)
    {
        var emailSection = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>() ?? throw new NullReferenceException("Provide Authentication:Cookie section in AppSettings.json");
        services.AddSingleton((sp) => emailSection);

        var fileSection = configuration.GetSection("StaticFileConfiguration").Get<StaticFileConfiguration>() ?? throw new NullReferenceException("Provide Authentication:Cookie section in AppSettings.json");
        services.AddSingleton((sp) => fileSection);
    }
}