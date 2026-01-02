using Configuration;
using Configuration.Options;
using DbContext;
using DbRepos;
using Services;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
 
var builder = WebApplication.CreateBuilder(args);
 
// Razor Pages
builder.Services.AddRazorPages();
 
// Options (KRÄVS)
builder.Services.Configure<DbConnectionSetsOptions>(
    builder.Configuration.GetSection("DbConnectionSets")
);
 
// DatabaseConnections helper
builder.Services.AddSingleton<DatabaseConnections>();
 
// DbContext
builder.Services.AddDbContext<MainDbContext>((sp, options) =>
{
    var dbConn = sp.GetRequiredService<DatabaseConnections>();
    var conn = dbConn.GetDataConnectionDetails("app");
    options.UseSqlServer(conn.DbConnectionString);
});
 
// Repos
builder.Services.AddScoped<FriendsDbRepos>();
 
// Services
builder.Services.AddScoped<IFriendsService, FriendsServiceDb>();
 
var app = builder.Build();
 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
 
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
 
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
 
app.Run();