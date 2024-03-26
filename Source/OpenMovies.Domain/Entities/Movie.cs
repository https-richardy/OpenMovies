using System.Text.Json.Serialization;

namespace OpenMovies.Models;


public class Movie : Entity
{
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDateOf { get; set; }
    public int ReleaseYear { get; set; }
    public string Synopsis { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Trailer>? Trailers { get; set; } = new List<Trailer>();

    public Director Director { get; set; }
    public Category Category { get; set; }

    public string CoverImagePath { get; set; } = string.Empty;

    # pragma warning disable CS8618
    public Movie() {  } // Empty constructor for Entity Framework

    # pragma warning restore
    public Movie(string title, DateTime releaseDateOf, string synopsis, Director director, Category category)
    {
        Title = title;
        ReleaseDateOf = releaseDateOf;
        ReleaseYear = releaseDateOf.Year;
        Synopsis = synopsis;
        Director = director;
        Category = category;
    }
}