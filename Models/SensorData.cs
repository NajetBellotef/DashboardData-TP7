using System.ComponentModel.DataAnnotations;

namespace DashboardData.Models;

public class SensorData
{
    [Key] // Clé primaire
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom doit faire entre 3 et 50 caractères.")]
    public string Name { get; set; }

    public string Type { get; set; } = "Temperature";

    [Range(-50.0, 150.0, ErrorMessage = "La valeur doit être entre -50 et 150.")]
    public double Value { get; set; }

    public DateTime LastUpdate { get; set; } = DateTime.Now;

    public ICollection<SensorValueHistory> Values { get; set; } = new List<SensorValueHistory>();

    // Clé étrangère (Location)
    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un lieu valide.")]
    public int LocationId { get; set; }

    public Location Location { get; set; }

    // Relation N-N avec Tags
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}