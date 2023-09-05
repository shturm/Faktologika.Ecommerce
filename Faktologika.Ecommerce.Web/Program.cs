using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Faktologika.Ecommerce.Web.Data;
using Microsoft.Extensions.DependencyInjection;
using Faktologika.Ecommerce.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var usersConnectionString = builder.Configuration.GetConnectionString("UsersConnection")
    ?? throw new InvalidOperationException("Connection string 'UsersConnection' not found.");
var catalogConnectionString = builder.Configuration.GetConnectionString("CatalogConnection")
    ?? throw new InvalidOperationException("Connection string 'CatalogConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseSqlite(catalogConnectionString,
        x => x.MigrationsHistoryTable("__CatalogMigrations", "catalogSchema")); // differentiates between migrations of different contexts
});
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseSqlite(usersConnectionString,
        x => x.MigrationsHistoryTable("__IdentityMigrations", "identitySchema")); // differentiates between migrations of different contexts
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UsersDbContext>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RefreshBeforeValidation = true,
            LogTokenId = true,
            LogValidationExceptions = true,

        };
    });

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    // swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        }
    );
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // was here before Jwt

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var usersDb = services.GetRequiredService<UsersDbContext>();
    var catalogDb = services.GetRequiredService<CatalogDbContext>();

    usersDb.Database.EnsureCreated();
    catalogDb.Database.EnsureCreated();
}

app.Run();
