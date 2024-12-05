using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.GraveRepo;
using Infrastructure.DeceasedRepo;
using Infrastructure.MessageRepo;
using Domain.RepositoryInterfaces;
using Domain.Services;
using Domain.ServiceInterfaces;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowOrigin",
      builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});

// Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
  options.Cookie.HttpOnly = false;
  options.Cookie.SameSite = SameSiteMode.None;
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// DbContext regisztrálása
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure")));

// Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>();

// Policy
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
  options.AddPolicy("Manager", policy => policy.RequireRole("Admin", 
    "Manager"));
  options.AddPolicy("Member", policy => policy.RequireRole("Member", 
    "Admin", "Manager"));
});

// Repository Pattern DI 
builder.Services.AddScoped<IGraveRepository, GraveRepository>();
builder.Services.AddScoped<IDeceasedRepository, DeceasedRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Service DI
builder.Services.AddScoped<IDeceasedService, DeceasedService>();
builder.Services.AddScoped<IGravesService, GravesService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();

// Ez elott lehet csak használni a builder.Services-t
var app = builder.Build();

// Role-ok létrehozása
using (var scope = app.Services.CreateScope())
{
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
  var roles = new[] { "Admin", "Manager", "Member" };

  foreach (var role in roles)
  {
    if (!await roleManager.RoleExistsAsync(role))
    {
      await roleManager.CreateAsync(new IdentityRole(role));
    }
  }
}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
