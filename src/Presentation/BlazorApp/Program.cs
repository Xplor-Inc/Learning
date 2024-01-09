var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
      .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration));
Log.Logger.Information("App is starting.....");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
//builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("XploringMe") ?? throw new InvalidOperationException("Connection string 'XploringMe' not found.");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddLogging();
builder.Services.AddDbContextPool<XploringMeContext>(
                         opentions => opentions.UseSqlServer(connectionString: connectionString));


// Custom Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlRepository();
//builder.Services.AddEmailHandler();
builder.Services.AddCookieAuthentication(builder.Configuration);
builder.Services.AddUtilityResolver();
builder.Services.AddContexts(connectionString);
builder.Services.AddConfigurationFiles(builder.Configuration);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons()
                .AddBlazoriseFluentValidation();

builder.Services.AddValidatorsFromAssembly(typeof(App).Assembly);

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

using var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
app.ConfigureSeedData(serviceScope, app.Environment);

var version = builder.Configuration.GetSection("Version").Value;
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("App-Version", version);
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapPost("/Account/Logout", async (
               ClaimsPrincipal user,
               HttpContext signInManager,
               [FromForm] string returnUrl) =>
{
    await signInManager.SignOutAsync("CA");
    return TypedResults.LocalRedirect($"~/{returnUrl}");
});
Log.Logger.Information($"App started successfully with version {version}");
app.Run();
