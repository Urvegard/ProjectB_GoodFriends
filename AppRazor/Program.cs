// using Services;
// using Services.Interfaces;

using Microsoft.Extensions.Logging;
using Services;
using Services.Interfaces;
using DbRepos;
using Configuration.Extensions;
using DbContext.Extensions;
using Encryption.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Logging & Razor Pages
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddLogging();
builder.Configuration.AddSecrets(builder.Environment);

// Encryption + database
builder.Services.AddEncryptions(builder.Configuration);
builder.Services.AddDatabaseConnections(builder.Configuration);
builder.Services.AddUserBasedDbContext();

// Repos
builder.Services.AddScoped<FriendsDbRepos>();
builder.Services.AddScoped<AddressesDbRepos>();
builder.Services.AddScoped<PetsDbRepos>();
builder.Services.AddScoped<QuotesDbRepos>();

// Services 
builder.Services.AddScoped<IFriendsService, FriendsServiceDb>();
builder.Services.AddScoped<IAddressesService, AddressesServiceDb>();
builder.Services.AddScoped<IPetsService, PetsServiceDb>();
builder.Services.AddScoped<IQuotesService, QuotesServiceDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();


// Razor Page
//    ↓
// Service (IFriendsService)
//    ↓
// Repo (FriendsDbRepos)
//    ↓
// DbContext
//    ↓
// Database
