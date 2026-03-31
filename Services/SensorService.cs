using DashboardData.Models; // Import du namespace ou se trouve SensorData
namespace DashboardData.Services;
using DashboardData.Data;
using Microsoft.EntityFrameworkCore;

public class SensorService : ISensorService
{
    private readonly AppDbContext _context;

public SensorService(AppDbContext context)
{
    _context = context;
}

public async Task<List<SensorData>> GetSensorsAsync()
{
    // EF Core traduit Include par un JOIN SQL vers la table Location
    return await _context.Sensors
        .Include(s => s.Location)
        .ToListAsync();
}


public async Task<List<SensorData>> GetCriticalSensorsAsync(double threshold)
{
    return await _context.Sensors
        .Include(s => s.Location)
        .Where(s => s.Value > threshold) // Traduit en : WHERE Value > @threshold
        .OrderByDescending(s => s.Value) // Traduit en : ORDER BY Value DESC
        .ToListAsync();                  // Déclenche l'exécution SQL
}
public async Task<int> GetTotalCountAsync()
{
    return await _context.Sensors.CountAsync();
}
public async Task<double> GetAverageValueAsync()
{
    // Sécurité si la base est vide
    if (!await _context.Sensors.AnyAsync())
        return 0;

    return await _context.Sensors.AverageAsync(s => s.Value);
}
public async Task<double> GetMaxValueAsync()
{
    if (!await _context.Sensors.AnyAsync())
        return 0;

    return await _context.Sensors.MaxAsync(s => s.Value);
}
public async Task<List<Location>> GetLocationsAsync()
{
    return await _context.Locations.ToListAsync();
}

public async Task<SensorData?> GetSensorByIdAsync(int id)
{
    // FindAsync cherche directement par la Clé Primaire (Id)
    return await _context.Sensors.FindAsync(id);
}

public async Task AddSensorAsync(SensorData sensor)
{
    sensor.LastUpdate = DateTime.Now;
    
    // Historisation de la valeur initiale (TP5)
    sensor.Values.Add(new SensorValueHistory {
        Value = sensor.Value,
        Timestamp= DateTime.Now
    });

    _context.Sensors.Add(sensor);
    await _context.SaveChangesAsync();
}

public async Task UpdateSensorAsync(SensorData sensor)
{
    sensor.LastUpdate = DateTime.Now; // Mise à jour de la date
    
    // Ajout à l'historique lors d'une modification (TP5)
    sensor.Values.Add(new SensorValueHistory {
        Value = sensor.Value,
        Timestamp = DateTime.Now
    });

    _context.Sensors.Update(sensor);
    await _context.SaveChangesAsync();
}

public async Task DeleteSensorAsync(int id)
{
    var sensor = await _context.Sensors.FindAsync(id);
    if (sensor != null)
    {
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }
}
}
