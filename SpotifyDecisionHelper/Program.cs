using Microsoft.EntityFrameworkCore;
using SpotifyDecisionHelper.DB;
using SpotifyDecisionHelper.DBLogic.Albums;
using SpotifyDecisionHelper.DBLogic.Artists;
using SpotifyDecisionHelper.DBLogic.Tracks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException();
services.AddDbContext<ApplicationContext>(param => param.UseSqlServer(connectionString));
services.AddScoped<IArtistsManager, ArtistsManager>();
services.AddScoped<IAlbumsManager, AlbumsManager>();
services.AddScoped<ITracksManager, TracksManager>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();