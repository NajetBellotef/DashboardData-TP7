using DashboardData.Models;
namespace DashboardData.Services;
public interface ISensorService
{
    //List<SensorData> GetSensors();
    Task<List<SensorData>> GetSensorsAsync();
    Task<List<SensorData>> GetCriticalSensorsAsync(double threshold);
    Task ReloadSensorAsync(SensorData sensor);
    Task<int> GetTotalCountAsync();
    Task<double> GetAverageValueAsync();
    Task<double> GetMaxValueAsync();
    Task<List<Location>> GetLocationsAsync();
    Task<SensorData?> GetSensorByIdAsync(int id);
    Task AddSensorAsync(SensorData sensor);
    Task UpdateSensorAsync(SensorData sensor);
    Task DeleteSensorAsync(int id);
}