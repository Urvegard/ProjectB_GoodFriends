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
    // Här kan du hämta connection string från user secrets eller appsettings
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connString); // eller UseMySql / UseNpgsql beroende på din DB
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
