using System.Text.Json.Serialization;

namespace TvMaze.Scraper.Models;

public class TvMazeApiShowModel
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [JsonPropertyName("_embedded")]
    public TvMazeApiEmbeddedCastModel EmbeddedCast { get; set; }
}

public class TvMazeApiEmbeddedCastModel
{
    [JsonPropertyName("cast")]
    public ICollection<TvMazeApiCastMemberModel> Actors { get; set; }
}

public class TvMazeApiCastMemberModel
{
    public TvMazeApiPersonModel Person { get; set; }
}

public class TvMazeApiPersonModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthday { get; set; }

}