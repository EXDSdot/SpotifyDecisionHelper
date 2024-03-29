using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DBLogic.Albums;
using SpotifyDecisionHelper.DBLogic.Artists;
using SpotifyDecisionHelper.DBLogic.Brackets;
using SpotifyDecisionHelper.DBLogic.Matches;
using SpotifyDecisionHelper.DBLogic.Tracks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();

services.AddDistributedMemoryCache();

services.AddSession(options =>
{
    options.Cookie.Name = ".SpotifyDecisionHelper.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException();
services.AddDbContext<ApplicationContext>(param => param.UseSqlServer(connectionString));
services.AddScoped<IArtistsManager, ArtistsManager>();
services.AddScoped<IAlbumsManager, AlbumsManager>();
services.AddScoped<ITracksManager, TracksManager>();
services.AddScoped<IMatchesManager, MatchesManager>();
services.AddScoped<IBracketsManager, BracketsManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();