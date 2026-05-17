using Domain.RepositoryInterfaces;
using Domain.Services;
using Domain.ServiceInterfaces;
using Infrastructure.Context;
using Infrastructure.DeceasedRepo;
using Infrastructure.GraveRepo;
using Infrastructure.MessageRepo;
using Infrastructure.ParcelRepo;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// --- Forwarded headers (Azure App Service Linux terminates TLS at the front door) ---
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// --- MVC + JSON ---
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// --- Swagger (enabled in all environments — thesis project, discoverability is a feature) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Memoriam API",
        Version = "v1",
        Description = "Backend for the Cemetery Management System thesis project. " +
                      "Frontend: https://memoriam.social"
    });
});

// --- CORS: exact origin + host-suffix matching ---
var exactOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                   ?? Array.Empty<string>();
var originSuffixes = builder.Configuration.GetSection("Cors:AllowedOriginSuffixes").Get<string[]>()
                     ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy => policy
        .SetIsOriginAllowed(origin =>
        {
            foreach (var o in exactOrigins)
            {
                if (string.Equals(o, origin, StringComparison.OrdinalIgnoreCase)) return true;
            }
            if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;
            foreach (var suf in originSuffixes)
            {
                if (uri.Host.EndsWith(suf, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithExposedHeaders("Total-Count", "Page-Number", "Page-Size", "Total-Pages"));
});

// --- Identity cookie ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
    options.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };
});

// --- DbContext ---
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npg => npg.MigrationsAssembly("Infrastructure")));

// --- Identity ---
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", p => p.RequireRole("Admin"));
    options.AddPolicy("Manager", p => p.RequireRole("Admin", "Manager"));
    options.AddPolicy("Member", p => p.RequireRole("Member", "Admin", "Manager"));
});

// --- Health checks ---
builder.Services.AddHealthChecks()
    .AddDbContextCheck<DatabaseContext>("db", tags: new[] { "ready" });

// --- Image storage: R2 in production, local filesystem in development ---
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<LocalImageStorageOptions>(builder.Configuration.GetSection("LocalStorage"));
    builder.Services.PostConfigure<LocalImageStorageOptions>(o =>
    {
        if (!Path.IsPathRooted(o.RootPath))
            o.RootPath = Path.Combine(builder.Environment.ContentRootPath, o.RootPath);
    });
    builder.Services.AddSingleton<IImageStorage, LocalImageStorage>();
}
else
{
    builder.Services.Configure<R2Options>(builder.Configuration.GetSection("R2"));
    builder.Services.AddSingleton<IImageStorage, R2ImageStorage>();
}

// --- Repository Pattern DI ---
builder.Services.AddScoped<IGraveRepository, GraveRepository>();
builder.Services.AddScoped<IDeceasedRepository, DeceasedRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IParcelRepository, ParcelRepository>();

// --- Service DI ---
builder.Services.AddScoped<IDeceasedService, DeceasedService>();
builder.Services.AddScoped<IGravesService, GravesService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IParcelsService, ParcelsService>();

var app = builder.Build();

// --- Startup migration + role seed (Production and Development only) ---
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        logger.LogInformation("Applying EF Core migrations...");
        await db.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "Manager", "Member" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        logger.LogInformation("Startup migration + role seed complete.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Startup migration/seed failed. Aborting.");
        throw;
    }
}

// --- Middleware pipeline ---
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errApp => errApp.Run(async ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
        ctx.Response.ContentType = "application/problem+json";
        await ctx.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = 500,
            Title = "An unexpected error occurred."
        });
    }));
}

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    var uploadsRoot = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "uploads");
    Directory.CreateDirectory(uploadsRoot);
    app.UseStaticFiles();
}

app.UseCors("Default");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready")
});

app.MapControllers();

app.Run();
