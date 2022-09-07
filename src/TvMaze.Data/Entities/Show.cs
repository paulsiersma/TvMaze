using System.ComponentModel.DataAnnotations;

namespace TvMaze.Data.Entities;

public class Show
{
    [Key]
    public int ShowId { get; set; }
    public string? Name { get; set; }
    public List<Actor> Actors { get; set; } = new();
}