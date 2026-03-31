namespace DashboardData.Models;

public class SensorValueHistory
{
    public int Id { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }

    // Foreign Key (Indice 2: EF Core matches this automatically to SensorData)
    public int SensorDataId { get; set; }
    
    // Navigation property
    public SensorData SensorData { get; set; } = null!;
}