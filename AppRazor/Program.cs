using Services;
using Encryption;
using Services.Interfaces;
using DbRepos;
using DbContext;
using Microsoft.EntityFrameworkCore;
using Encryption.Options;

var builder = WebApplication.CreateBuilder(args);

// --- Bind AesEncryptionOptions från user-secrets ---
builder.Services.Configure<AesEncryptionOptions>(
    builder.Configuration.GetSection("AesEncryption")); 

// --- Lägg till Encryptions som Scoped (så DI fungerar korrekt) ---
builder.Services.AddScoped<Encryptions>();

// Lägg till Razor Pages
builder.Services.AddRazorPages();

// Lägg till DbContext (MainDbContext) med standard-konfiguration
builder.Services.AddDbContext<MainDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("sql-friends.sqlserver.docker.root");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException("Connection string not found in User Secrets");

    options.UseSqlServer(connStr, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

// Lägg till Repos
builder.Services.AddScoped<FriendsDbRepos>();
builder.Services.AddScoped<AddressesDbRepos>();
builder.Services.AddScoped<PetsDbRepos>();
builder.Services.AddScoped<QuotesDbRepos>();
builder.Services.AddScoped<AdminDbRepos>();

// Lägg till Services
builder.Services.AddScoped<IFriendsService, FriendsServiceDb>();
builder.Services.AddScoped<IAddressesService, AddressesServiceDb>();
builder.Services.AddScoped<IPetsService, PetsServiceDb>();
builder.Services.AddScoped<IQuotesService, QuotesServiceDb>();
builder.Services.AddScoped<IAdminService, AdminServiceDb>();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // standard statiska filer från wwwroot

app.UseRouting();
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

app.Run();
