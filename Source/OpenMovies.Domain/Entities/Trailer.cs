using System.Text.Json.Serialization;

namespace OpenMovies.Domain.Enitites;

public class Trailer : Entity
{

    [JsonIgnore]
    public Movie Movie { get; set; }
    public string Link { get; set; } = string.Empty;

    #pragma warning disable CS8618
    public Trailer() {  }  // Empty constructor for Entity Framework

    public Trailer(string link, Movie movie)
    {
        Link = link;
        Movie = movie;
    }

    public string GenerateEmbeddedLink()
    {
        string videoIdentifier = Link.Split("v=")[1];
        string embeddedLink = $"https://www.youtube.com/embed/{videoIdentifier}";

        return embeddedLink;
    }
}
