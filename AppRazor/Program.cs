using Services;
using Services.Interfaces;
using DbRepos;
using DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Lägg till Services
builder.Services.AddScoped<IFriendsService, FriendsServiceDb>();
builder.Services.AddScoped<IAddressesService, AddressesServiceDb>();
builder.Services.AddScoped<IPetsService, PetsServiceDb>();
builder.Services.AddScoped<IQuotesService, QuotesServiceDb>();

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
