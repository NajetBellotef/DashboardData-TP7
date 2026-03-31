using DashboardData.Components;
using DashboardData.Services;
using DashboardData.Models;
using Microsoft. EntityFrameworkCore;
using DashboardData.Data;



var builder = WebApplication.CreateBuilder(args);

var connectionString = builder. Configuration. GetConnectionString("DefaultConnection");
builder. Services. AddDbContext<AppDbContext>(options =>
options.UseSqlite(connectionString));
// pour que le compteur revienne à zéro si on rafraîchit la page (F5)
builder.Services.AddScoped<UserCounterService>();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Teste d'abord avec Singleton

builder.Services.AddScoped<ISensorService,SensorService>();
//builder.Services.AddSingleton<UserCounterService>();
var app = builder.Build();

// Creation d'un scope temporaire pour recuperer les services
using (var scope = app.Services.CreateScope())
{
var services = scope.ServiceProvider;
var context = services. GetRequiredService<AppDbContext>();

if (!context.Sensors.Any())
{
Console.WriteLine(" --- Generation de donnees de test --- ");

// 1. Creer les Emplacements
var labo = new Location { Name = "Labo", Building = "Bât. A" };
var usine = new Location { Name = "Usine", Building = "Bât. B" };
context. Locations.AddRange(labo, usine);

// 2. Creer les Tags
var tagCritique = new Tag { Label = "Critique" };
var tagMaintenance = new Tag { Label = "Maintenance" };
context.Tags.AddRange(tagCritique, tagMaintenance);
context.SaveChanges(); // Sauvegarder AVANT pour genérer les Id

// 3. Creer les Capteurs avec relations
var sonde1 = new SensorData
{
Name = "Sonde_Alpha", Value = 25.4,
LocationId = labo. Id,
Tags = new List<Tag> { tagCritique }
};
var sonde2 = new SensorData
{
Name = "Sonde_Beta", Value = 40.2,
LocationId = usine. Id,
Tags = new List<Tag> { tagCritique, tagMaintenance }
};
context.Sensors.AddRange(sonde1, sonde2);
context.SaveChanges();

}

}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
