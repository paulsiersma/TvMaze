using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TvMaze.Data.Entities;

public class Actor
{
    [Key]
    public int ActorId { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthday { get; set; }

    [JsonIgnore]
    public List<Show> Shows { get; set; } = new();
}